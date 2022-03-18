using System;
using System.Collections.Generic;
using System.Linq;

namespace wordle_solver
{
    public class TestGame
    {
        private const int GUESS_COUNT = 6;
        private const int TEST_SIZE = 2000;
        private readonly IList<string> _words;

        private class ResultDist
        {
            public ResultDist()
            {
                ScoreCount = new int[GUESS_COUNT + 1];
            }

            public int[] ScoreCount { get; set; }
            public int Misses { get; set; }

            private decimal Average()
            {
                decimal total = 0;
                decimal cases = 0;
                for (int i = 1; i <= GUESS_COUNT; i++)
                {
                    total += i * ScoreCount[i];
                    cases += ScoreCount[i];
                }
                return total / cases;
            }

            private decimal Score()
            {
                decimal total = 0;
                decimal cases = 0;
                for (int i = 1; i <= GUESS_COUNT; i++)
                {
                    total += i * ScoreCount[i];
                    cases += ScoreCount[i];
                }
                total += Misses * 10;

                return total / cases;
            }

            private decimal WinRate()
            {
                decimal cases = 0;
                for (int i = 1; i <= GUESS_COUNT; i++)
                {
                    cases += ScoreCount[i];
                }
                return cases / (cases + Misses);
            }

            public override string ToString()
            {
                string result = "";
                for (int i = 1; i <= GUESS_COUNT; i++)
                    result += $"{i}: {ScoreCount[i]}\r\n";
                result += $"Misses: {Misses}\r\n";
                result += $"Win Rate: {WinRate().ToString("P3")}\r\n";
                result += $"Average: {Average().ToString("0.000")}\r\n";
                result += $"Score: {Score().ToString("0.000")}\r\n";
                return result;
            }
        }

        public TestGame(IEnumerable<string> words)
        {
            _words = words.ToList();
        }

        public void RunTest()
        {
            int start = Environment.TickCount;
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
            Console.WriteLine($"Time: {Environment.TickCount - start} ms");
        }

        public int? PlayGame(string target)
        {
            var options = new PossibleWords(_words, GUESS_COUNT);

            for (int i = 0; i < GUESS_COUNT; i++)
            {
                var currGuess = options.BestGuess();
                var result = GetResult(currGuess, target);
                if (result == "GGGGG")
                {
                    return i + 1;
                }
                options.UpdateGuess(currGuess, result);
            }
            return null;
        }

        private string GetResult(string guess, string target)
            => PossibleWords.CalcResult(guess, target);
    }
}