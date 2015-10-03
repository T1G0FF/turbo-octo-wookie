using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    class WordLocation
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public WordLocation(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static WordLocation Invalid { get { return new WordLocation(-1, -1); } }
        public static WordLocation Zero { get { return new WordLocation(0, 0); } }

        public static bool AreEqual(WordLocation value, WordLocation compare)
        {
            if (value.X != compare.X) return false;
            if (value.Y != compare.Y) return false;
            return true;
        }

        public static bool AreInequal(WordLocation value, WordLocation compare)
        {
            return !AreEqual(value, compare);
        }
    }
}
