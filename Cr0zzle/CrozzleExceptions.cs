using System;

namespace CrozzleExceptions
{
    public class CrozzleFileFormatException : System.IO.IOException
    {
        const string formatMessage = "!!TODO!!";

        public CrozzleFileFormatException(string path)
            : base(String.Format("{0} {1}", path, formatMessage))
        {
        }
    }

    public class WordlistFileFormatException : System.IO.IOException
    {
        const string formatMessage = "does not conform to the correct wordlist format:\n" +
                                        "[No. of Words] [Width] [Height] [Difficulty] [Word list]";

        public WordlistFileFormatException(string path)
            : base(String.Format("{0} {1}", path, formatMessage))
        {
        }
    }
}