using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    internal class FileWriterSubstitutionItem
    {
        private string _fileName;
        private string _fileSuffix;

        public string FileName { get => _fileName; set => _fileName = value; }
        public string FileSuffix { get => _fileSuffix; set => _fileSuffix = value; }

        /*
         * Dictionary<string, string> --> Dictionary<substitute, original>
         */
        public void Write( Dictionary<string, string> substitutionItem )
        {
            if( _fileName == null )
            {
                return;
            }

            if (_fileSuffix == null)
            {
                return;
            }

            using (StreamWriter outputFile = new StreamWriter(_fileName + _fileSuffix))
            {
                foreach (KeyValuePair<string, string> entry in substitutionItem)
                {
                    outputFile.WriteLine( entry.Key +";" +entry.Value );
                }
            }
        }
    }
}
