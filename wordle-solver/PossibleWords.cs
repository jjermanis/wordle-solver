using System.Collections.Generic;
using System.Linq;

namespace wordle_solver
{
    internal class PossibleWords
    {
        private List<string> _options;
        private LetterDistribution _dist;

        public PossibleWords(IEnumerable<string> words)
        {
            _dist = new LetterDistribution(words);
            _options = words.OrderByDescending(w => Score(w)).ToList();
        }

        public long Score(string word)
        {
            var alreadySeen = new HashSet<char>();
            var result = 1L;

            foreach (var c in word)
            {
                if (alreadySeen.Contains(c))
                    continue;
                alreadySeen.Add(c);
                result *= _dist.Frequency[c];
            }
            return result;
        }

        public string BestGuess()
            => _options.FirstOrDefault();

        public void AddClue(string guess, string result)
        {
            var newList = new List<string>();
            foreach (var word in _options)
            {
                var isMatch = true;
                for (var i = 0; i < 5; i++)
                {
                    switch (result[i])
                    {
                        case 'G':
                            if (word[i] != guess[i])
                                isMatch = false;
                            break;

                        case 'Y':
                            if (word[i] == guess[i])
                                isMatch = false;
                            if (!word.Contains(guess[i]))
                                isMatch = false;
                            break;

                        case 'X':
                            if (word.Contains(guess[i]))
                                isMatch = false;
                            break;
                    }
                }
                if (isMatch)
                    newList.Add(word);
            }
            _options = newList;
        }
    }
}