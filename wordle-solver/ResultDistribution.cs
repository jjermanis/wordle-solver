namespace wordle_solver
{
    internal class ResultDistribution
    {
        private readonly int _guessCount;

        public ResultDistribution(int guessCount)
        {
            _guessCount = guessCount;
            ScoreCount = new int[_guessCount + 1];
        }

        public int[] ScoreCount { get; set; }
        public int Misses { get; set; }
        public int Duration { get; set; }

        public decimal Average()
        {
            decimal total = 0;
            decimal cases = 0;
            for (int i = 1; i <= _guessCount; i++)
            {
                total += i * ScoreCount[i];
                cases += ScoreCount[i];
            }
            return total / cases;
        }

        public decimal Score()
        {
            decimal total = 0;
            decimal cases = 0;
            for (int i = 1; i <= _guessCount; i++)
            {
                total += i * ScoreCount[i];
                cases += ScoreCount[i];
            }
            total += Misses * 10;

            return total / cases;
        }

        public decimal WinRate()
        {
            decimal cases = 0;
            for (int i = 1; i <= _guessCount; i++)
            {
                cases += ScoreCount[i];
            }
            return cases / (cases + Misses);
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 1; i <= _guessCount; i++)
                result += $"{i}: {ScoreCount[i]}\r\n";
            result += $"Misses: {Misses}\r\n";
            result += $"Win Rate: {WinRate():P3}\r\n";
            result += $"Average: {Average():F3}\r\n";
            result += $"Score: {Score():F3}\r\n";
            result += $"Duration: {Duration} ms\r\n";
            return result;
        }
    }
}