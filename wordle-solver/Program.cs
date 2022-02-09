using System;
using System.IO;

namespace wordle_solver
{
    internal class Program
    {
        private const string FILE_PATH = @"..\..\..\words.txt";

        private static void Main(string[] args)
        {
            var words = File.ReadLines(FILE_PATH);
            var options = new PossibleWords(words);
            Console.WriteLine("Welcome to wordle-solver. This program will help you solve the daily Wordle puzzle.");
            Console.WriteLine("To use, guess what this program suggests. Then, let this program know the result.");
            Console.WriteLine("Enter the five colors from the result. G for green, Y for yellow, and X for gray.");

            for (int i = 0; i < 6; i++)
            {
                var currGuess = options.BestGuess();
                Console.WriteLine($"Please guess: {currGuess}");
                Console.Write("Result? ");
                var result = Console.ReadLine().ToUpper();
                if (result == "GGGGG")
                {
                    Console.WriteLine("Congrats!");
                    return;
                }
                options.AddClue(currGuess, result);
            }
            Console.WriteLine("Looks like you ran out of guesses. My fault.");
        }
    }
}