using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    class CrozzleArray
    {
        public char[][] _crozzleGrid { get; set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        public char this[int x, int y]
        {
            get
            {
                return _crozzleGrid[y][x];
            }
            set
            {
                _crozzleGrid[y][x] = value;
            }
        }

        public CrozzleArray(int height, int width)
        {
            Height = height;
            Width = width;
            _crozzleGrid = new char[height][];
            for (int i = 0; i < _crozzleGrid.Length; i++)
            {
                _crozzleGrid[i] = new char[width];
            }

            FillWithBlanks();
        }

        public void FillWithBlanks()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    this[x,y] = ' ';
                }
            }
        }

        public void DeepCopy(CrozzleArray ca)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    this[x, y] = ca[x,y];
                }
            }
        }

        public void DeepCopy(char[][] ca)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    this[x, y] = ca[y][x];
                }
            }
        }
    }
}
