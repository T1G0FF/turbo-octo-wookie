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
    }
}
