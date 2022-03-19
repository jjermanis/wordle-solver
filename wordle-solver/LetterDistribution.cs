using System.Collections.Generic;

namespace wordle_solver
{
    public class LetterDistribution
    {
        public IDictionary<char, int> Frequency { get; set; }

        public LetterDistribution(IEnumerable<string> words)
        {
            InitMap(words);
        }

        private void InitMap(IEnumerable<string> words)
        {
            Frequency = new Dictionary<char, int>();
            for (var c = 'a'; c <= 'z'; c++)
                Frequency[c] = 0;

            foreach (var word in words)
                foreach (var c in word)
                    Frequency[c]++;
        }
    }
}