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

            List<TranslationFile> translationFiles = Utility.CreateTranslationFilesFromDirectory(PathGerman);
            if( null == translationFiles )
            {
                Console.WriteLine("No translation files found at path! " +PathGerman);
                return false;
            }

            Dictionary<string, List<LineObject>> doubleKeyFiles = WriteDoubleKeyFiles(DoFunctionCheckForDoubleKeys(translationFiles));

            if (null == doubleKeyFiles)
            {
                return true;
            }

            if (doubleKeyFiles.Count == 0 )
            { 
                return true; 
            }

            if (true == _deleteDoubleKeys)
            {
                List<TranslationFile> translationFilesCorrected = DoDeleteDoubleKeys(doubleKeyFiles, translationFiles);
                if (null == translationFilesCorrected)
                {
                    return true;
                }
                
                foreach (TranslationFile translationFile in translationFilesCorrected )
                {
                    Utility.WriteTranslationFile(translationFile,Path.Combine(_pathAnalyze, Utility.CreateFullNameGerman(translationFile)) );
                }
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


            Utility.Write(doubleKeyFiles,Path.Combine(_pathAnalyze,"KeysDouble.txt"));
            return doubleKeyFiles;
        }

        private List<TranslationFile>? DoDeleteDoubleKeys(Dictionary<string, List<LineObject>> doubleKeyFiles, List<TranslationFile> originalFiles)
        {
            if (doubleKeyFiles == null)
            {
                Console.WriteLine("Parameter <doubleKeyFiles> must not be null!");
                return null;
            }

            if (originalFiles == null)
            {
                Console.WriteLine("Parameter <originalFiles> must not be null!");
                return null;
            }

            List<TranslationFile> translationFilesCorrected = new List<TranslationFile>();
            foreach (KeyValuePair<string, List<LineObject>> doubleKey in doubleKeyFiles)
            {
                foreach( LineObject lineObject in doubleKey.Value )
                {
                    TranslationFile translationFile = originalFiles.Find(x => x.FileName.Equals(lineObject.TranslationFile.FileName));
                    if (translationFile == null)
                    {
                        Console.WriteLine("Unable to find translation file: " + lineObject.TranslationFile.FileName);
                        continue;
                    }

                    if( false == translationFilesCorrected.Contains(translationFile) )
                    {
                        translationFilesCorrected.Add(translationFile);
                    }
                    translationFile.Lines.Remove(lineObject.LineNumber);
                }
            }

            return translationFilesCorrected;
        }
    }
}
