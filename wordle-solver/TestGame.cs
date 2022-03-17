using System;
using System.Collections.Generic;
using System.Linq;

namespace wordle_solver
{
    public class TestGame
    {
        private const int TEST_SIZE = 2000;
        private readonly IList<string> _words;

        private class ResultDist
        {
            public ResultDist()
            {
                ScoreCount = new int[7];
            }

            public int[] ScoreCount { get; set; }
            public int Misses { get; set; }

            private decimal Average()
            {
                decimal total = 0;
                decimal cases = 0;
                for (int i = 1; i <= 6; i++)
                {
                    total += i * ScoreCount[i];
                    cases += ScoreCount[i];
                }
                return total / cases;
            }

            public override string ToString()
            {
                string result = "";
                for (int i = 1; i <= 6; i++)
                    result += $"{i}: {ScoreCount[i]}\r\n";
                result += $"Misses: {Misses}\r\n";
                result += $"Average: {Average()}\r\n";
                return result;
            }
        }

        public TestGame(IEnumerable<string> words)
        {
            _words = words.ToList();
        }

        public void RunTest()
        {
            Console.WriteLine($"Running test: {TEST_SIZE} words");
            var testWords = _words.Take(TEST_SIZE);
            var results = new ResultDist();
            foreach (var word in testWords)
            {
                var score = PlayGame(word);
                if (score.HasValue)
                    results.ScoreCount[score.Value]++;
                else
                    results.Misses++;
            }
            Console.Write(results);
        }

        public int? PlayGame(string target)
        {
            var options = new PossibleWords(_words);

            for (int i = 0; i < 6; i++)
            {
                var currGuess = options.BestGuess();
                var result = GetResult(currGuess, target);
                if (result == "GGGGG")
                {
                    return i + 1;
                }
                options.AddClue(currGuess, result);
            }
            return null;
        }

        private string GetResult(string guess, string target)
            => PossibleWords.CalcResult(guess, target);
    }
}