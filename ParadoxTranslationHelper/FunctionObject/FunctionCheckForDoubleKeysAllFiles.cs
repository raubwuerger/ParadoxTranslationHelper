using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionCheckForDoubleKeysAllFiles : FunctionObjectBase
    {
        private string _pathGerman;
        private string _pathAnalyze;
        private bool _deleteDoubleKeys = false;
        public FunctionCheckForDoubleKeysAllFiles(string name) : base(name)
        {
        }

        public string PathGerman { get => _pathGerman; set => _pathGerman = value; }
        public string PathAnalyze { get => _pathAnalyze; set => _pathAnalyze = value; }
        public bool DeleteDoubleKeys { get => _deleteDoubleKeys; set => _deleteDoubleKeys = value; }

        public override bool DoWork()
        {
            if( true == string.IsNullOrEmpty(_pathGerman ) )
            {
                Console.WriteLine("Member <PathGerman> must not be null or empty!");
                return false;
            }

            if(true == string.IsNullOrEmpty(_pathAnalyze) ) 
            {
                Console.WriteLine("Member <PathAnalyze> must not be null or empty!");
                return false;
            }

            Dictionary<string, List<LineObject>> doubleKeyFiles = WriteDoubleKeyFiles(DoFunctionCheckForDoubleKeys(Utility.CreateTranslationFilesFromDirectory(PathGerman)));

            if (true == _deleteDoubleKeys)
            {
                DoDeleteDoubleKeys(doubleKeyFiles);
            }

            return true;
        }

        private Dictionary<string, List<LineObject>>? DoFunctionCheckForDoubleKeys(List<TranslationFile> translationFiles) 
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

            Dictionary<string,List<LineObject>> doubleKeyFiles = new Dictionary<string, List<LineObject>>();
            Dictionary<string,LineObject> keysFilesAll = new Dictionary<string, LineObject>();
            foreach( TranslationFile translationFile in translationFiles )
            {
                foreach( KeyValuePair<int, LineObject> line in translationFile.Lines )
                {
                    if( true == string.IsNullOrEmpty(line.Value.Key) )
                    {
                        continue;
                    }

                    if ( false == keysFilesAll.ContainsKey(line.Value.Key) )
                    {
                        keysFilesAll.Add(line.Value.Key, line.Value);
                        continue;
                    }

                    if( false == doubleKeyFiles.ContainsKey(line.Value.Key))
                    {
                        doubleKeyFiles.Add(line.Value.Key, new List<LineObject> { keysFilesAll[line.Value.Key], line.Value });
                        continue;
                    }

                    doubleKeyFiles[line.Value.Key].Add(line.Value);
                }
            }

            return doubleKeyFiles;
        }
        private Dictionary<string, List<LineObject>> WriteDoubleKeyFiles( Dictionary<string, List<LineObject>> doubleKeyFiles )
        {
            if ( doubleKeyFiles == null ) 
            {
                Console.WriteLine("Parameter <doubleKeyFiles> must not be null!");
            }

            DirectoryInfo directoryInfo = Directory.CreateDirectory(PathAnalyze);


            Utility.Write(doubleKeyFiles);
            return doubleKeyFiles;
        }

        private void DoDeleteDoubleKeys(Dictionary<string, List<LineObject>> doubleKeyFiles)
        {
            //TODO: 2025-02-25 - JHA - To Implement
        }
    }
}
