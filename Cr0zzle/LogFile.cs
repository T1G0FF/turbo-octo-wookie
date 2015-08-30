using System;
using System.IO;
using System.Text;

namespace Assignment1
{
    public static class LogFile
    {
        const string filePath = @".\LogFile.txt";

        public static void WriteLine(string text, params object[] args)
        {
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                sw.WriteLine(text, args);
            }
        }

        public static void Write(string text, params object[] args)
        {
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                sw.Write(text, args);
            }
        }  
    }
}
