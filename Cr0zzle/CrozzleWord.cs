using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    class CrozzleWord
    {
        public string Word { get; private set; }
        public WordLocation Location { get; private set; }
        public int X { get { return Location.X; } }
        public int Y { get { return Location.Y; } }
        public int Length { get { return Word.Length; } }
        public int Score { get; private set; }

        public CrozzleWord(string word, int score)
        {
            Word = word;
            Score = score;
            Location = new WordLocation(-1, -1); ;
        }

        public CrozzleWord(string word, WordLocation location)
        {
            Word = word;
            Location = location;
        }

        public CrozzleWord(string word, int x, int y)
        {
            Word = word;
            Location = new WordLocation(x,y);
        }

        public void SetLocation(WordLocation location)
        {
            Location = location;
        }

        public void SetLocation(int x, int y)
        {
            Location = new WordLocation(x, y);
        }
    }
}
