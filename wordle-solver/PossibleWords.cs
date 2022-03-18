using System;
using System.Collections.Generic;
using System.Linq;

namespace wordle_solver
{
    public class PossibleWords
    {
        private static readonly HashSet<char> VALID_RESULT_CHARS = new HashSet<char> { 'G', 'Y', 'X' };

        private readonly List<string> _allWords;
        private int _remainingGuesses;
        private LetterDistribution _dist;
        private int? _remainingLetterIndex;
        private List<WordElement> _options;

        public PossibleWords(IEnumerable<string> words, int totalGuesses)
        {
            _allWords = new List<string>(words);
            _dist = new LetterDistribution(words);
            _remainingGuesses = totalGuesses;

            _options = new List<WordElement>();
            var rank = 1;
            foreach (var word in words)
            {
                var popularScore = Math.Max(5 - (rank / 1000), 1);
                _options.Add(new WordElement(word, popularScore));
                rank++;
            }
            _options = _options.OrderByDescending(o => o.CalcScore(_dist)).ToList();
        }

        public string BestGuess()

        {
            if (!_remainingLetterIndex.HasValue
                || _options.Count <= 2
                || _remainingGuesses == 1)
                return _options.FirstOrDefault()?.Word;
            else
                return SearchingGuess();
        }

        public string SearchingGuess()
        {
            var candidateLetters = new Dictionary<char, int>();
            foreach (var word in _options)
                candidateLetters[word.Word[_remainingLetterIndex.Value]] = word.CommonWordScore * 2;

            var bestScore = 1;
            var bestWord = "";
            foreach (var word in _allWords)
            {
                var currScore = 1;
                var currCandidates = new Dictionary<char, int>(candidateLetters);
                foreach (var c in word)
                    if (currCandidates.ContainsKey(c))
                    {
                        currScore *= currCandidates[c];
                        currCandidates.Remove(c);
                    }
                if (currScore > bestScore)
                {
                    bestScore = currScore;
                    bestWord = word;
                }
            }
            return bestWord;
        }

        public static bool IsValidResult(string result)
        {
            if (result.Length != 5)
                return false;
            foreach (var c in result)
                if (!VALID_RESULT_CHARS.Contains(c))
                    return false;
            return true;
        }

        public void UpdateGuess(string guess, string result)
        {
            _remainingGuesses--;

            if (result.Count(c => c == 'G') == 4)
                _remainingLetterIndex = result.IndexOf('X');

            var newList = new List<WordElement>();
            foreach (var element in _options)
            {
                var word = element.Word;
                if (IsWordValid(guess, result, word))
                    newList.Add(element);
            }
            _options = newList;
            _dist = new LetterDistribution(_options.Select(o => o.Word));
            _options = _options.OrderByDescending(o => o.CalcScore(_dist)).ToList();
        }

        public static string CalcResult(string guess, string word)
        {
            var result = Enumerable.Repeat('X', 5).ToArray();
            var remainingLetters = new List<char>();

            // Check for greens first
            for (var x = 0; x < 5; x++)
            {
                if (guess[x] == word[x])
                    result[x] = 'G';
                else
                    remainingLetters.Add(word[x]);
            }

            // Now check for yellows
            for (var x = 0; x < 5; x++)
            {
                if (result[x] != 'G' && remainingLetters.Contains(guess[x]))
                {
                    result[x] = 'Y';
                    remainingLetters.Remove(guess[x]);
                }
            }

            return new string(result);
        }

        public static bool IsWordValid(string guess, string result, string word)
            // Word is valid if it would generate the same result as the most recent guess
            => result.Equals(CalcResult(guess, word));
    }
}