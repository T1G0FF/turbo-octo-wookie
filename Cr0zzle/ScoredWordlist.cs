using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assignment1
{
    class ScoredWordlist : IEnumerable
    {
        private List<CrozzleWord> sortedByScore;
        public int Count { get { return sortedByScore.Count; } }

        public CrozzleWord this[int index]
        {
            get
            {
                return sortedByScore[index];
            }
        }

        public ScoredWordlist(Wordlist wl)
        {
            sortedByScore = sortScoreList(wl);
        }

        public ScoredWordlist(ScoredWordlist swl)
        {
            sortedByScore = new List<CrozzleWord>(swl.Count);
            foreach (CrozzleWord cw in swl)
            {
                sortedByScore.Add(cw);
            }
        }

        private List<CrozzleWord> sortScoreList(Wordlist currentWordlist)
        {
            string Difficulty = currentWordlist.Difficulty;
            if (Difficulty != "EXTREME") Difficulty = "HARD";
            List<CrozzleWord> wordScores = new List<CrozzleWord>(currentWordlist.WordCount);
            foreach (string word in currentWordlist)
            {
                wordScores.Add(new CrozzleWord(word, CrozzleValidation.GetWordScore(Difficulty, word)));
            }

            List<CrozzleWord> result = wordScores.OrderByDescending(s => s.Score).ThenBy(s => s.Score).ToList();

            if (result.Count > 1)
            {
                CrozzleWord ts = result[0];
                result[0] = result[1];
                result[1] = ts;
            }

            return result;
        }

        public IEnumerator GetEnumerator()
        {
            return sortedByScore.GetEnumerator();
        }

        public bool Remove(CrozzleWord value)
        {
            return sortedByScore.Remove(value);
        }

        public int IndexOf(CrozzleWord value)
        {
            return sortedByScore.IndexOf(value);
        }
    }
}
