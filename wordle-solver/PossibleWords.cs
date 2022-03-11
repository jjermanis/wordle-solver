using System.Collections.Generic;
using System.Linq;

namespace wordle_solver
{
    public class PossibleWords
    {
        private static readonly HashSet<char> VALID_RESULT_CHARS = new HashSet<char> { 'G', 'Y', 'X' };
        private readonly LetterDistribution _dist;
        private List<string> _options;

        public PossibleWords(IEnumerable<string> words)
        {
            _dist = new LetterDistribution(words);
            _options = words.OrderByDescending(w => Score(w)).ToList();
        }

        public long Score(string word)
        {
            var alreadySeen = new HashSet<char>();
            var result = 1L;

            foreach (var c in word)
            {
                if (alreadySeen.Contains(c))
                    continue;
                alreadySeen.Add(c);
                result *= _dist.Frequency[c];
            }
            return result;
        }

        public string BestGuess()
            => _options.FirstOrDefault();

        public static bool IsValidResult(string result)
        {
            if (result.Length != 5)
                return false;
            foreach (var c in result)
                if (!VALID_RESULT_CHARS.Contains(c))
                    return false;
            return true;
        }

        public void AddClue(string guess, string result)
        {
            var newList = new List<string>();
            foreach (var word in _options)
                if (IsWordValid(guess, result, word))
                    newList.Add(word);

            _options = newList;
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