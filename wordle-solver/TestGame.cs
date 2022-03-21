using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace wordle_solver
{
    public class TestGame
    {
        private const int GUESS_COUNT = 6;
        private const int TEST_SIZE = 2000;
        private readonly IList<string> _allWords;
        private readonly LetterDistribution _startingLetterDistribution;
        private readonly IList<WordElement> _startingWordOptions;

        public TestGame(IEnumerable<string> words)
        {
            _allWords = words.ToList();
            _startingLetterDistribution = new LetterDistribution(words);
            _startingWordOptions = WordElement.GetWordElementList(words, _startingLetterDistribution);
        }

        public void RunTest()
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine($"Running test: {TEST_SIZE} words");
            var testWords = _allWords.Take(TEST_SIZE);
            var results = new ResultDistribution(GUESS_COUNT);

            int[] scoreCounts = new int[6];
            int misses = 0;

            Parallel.ForEach(testWords, word =>
            {
                var score = PlayGame(word);
                if (score.HasValue)
                    Interlocked.Increment(ref scoreCounts[score.Value - 1]);
                else
                    Interlocked.Increment(ref misses);
            });

            results.Misses = misses;
            for (int i = 0; i < scoreCounts.Length; i++)
                results.ScoreCount[i + 1] = scoreCounts[i];

            Console.Write(results);
            Console.WriteLine($"Time: {sw.Elapsed} ms");
        }

        public int? PlayGame(string target)
        {
            var options = new PossibleWords(
                _allWords, _startingLetterDistribution,
                _startingWordOptions, GUESS_COUNT);

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