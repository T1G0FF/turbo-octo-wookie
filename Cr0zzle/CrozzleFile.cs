using System;
using System.IO;

namespace Assignment1
{
    class CrozzleFile
    {
        public string FilePath { get; protected set; }
        public string FileName { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public CrozzleFile(string filePath)
        {
            FilePath = filePath;
            FileName = FilePath.Substring(FilePath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
        }
    }
}
