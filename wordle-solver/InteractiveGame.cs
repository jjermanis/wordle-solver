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
            var guesses = new string[6];

            Console.WriteLine("Welcome to wordle-solver. This program will help you solve the daily Wordle puzzle.");
            Console.WriteLine("To use, guess what this program suggests. Then, let this program know the result.");
            Console.WriteLine("Enter the five colors from the result. G for green, Y for yellow, and X for gray.");

            for (int i = 0; i < GUESS_COUNT; i++)
            {
                var currGuess = options.BestGuess();

                if (currGuess == null)
                {
                    options = ReplayGuesses(guesses);
                    currGuess = options.BestGuess();
                }

                guesses[i] = currGuess;
                var result = PromptResult(currGuess);
                if (result == "GGGGG")
                {
                    Console.WriteLine("Congrats!");
                    return;
                }
                options.UpdateGuess(currGuess, result);
            }
            Console.WriteLine("Looks like you ran out of guesses. My fault.");
        }

        private static string PromptResult(string word)
        {
            Console.WriteLine($"Please guess: {word}");
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

        private PossibleWords ReplayGuesses(string[] previousGuesses)
        {
            Console.WriteLine("Hmmm. I do not see any valid guesses for you. Maybe this happened because you");
            Console.WriteLine("made a mistake entering in the results after a guess. I'm going to ask for the");
            Console.WriteLine("results for each guess again. Please enter each result and we'll see if it helps.");

            var options = new PossibleWords(_words, GUESS_COUNT);
            foreach (var guess in previousGuesses)
            {
                if (guess == null)
                    break;
                var result = PromptResult(guess);
                options.UpdateGuess(guess, result);
            }
            if (options.BestGuess() == null)
            {
                Console.WriteLine("I still do not have a guess for you. Sorry - I might have a bug.");
                throw new Exception();
            }
            return options;
        }
    }
}