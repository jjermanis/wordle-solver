using System;
using System.Collections.Generic;
using System.Linq;

namespace wordle_solver
{
    public class TestGame
    {
        private const int GUESS_COUNT = 6;
        private const int TEST_SIZE = 2000;
        private readonly bool SHOW_WORD_DETAILS = false;

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
            int start = Environment.TickCount;
            Console.WriteLine($"Running test: {TEST_SIZE} words");
            var testWords = _allWords.Take(TEST_SIZE);
            var results = new ResultDistribution(GUESS_COUNT);
            if (SHOW_WORD_DETAILS)
                Console.WriteLine($"First guess: {_startingWordOptions.First()}");
            foreach (var word in testWords)
            {
                var score = PlayGame(word);
                if (score.HasValue)
                    results.ScoreCount[score.Value]++;
                else
                {
                    results.Misses++;
                    if (SHOW_WORD_DETAILS)
                        Console.WriteLine($"Missed: {word}");
                }
            }
            Console.Write(results);
            Console.WriteLine($"Time: {Environment.TickCount - start} ms");
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