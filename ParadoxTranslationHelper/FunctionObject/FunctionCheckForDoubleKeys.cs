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

            List<string> doubleKeys = DoFunctionCheckForDoubleKeys( Utility.CreateTranslationFilesFromDirectory(PathGerman));

            return true;
        }

        private List<string>? DoFunctionCheckForDoubleKeys(List<TranslationFile> translationFiles) 
        {
            //TODO: 2025-02-23 - JHA - To implement
            //TranslationFileCreator
            //Iterate over every TranslationFile.lineObjects
            //If double key found put string with filename in top of list, then put double key with line number.

            if (translationFiles == null)
            {
                Console.WriteLine("Parameter <translationFiles> must not be null!");
                return null;
            }

            if (translationFiles.Count == 0)
            {
                Console.WriteLine("Parameter <translationFiles> contains no translation files!");
                return null;
            }

            List<List<string>> doubleKeys = new List<List<string>>();
            foreach( TranslationFile translationFile in translationFiles )
            {
                doubleKeys.Add(CheckForDoubleKeys(translationFile));
            }
            return null;
        }

        private List<string>? CheckForDoubleKeys(TranslationFile translationFile)
        {
            if (translationFile == null)
            {
                Console.WriteLine("Parameter <translationFile> must not be null!");
                return null;
            }

            List<string> doubleKeys = new List<string>();
            if (translationFile.Lines.Count == 0)
            {
                Console.WriteLine("Parameter <translationFile> must not be null!");
                return doubleKeys;
            }

            Dictionary<string,LineObject> keyLines = new Dictionary<string,LineObject>();

            foreach (var line in translationFile.Lines)
            {
                if( string.IsNullOrEmpty(line.Value.Key) )
                { 
                    continue; 
                }

                if (keyLines.ContainsKey(line.Value.Key))
                {
                    doubleKeys.Add(line.Value.Key + ";" + line.Value.LineNumber);
                    continue;
                }

                keyLines.Add(line.Value.Key,line.Value);
            }

            return doubleKeys;
        }
    }
}
