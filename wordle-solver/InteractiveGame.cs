using System;
using System.Collections.Generic;

namespace wordle_solver
{
    internal class InteractiveGame
    {
        private const int GUESS_COUNT = 6;

        private readonly IEnumerable<string> _words;

        public InteractiveGame(IEnumerable<string> words)
        {
            _words = words;
        }

        public void PlayGame()
        {
            var options = new PossibleWords(_words, GUESS_COUNT);
            Console.WriteLine("Welcome to wordle-solver. This program will help you solve the daily Wordle puzzle.");
            Console.WriteLine("To use, guess what this program suggests. Then, let this program know the result.");
            Console.WriteLine("Enter the five colors from the result. G for green, Y for yellow, and X for gray.");

            for (int i = 0; i < GUESS_COUNT; i++)
            {
                var currGuess = options.BestGuess();
                Console.WriteLine($"Please guess: {currGuess}");
                var result = PromptResult();
                if (result == "GGGGG")
                {
                    Console.WriteLine("Congrats!");
                    return;
                }
                options.UpdateGuess(currGuess, result);
            }
            Console.WriteLine("Looks like you ran out of guesses. My fault.");
        }

        private static string PromptResult()
        {
            while (true)
            {
                Console.Write("Result? ");
                var result = Console.ReadLine().ToUpper();
                if (!PossibleWords.IsValidResult(result))
                    Console.WriteLine("Incorrect format. Results should be five letters long. G for green, Y for yellow, and X for gray.");
                else
                    return result;
            }
        }
    }
}