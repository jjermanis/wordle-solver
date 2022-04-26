using System;
using System.Collections.Generic;

namespace wordle_solver
{
    public class MinimizeExpectedRemainingCasesChooser : IWordChooser
    {
        // Optimization to avoid lengthy calculation of first guess - it's pre-calculated
        private const string OPENING_GUESS = "tales";

        private readonly IList<string> _allWords;
        private readonly int _totalGuesses;
        private readonly bool _isHardMode;
        private readonly string _firstGuess;

        private IList<string> _remainingWords;
        private int _remainingGuesses;

        public MinimizeExpectedRemainingCasesChooser(
            IEnumerable<string> words,
            int totalGuesses,
            bool isHardMode) : this (words, totalGuesses, isHardMode, OPENING_GUESS)
        {

        }

        public MinimizeExpectedRemainingCasesChooser(
            IEnumerable<string> words,
            int totalGuesses,
            bool isHardMode,
            string overrideFirstGuess)
        {
            _allWords = new List<string>(words);
            _totalGuesses = totalGuesses;
            _isHardMode = isHardMode;
            _firstGuess = overrideFirstGuess;

            _remainingWords = new List<string>(words);
            _remainingGuesses = totalGuesses;
        }

        public string BestGuess()
        {
            if (_remainingWords.Count == 1)
                return _remainingWords[0];

            // TODO consider caching all 2nd turn guesses
            if (_remainingGuesses == _totalGuesses)
                return _firstGuess;

            // TODO weight words based on probability on being a possible answer
            string bestWord = null;
            var bestExpectedCount = Decimal.MaxValue;
            (bestWord, bestExpectedCount) = FindBestWord(_remainingWords, bestWord, bestExpectedCount);
            if (bestExpectedCount < 1)
                return bestWord;

            if (!_isHardMode && _remainingGuesses > 1)
                (bestWord, _) = FindBestWord(_allWords, bestWord, bestExpectedCount);

            return bestWord;
        }

        public void UpdateAfterGuess(string guess, string result)
        {
            var newList = new List<string>();
            foreach (var word in _remainingWords)
            {
                if (WordleUtil.IsWordValid(guess, result, word))
                    newList.Add(word);
            }
            _remainingWords = newList;
            _remainingGuesses--;
        }

        private (string bestWord, decimal worstCaseCount) FindBestWord(
            IEnumerable<string> words,
            string currBestWord,
            decimal currExpectedCaseCount)
        {
            var bestWord = currBestWord;
            var bestExpectedCaseCount = currExpectedCaseCount;

            foreach (var word in words)
            {
                var expectedCaseCount = ExpectedCaseCount(word);
                if (expectedCaseCount < bestExpectedCaseCount)
                {
                    bestWord = word;
                    bestExpectedCaseCount = expectedCaseCount;
                }
            }
            return (bestWord, bestExpectedCaseCount);
        }

        private decimal ExpectedCaseCount(string word)
        {
            var counts = new Dictionary<string, int>();

            foreach (var candidate in _remainingWords)
            {
                var result = WordleUtil.CalcResult(word, candidate);
                if (counts.ContainsKey(result))
                    counts[result]++;
                else
                    counts[result] = 1;
            }
            if (counts.ContainsKey("GGGGG"))
                counts.Remove("GGGGG");

            var temp = 0M;
            foreach (var resultValue in counts.Keys)
                temp += counts[resultValue] * counts[resultValue];
            return temp / _remainingWords.Count;
        }
    }
}