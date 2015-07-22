using System;
using System.Collections;
using System.IO;
using CrozzleExceptions;

namespace Assignment1
{
    class Wordlist : CrozzleFile, IEnumerable
    {
        #region Properties
        public string Difficulty { get; private set; }
        
        private string[] _wordlist { get; set; }
        public string this[int index]
        {   get
            {   return _wordlist[index];
            }
        }
        
        public int WordCount
        {   get
            {   return _wordlist.Length;
            }
        }
        #endregion

        #region Constructor
        public Wordlist(string filePath) : base(filePath)
        {
            if ( File.Exists(FilePath) == false)    // Check file exists
            {
                throw new FileNotFoundException(String.Format("Wordlist file not found - {0}", FilePath));
            }
            else
            {
                if (LoadData() == false)            // Check file format, load if valid
                {
                    throw new WordlistFileFormatException(FilePath);
                }
            }
        }
        #endregion

        #region Public Methods
        public bool Find(string query)
        {
            foreach (string word in _wordlist)
			{	if( query.Equals(word) )
				{	return true;
				}
			}
			return false;
		}
        #endregion

        #region Private Methods
        private bool LoadData()
        {
            bool IsValid = false;	// Default to Invalid
            int RowCount = 0;

            try
            {
                RowCount = File.ReadAllLines(FilePath).Length;

                if (RowCount == 1)
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
                                throw new InvalidDataException(String.Format("{0} is not a valid Integer [Word count]", row[0]));

                            int tempCrozzleWidth = 0;
                            if (int.TryParse(row[1], out tempCrozzleWidth) == false)
                                throw new InvalidDataException(String.Format("{0} is not a valid Integer [Width]", row[1]));
                            Width = tempCrozzleWidth;

                            int tempCrozzleHeight = 0;
                            if (int.TryParse(row[2], out tempCrozzleHeight) == false)
                                throw new InvalidDataException(String.Format("{0} is not a valid Integer [Height]", row[2]));
                            Height = tempCrozzleHeight;

                            string tempDifficulty = row[3];
                            switch (tempDifficulty.ToUpper())
                            {
                                case "EASY":
                                case "MEDIUM":
                                case "HARD":
                                case "EXTREME":
                                    Difficulty = tempDifficulty.ToUpper();
                                    break;
                                default:
                                    throw new InvalidDataException(String.Format("{0} is not a valid Difficulty", row[3]));
                            }

                            int tempLength = row.Length - 4;
                            _wordlist = new string[tempLength];
                            Array.ConstrainedCopy(row, 4, _wordlist, 0, tempLength);

                            IsValid = true;
                        }
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

        public IEnumerator GetEnumerator()
        {
            return _wordlist.GetEnumerator();
        }
    }
}
