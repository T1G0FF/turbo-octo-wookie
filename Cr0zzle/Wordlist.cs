using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Assignment1
{
    class Wordlist : CrozzleFile, IEnumerable
    {
        #region Properties
        public string Difficulty { get; private set; }

        private string[] _wordlist { get; set; }
        public string this[int index]
        {
            get
            {
                return _wordlist[index];
            }
        }

        public int WordCount
        {
            get
            {
                return _wordlist.Length;
            }
        }
        #endregion

        #region Constructor
        public Wordlist(string filePath)
            : base(filePath)
        {
            LogFile.WriteLine("[Wordlist] - '{0}'", FilePath);

            if (File.Exists(FilePath) == false)    // Check file exists
            {
                string error = String.Format("\t[!ERROR!] Word list file not found!");
                LogFile.WriteLine(error);
                //throw new FileNotFoundException(error);
            }
            else
            {
                ValidFile = LoadData();
                if (ValidFile == false)            // Check file format, load if valid
                {
                    string error = String.Format("\t[WARN] The correct word list format is: [No. of Words] [Height] [Width] [Difficulty] [Word list]");
                    LogFile.WriteLine(error);
                    //throw new InvalidDataException(error);
                }
            }
        }
        #endregion

        #region Public Methods
        public bool Contains(string query)
        {
            return _wordlist.Contains(query);
        }

        public bool StartsWith(string query)
        {
            foreach (string word in _wordlist)
            {
                if (word.StartsWith(query))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Private Methods
        private bool LoadData()
        {
            bool IsValid = true;	// Default to Valid
            int RowCount = 0;

            RowCount = File.ReadAllLines(FilePath).Length;

            if (RowCount == 1)
            {
                try
                {
                    using (FileStream fs = File.Open(FilePath, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            char[] _separator = new char[] { ',' };

                            string[] row = sr.ReadLine().Split(_separator);

                            sr.Close();

                            int tempWordCount = 0;
                            if (int.TryParse(row[0], out tempWordCount) == false)
                            {
                                LogFile.WriteLine("\t[!ERROR!] '{0}' is not a valid Integer [Word count]", row[0]);
                                IsValid = IsValid & false;
                            }
                            else if (tempWordCount < 10)
                            {
                                LogFile.WriteLine("\t[!ERROR!] Word list must contain more than 10 words ({0} < 10)", tempWordCount);
                                IsValid = IsValid & false;
                            }
                            else if (tempWordCount > 1000)
                            {
                                LogFile.WriteLine("\t[!ERROR!] Word list must contain no more than 1000 words ({0} > 1000)", tempWordCount);
                                IsValid = IsValid & false;
                            }
                            else
                            {
                                IsValid = IsValid & true;
                            }

                            int tempCrozzleHeight = 0;
                            if (int.TryParse(row[1], out tempCrozzleHeight) == false)
                            {
                                LogFile.WriteLine("\t[!ERROR!] '{0}' is not a valid Integer [Height]", row[1]);
                                IsValid = IsValid & false;
                            }
                            else if (tempCrozzleHeight < 4)
                            {
                                LogFile.WriteLine("\t[!ERROR!] Crozzle Height must be greater than 4 ({0} < 4)", tempCrozzleHeight);
                                IsValid = IsValid & false;
                            }
                            else if (tempCrozzleHeight > 400)
                            {
                                LogFile.WriteLine("\t[!ERROR!] Crozzle Height must be less than 400 ({0} > 400)", tempCrozzleHeight);
                                IsValid = IsValid & false;
                            }
                            else
                            {
                                Height = tempCrozzleHeight;
                                IsValid = IsValid & true;
                            }

                            int tempCrozzleWidth = 0;
                            if (int.TryParse(row[2], out tempCrozzleWidth) == false)
                            {
                                LogFile.WriteLine("\t[!ERROR!] '{0}' is not a valid Integer [Width]", row[2]);
                                IsValid = IsValid & false;
                            }
                            else if (tempCrozzleWidth < 4)
                            {
                                LogFile.WriteLine("\t[!ERROR!] Crozzle Width must be greater than 4 ({0} < 4)", tempCrozzleWidth);
                                IsValid = IsValid & false;
                            }
                            else if (tempCrozzleWidth > 400)
                            {
                                LogFile.WriteLine("\t[!ERROR!] Crozzle Width must be less than 400 ({0} > 400)", tempCrozzleWidth);
                                IsValid = IsValid & false;
                            }
                            else
                            {
                                Width = tempCrozzleWidth;
                                IsValid = IsValid & true;
                            }

                            string tempDifficulty = row[3];
                            switch (tempDifficulty.ToUpper())
                            {
                                case "EASY":
                                case "MEDIUM":
                                case "HARD":
                                case "EXTREME":
                                    Difficulty = tempDifficulty.ToUpper();
                                    IsValid = IsValid & true;
                                    break;
                                default:
                                    LogFile.WriteLine("\t[!ERROR!] '{0}' is not a valid Difficulty", row[3]);
                                    IsValid = IsValid & false;
                                    break;
                            }

                            int tempLength = row.Length - 4;
                            string[] tempArray = new string[tempLength];
                            Array.ConstrainedCopy(row, 4, tempArray, 0, tempLength);

                            _wordlist = tempArray.OrderBy(s => s.Length).ToArray();
                            int duplicateCheck = tempArray.Distinct().ToArray().Length;

                            if (_wordlist.Length != tempWordCount)
                            {
                                LogFile.WriteLine("\t[!ERROR!] Word count does not equal the number of given words ({0} != {1})", tempWordCount, _wordlist.Length);
                                IsValid = IsValid & false;
                            }
                            else if (_wordlist.Length != duplicateCheck)
                            {
                                LogFile.WriteLine("\t[!ERROR!] {0} duplicate words found!", _wordlist.Length - duplicateCheck);
                                IsValid = IsValid & false;
                            }
                            else
                            {
                                IsValid = IsValid & true;
                            }

                            foreach (string s in _wordlist)
                            {
                                if (ContainsInvalidChars(s))
                                {
                                    LogFile.WriteLine("\t[!ERROR!] '{0}' is not a valid word!", s);
                                    IsValid = IsValid & false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFile.WriteLine(ex.Message);
                    throw;
                }
            }

            return IsValid;
        }
        #endregion

        private bool ContainsInvalidChars(string s)
        {
            Regex rgx = new Regex(@"^[a-zA-Z ]+$");
            return !rgx.IsMatch(s);
        }

        public IEnumerator GetEnumerator()
        {
            return _wordlist.GetEnumerator();
        }
    }
}
