using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace wordle_solver
{
    internal class DictionaryCheck
    {
        private const string LEGAL_WORDS_PATH = @"..\..\..\legalEntries.txt";
        private const string POSSIBLE_ANSWERS_PATH = @"..\..\..\possibleAnswers.txt";

        private readonly IList<string> _words;
        private readonly HashSet<string> _legalEntries;
        private readonly IList<string> _possibleAnswers;

        public DictionaryCheck(IEnumerable<string> words)
        {
            _words = words.ToList();

            var rawLegal = File.ReadLines(LEGAL_WORDS_PATH).First().Replace("\"", "");
            _legalEntries = new HashSet<string>(rawLegal.Split(','));
            var rawPossible = File.ReadLines(POSSIBLE_ANSWERS_PATH).First().Replace("\"", "");
            _possibleAnswers = new List<string>(rawPossible.Split(','));
        }

        public void IllegalWordCheck()
        {
            var results = new List<string>();

            foreach (var word in _words)
            {
                if (!_legalEntries.Contains(word))
                    results.Add(word);
            }

            if (results.Count == 0)
                Console.WriteLine("All entries in dictionary are legal!");
            else
            {
                Console.WriteLine("The following dictionary entries are invalid:");
                foreach (var word in results)
                    Console.WriteLine(word);
            }
        }

        public void PossibleAnswersCheck()
        {
            var results = new List<string>();
            var dictionarySet = new HashSet<string>(_words);

            foreach (var answer in _possibleAnswers)
            {
                if (!dictionarySet.Contains(answer))
                    results.Add(answer);
            }

            if (results.Count == 0)
                Console.WriteLine("All possible answers are in dictionary!");
            else
            {
                Console.WriteLine("The following possible answers are not in the dictionary:");
                foreach (var word in results)
                    Console.WriteLine(word);
            }
        }
    }
}