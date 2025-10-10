using ParadoxTranslationHelper.Utilities;
using Serilog;
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
                Log.Verbose("Member <PathGerman> must not be null or empty!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_resultFileNameAppendix))
            {
                Log.Verbose("Member <ResultFileNameAppendix> must not be null or empty!");
                return false;
            }

            if(true == string.IsNullOrEmpty(_pathAnalyze) ) 
            {
                Log.Verbose("Member <PathAnalyze> must not be null or empty!");
                return false;
            }

            WriteDoubleKeyFiles( DoFunctionCheckForDoubleKeys(FileUtility.CreateTranslationFilesFromDirectory(PathGerman)) );

            return true;
        }

        private Dictionary<string, List<string>>? DoFunctionCheckForDoubleKeys(List<TranslationFile> translationFiles) 
        {
            if (translationFiles == null)
            {
                Log.Verbose("Parameter <translationFiles> must not be null!", translationFiles);
                return null;
            }

            if (translationFiles.Count == 0)
            {
                Log.Verbose("Parameter <translationFiles> contains no translation files!", translationFiles);
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

                DirectoryInfo directoryInfo = Directory.CreateDirectory(PathAnalyze);

                FileUtility.Write(translationFile, Path.Combine(PathAnalyze, Utility.CreateFileNameWithoutLocalisationGerman(translationFile)));
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
                Log.Verbose("Parameter <translationFile> must not be null!");
                return null;
            }

            List<string> doubleKeys = new List<string>();
            if (translationFile.Lines.Count == 0)
            {
                Log.Verbose("Parameter <translationFile> must not be null!");
                return doubleKeys;
            }

            Dictionary<string,LineObject> keyLines = new Dictionary<string,LineObject>();
            Dictionary<int, LineObject> uniqueLines = new Dictionary<int, LineObject>();

            foreach (var line in translationFile.Lines)
            {
                if( string.IsNullOrEmpty(line.Value.Key) )
                {
                    uniqueLines.Add(line.Key, line.Value);
                    continue; 
                }

                if (keyLines.ContainsKey(line.Value.Key))
                {
                    doubleKeys.Add(line.Value.Key + ";" + line.Value.LineNumber + ";" +line.Value.OriginalLine);
                    continue;
                }

                keyLines.Add(line.Value.Key,line.Value);
                uniqueLines.Add(line.Key,line.Value);
            }

            translationFile.Lines = uniqueLines;

            return doubleKeys;
        }

        private void WriteDoubleKeyFiles(Dictionary<string, List<string>> doubleKeyFiles )
        {
            if ( doubleKeyFiles == null ) 
            {
                Log.Verbose("Parameter <doubleKeyFiles> must not be null!");
            }

            DirectoryInfo directoryInfo = Directory.CreateDirectory(PathAnalyze);


            FileUtility.Write(doubleKeyFiles);
        }
    }
}
