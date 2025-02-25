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
        private string _resultFileNameAppendix;
        private string _pathAnalyze;
        public FunctionCheckForDoubleKeys(string name) : base(name)
        {
        }

        public string PathGerman { get => _pathGerman; set => _pathGerman = value; }
        public string ResultFileNameAppendix { get => _resultFileNameAppendix; set => _resultFileNameAppendix = value; }
        public string PathAnalyze { get => _pathAnalyze; set => _pathAnalyze = value; }

        public override bool DoWork()
        {
            if( true == string.IsNullOrEmpty(_pathGerman ) )
            {
                Console.WriteLine("Member <PathGerman> must not be null or empty!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_resultFileNameAppendix))
            {
                Console.WriteLine("Member <ResultFileNameAppendix> must not be null or empty!");
                return false;
            }

            if(true == string.IsNullOrEmpty(_pathAnalyze) ) 
            {
                Console.WriteLine("Member <PathAnalyze> must not be null or empty!");
                return false;
            }

            WriteDoubleKeyFiles( DoFunctionCheckForDoubleKeys( Utility.CreateTranslationFilesFromDirectory(PathGerman)) );

            return true;
        }

        private Dictionary<string, List<string>>? DoFunctionCheckForDoubleKeys(List<TranslationFile> translationFiles) 
        {
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

            Dictionary<string,List<string>> doubleKeyFiles = new Dictionary<string, List<string>>();
            foreach( TranslationFile translationFile in translationFiles )
            {
                List<string> doubleKeys = CheckForDoubleKeys(translationFile);
                if( doubleKeys == null )
                {
                    continue;
                }

                if(doubleKeys.Count == 0 ) 
                {
                    continue;
                }

                doubleKeyFiles.Add(CreateFileNameDoubleKey(translationFile), doubleKeys );
            }

            return doubleKeyFiles;
        }

        private string CreateFileNameDoubleKey(TranslationFile translationFile)
        {
            return Path.Combine(PathAnalyze, Path.GetFileName(translationFile.FileName) + ResultFileNameAppendix);
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

        private void WriteDoubleKeyFiles(Dictionary<string, List<string>> doubleKeyFiles )
        {
            if ( doubleKeyFiles == null ) 
            {
                Console.WriteLine("Parameter <doubleKeyFiles> must not be null!");
            }

            DirectoryInfo directoryInfo = Directory.CreateDirectory(PathAnalyze);


            Utility.Write(doubleKeyFiles);
        }
    }
}
