using System;
using System.IO;
using System.Text;

namespace Assignment1
{
    public static class LogFile
    {
        public static string folderPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + Path.DirectorySeparatorChar + @"Log Files" + Path.DirectorySeparatorChar;
        public static string filePath = folderPath + @"Log.txt";

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
