using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordle_solver
{
    public class TestGame
    {
        private const int GUESS_COUNT = 6;
        private readonly bool SHOW_WORD_DETAILS = false;

        private readonly IList<string> _allWords;
        private readonly bool _isHardMode;

        private readonly LetterDistribution _startingLetterDistribution;
        private readonly IList<WordElement> _startingWordOptions;

        public TestGame(IEnumerable<string> words, bool isHardMode)
        {
            _allWords = words.ToList();
            _isHardMode = isHardMode;
            _startingLetterDistribution = new LetterDistribution(words);
            _startingWordOptions = WordElement.GetWordElementList(words, _startingLetterDistribution);
        }

        public void RunTests()
        {
            RunTestCase(2000, 5);
            RunTestCase(4000, 3);
        }

        private void RunTestCase(int testSize, int testCaseIterations)
        {
            Console.WriteLine($"Running test: {testSize} words");
            if (SHOW_WORD_DETAILS)
                Console.WriteLine($"First guess: {_startingWordOptions.First()}");
            var results = new List<ResultDistribution>(testCaseIterations);
            for (var x=0; x < testCaseIterations; x++)
            {
                results.Add(RunSingleCase(testSize));
            }

            // TODO - confirm all results match
            results.OrderBy(r => r.Duration);
            var median = testCaseIterations / 2;
            Console.WriteLine(results[median]);
        }

        private ResultDistribution RunSingleCase(int testSize)
        {
            var start = Environment.TickCount;
            var result = new ResultDistribution(GUESS_COUNT);
            var testWords = _allWords.Take(testSize);

            Parallel.ForEach(testWords, word =>
            {
                var score = PlayGame(word);
                if (score.HasValue)
                    result.ScoreCount[score.Value]++;
                else
                    result.Misses++;
            });

            result.Duration = Environment.TickCount - start;
            return result;
        }

        public int? PlayGame(string target)
        {
            var options = new PossibleWords(
                _allWords, GUESS_COUNT, _isHardMode,
                _startingLetterDistribution, _startingWordOptions);

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