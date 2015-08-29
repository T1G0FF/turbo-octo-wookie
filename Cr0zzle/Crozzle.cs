using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Assignment1
{
    class Crozzle : CrozzleFile
    {
        #region Properties
        private char[][] _crozzle { get; set; }
        public Dictionary<string, WordLocation> Words;
        public List<string> HorizontalWords;
        public List<string> VerticalWords;
        public Dictionary<string, List<CrozzleWord>> IntersectedWords;

        public char this[int x, int y]
        {
            get
            {
                return _crozzle[y][x];
            }
        }
        #endregion

        #region Constructor
        public Crozzle(string filePath)
            : base(filePath)
        {
            LogFile.WriteLine("[Crozzle]  - '{0}'", FilePath);

            if (File.Exists(FilePath) == false)
            {
                string error = String.Format("\t[!ERROR!] Crozzle file not found!");
                LogFile.WriteLine(error);
                //throw new FileNotFoundException(error);
            }
            else
            {
                ValidFile = LoadData();
                if (ValidFile == false)            // Check file format, load if valid
                {
                    string error = String.Format("\t[WARN] A crozzle must be placed within the first several lines and first several columns of a file, with spaces for empty squares");
                    LogFile.WriteLine(error);
                    //throw new InvalidDataException(error);
                }
            }
        }
        #endregion

        #region Private Methods
        private bool LoadData()
        {
            bool IsValid = true;	// Default to Valid

            try
            {
                Height = File.ReadAllLines(FilePath).Length;

                if (Height < 4)
                {
                    LogFile.WriteLine("\t[!ERROR!] Crozzle Height must be greater than 4 ({0} < 4)", Height);
                    IsValid = IsValid & false;
                }
                else if (Height > 400)
                {
                    LogFile.WriteLine("\t[!ERROR!] Crozzle Height must be less than 400 ({0} > 400)", Height);
                    IsValid = IsValid & false;
                }
                else
                {
                    _crozzle = new char[Height][];

                    using (FileStream fs = File.Open(FilePath, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            int prevWidth = -1;
                            for (int y = 0; y < Height; y++)
                            {
                                string currentLine = sr.ReadLine();
                                if (ContainsInvalidChars(currentLine))
                                {
                                    string errorLine = currentLine;
#if DEBUG
                                    errorLine = errorLine.Replace("	", "→").Replace(" ", "·");
#endif
                                    LogFile.WriteLine("\t[!ERROR!] Crozzle contains invalid characters ({0})", errorLine);
                                    IsValid = IsValid & false;
                                }
                                
                                _crozzle[y] = currentLine.ToCharArray();

                                if (prevWidth == -1)
                                {
                                    Width = _crozzle[y].Length;
                                    if (Width < 4)
                                    {
                                        LogFile.WriteLine("\t[!ERROR!] Crozzle Width must be greater than 4 ({0} < 4)", Width);
                                        IsValid = IsValid & false;
                                    }
                                    else if (Width > 400)
                                    {
                                        LogFile.WriteLine("\t[!ERROR!] Crozzle Width must be less than 400 ({0} > 400)", Width);
                                        IsValid = IsValid & false;
                                    }
                                }
                                prevWidth = _crozzle[y].Length;

                                if (prevWidth != Width)
                                {
                                    LogFile.WriteLine("\t[!ERROR!] Crozzle does not contain consistent row lengths ({0} != {1})", prevWidth, Width);
                                    IsValid = IsValid & false;
                                }
                                else
                                {
                                    IsValid = IsValid & true;
                                }
                            }
                        }
                    }
                }
                return IsValid;
            }
            catch (Exception ex)
            {
                LogFile.WriteLine(ex.Message);
                throw;
            }
        }

        private bool ContainsInvalidChars(string s)
        {
            Regex rgx = new Regex(@"^[a-zA-Z ]+$");
            return !rgx.IsMatch(s);
        }
        #endregion
    }
}
