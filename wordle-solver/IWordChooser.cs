namespace wordle_solver
{
    internal interface IWordChooser
    {
        string BestGuess();

        void UpdateAfterGuess(string guess, string result);
    }
}