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

            var commandArgs = new CommandLineArgs(args);

            switch (commandArgs.Action)
            {
                case GameActions.TestEngine:
                    new TestGame(words, commandArgs.IsHardMode).RunTest();
                    break;

                case GameActions.DictionaryCheckIllegal:
                    new DictionaryCheck(words).IllegalWordCheck();
                    break;

                case GameActions.DictionaryCheckLegal:
                    new DictionaryCheck(words).PossibleAnswersCheck();
                    break;

                case GameActions.Interactive:
                    new InteractiveGame(words, commandArgs.IsHardMode).PlayGame();
                    break;

                case GameActions.Help:
                case GameActions.Error:
                    Console.WriteLine(commandArgs.Docs);
                    break;

                default:
                    throw new Exception("Internal error - no action to perform");
            }
        }
    }
}