using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    internal class FileReaderSubstitutionItem
    {
        private string _fileName;

        public string FileName { get => _fileName; set => _fileName = value; }

        /*
         * Dictionary<string, string> --> Dictionary<substitute, original>
         */
        public Dictionary<string, string> Read()
        {
            if (_fileName == null)
            {
                return new Dictionary<string, string>();
            }

            List<string> allLines = new List<string>();

            using (TextReader reader = File.OpenText(_fileName))
            {
                allLines = ReadAllLines(reader);
            }

            return Utility.ConvertToDictionary(allLines);
        }

        private List<string> ReadAllLines(TextReader reader)
        {
            string line;

            List<string> lines = new List<string>();

            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }

            return lines;
        }

    }
}
