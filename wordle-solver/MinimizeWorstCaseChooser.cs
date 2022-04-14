using System;
using System.Collections.Generic;
using System.Linq;

namespace wordle_solver
{
    public class MinimizeWorstCaseChooser : IWordChooser
    {
        private readonly IList<string> _allWords;
        private IList<string> _remainingWords;

        public MinimizeWorstCaseChooser(IEnumerable<string> words)
        {
            _allWords = new List<string>(words);
            _remainingWords = new List<string>(words);
        }

        public string BestGuess()
        {
            if (_remainingWords.Count == 1)
                return _remainingWords[0];

            // TODO cleanup this optimization
            // TODO consider caching all 2nd turn guesses
            if (_remainingWords.Count > 3000)
                return "aloes";

            // TODO bias towards possible answers - check those first
            // TODO weight words based on probability on being a possible answer
            string bestWord = null;
            var bestWorstCaseCount = Int32.MaxValue;
            foreach (var word in _allWords)
            {
                var worstCaseCount = WordCaseCount(word);
                if (worstCaseCount < bestWorstCaseCount)
                {
                    bestWord = word;
                    bestWorstCaseCount = worstCaseCount;
                }
            }
            return bestWord;
        }

        public void UpdateAfterGuess(string guess, string result)
        {
            var newList = new List<string>();
            foreach (var word in _remainingWords)
            {
                if (IsWordValid(guess, result, word))
                    newList.Add(word);
            }
            _remainingWords = newList;
        }

        private int WordCaseCount(string word)
        {
            var counts = new Dictionary<string, int>();

            foreach (var candidate in _remainingWords)
            {
                var result = CalcResult(word, candidate);
                if (counts.ContainsKey(result))
                    counts[result]++;
                else
                    counts[result] = 1;
            }

            return counts.Values.Max();
        }

        // TODO this is duplicated code. Refactor into common class
        private static string CalcResult(string guess, string word)
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

        // TODO this is duplicated code. Refactor into common class
        private static bool IsWordValid(string guess, string result, string word)
        {
            // Optimization: do a simple check first. For each char, if the result is G, they
            // need to match, and if the result isn't G, they need to NOT match.
            for (int x = 0; x < 5; x++)
                if ((result[x] == 'G') == (guess[x] != word[x]))
                    return false;

            // Word is valid if it would generate the same result as the most recent guess
            return result.Equals(CalcResult(guess, word));
        }
    }
}