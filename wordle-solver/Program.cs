using System.IO;

namespace wordle_solver
{
    internal class Program
    {
        private const string INTERACTIVE_ARG = "-p";
        private const string TEST_ARG = "-t";
        private const string ILLEGAL_CHECK_ARG = "-i";
        private const string LEGAL_CHECK_ARG = "-l";

        private const string FILE_PATH = @"..\..\..\words.txt";

        private static void Main(string[] args)
        {
            var words = File.ReadLines(FILE_PATH);

            var arg = args.Length > 0 ? args[0] : INTERACTIVE_ARG;
            arg = TEST_ARG;

            switch (arg)
            {
                case TEST_ARG:
                    new TestGame(words).RunTest();
                    break;

                case ILLEGAL_CHECK_ARG:
                    new DictionaryCheck(words).IllegalWordCheck();
                    break;

                case LEGAL_CHECK_ARG:
                    new DictionaryCheck(words).PossibleAnswersCheck();
                    break;

                case INTERACTIVE_ARG:
                default:
                    new InteractiveGame(words).PlayGame();
                    break;
            }
        }
    }
}