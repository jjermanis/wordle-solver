using System;

namespace wordle_solver
{
    public class WordleUtil
    {
        public static string CalcResult(string guess, string word)
        {
            //var result = Enumerable.Repeat('X', 5).ToArray();
            Span<char> result = stackalloc char[5];
            Span<int> remainingLetters = stackalloc int[27];

            // Check for greens first
            for (var x = 0; x < 5; x++)
            {
                if (guess[x] == word[x])
                    result[x] = 'G';
                else
                {
                    result[x] = 'X';
                    remainingLetters[word[x] - 'a']++;
                }
            }

            // Now check for yellows
            for (var x = 0; x < 5; x++)
            {
                if (result[x] != 'G' && remainingLetters[guess[x]-'a'] > 0)
                {
                    result[x] = 'Y';
                    remainingLetters[guess[x] - 'a']--;
                }
            }

            return new string(result);
        }

        public static bool IsWordValid(string guess, string result, string word)
            // Word is valid if it would generate the same result as the most recent guess
            => result.Equals(CalcResult(guess, word));

    }
}