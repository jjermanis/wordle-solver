using System;
using System.Collections.Generic;
using System.Linq;

namespace wordle_solver
{
    public class WordElement
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

        public static IList<WordElement> GetWordElementList(
            IEnumerable<string> words,
            LetterDistribution dist)
        {
            var result = new List<WordElement>();
            var rank = 1;
            foreach (var word in words)
            {
                var popularScore = Math.Max(5 - (rank / 1000), 1);
                result.Add(new WordElement(word, popularScore));
                rank++;
            }
            result = result.OrderByDescending(o => o.CalcScore(dist)).ToList();
            return result;
        }
    }
}