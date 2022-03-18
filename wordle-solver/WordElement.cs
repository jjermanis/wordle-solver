using System.Collections.Generic;

namespace wordle_solver
{
    internal class WordElement
    {
        private readonly List<char> _uniqueLetters;
        private readonly int _commonWordScore;

        public string Word { get; set; }

        public IEnumerable<char> UniqueLetters { get => _uniqueLetters; }
        public int CommonWordScore { get => _commonWordScore; }

        public WordElement(string word, int commonWordScore)
        {
            Word = word;
            _commonWordScore = commonWordScore;

            _uniqueLetters = new List<char>();
            foreach (var c in word)
                if (!_uniqueLetters.Contains(c))
                    _uniqueLetters.Add(c);
        }

        public long CalcScore(LetterDistribution dist)
        {
            var result = 1L;

            foreach (var c in UniqueLetters)
                result *= dist.Frequency[c];

            return result * _commonWordScore;
        }

        public override string ToString()
            => Word;
    }
}