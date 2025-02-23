using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionCheckForDoubleKeys : FunctionObjectBase
    {
        private string _pathGerman;
        private string _resultFileName;
        public FunctionCheckForDoubleKeys(string name) : base(name)
        {
        }

        public string PathGerman { get => _pathGerman; set => _pathGerman = value; }
        public string ResultFileName { get => _resultFileName; set => _resultFileName = value; }

        public override bool DoWork()
        {
            if( true == string.IsNullOrEmpty(_pathGerman ) )
            {
                Console.WriteLine("Member <PathGerman> must not be null or empty!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_resultFileName))
            {
                Console.WriteLine("Member <ResultFileName> must not be null or empty!");
                return false;
            }

            List<string> doubleKeys = DoFunctionCheckForDoubleKeys();

            return true;
        }

        private List<string>? DoFunctionCheckForDoubleKeys() 
        {
            //TODO: 2025-02-23 - JHA - To implement
            //TranslationFileCreator
            //Iterate over every TranslationFile.lineObjects
            //If double key found put string with filename in top of list, then put double key with line number.
            return null;
        }
    }
}
