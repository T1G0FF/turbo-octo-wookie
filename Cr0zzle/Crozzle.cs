using System;
using System.IO;
using CrozzleExceptions;

namespace Assignment1
{
    class Crozzle : CrozzleFile
    {
        #region Properties
        private char[][] _crozzle { get; set; }
        public char this[int x, int y]
        {   get
            {   return _crozzle[y][x];
            }
        }
        #endregion

        #region Constructor
        public Crozzle(string filePath) : base(filePath)
        {
            if (File.Exists(FilePath) == false)
            {   throw new FileNotFoundException(String.Format("Crozzle file not found - {0}", FilePath));
            }
            else
            {
                if (LoadData() == false)            // Check file format, load if valid
                {
                    throw new CrozzleFileFormatException(FilePath);
                }
            }
        }
        #endregion

        #region Private Methods
        private bool LoadData()
        {
            bool IsValid = false;	// Default to Invalid
            
            try		// If loading file fails
            {
                Height = File.ReadAllLines(FilePath).Length;

                _crozzle = new char[Height][];

                using (FileStream fs = File.Open(FilePath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        int prevWidth = -1;
                        for (int y = 0; y < Height; y++)
                        {
                            _crozzle[y] = sr.ReadLine().ToCharArray();
                            
                            if(prevWidth == -1) { Width = _crozzle[y].Length; }
                            prevWidth = _crozzle[y].Length;
                            
                            if( prevWidth != Width) { break; }
                        }
                        IsValid = true;
                    }
                }
                return IsValid;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
