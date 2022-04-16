using System.Collections.Generic;
using System.Linq;

namespace wordle_solver
{
    public class WordleUtil
    {
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