using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectDiff : FunctionObjectBase
    {
        private string _pathRepository;
        private string _pathSteam;
        private string _resultFileName;

        public string PathRepository { get => _pathRepository; set => _pathRepository = value; }
        public string PathSteam { get => _pathSteam; set => _pathSteam = value; }
        public string ResultFileName { get => _resultFileName; set => _resultFileName = value; }

        public FunctionObjectDiff(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            if( _pathRepository == null) 
            {
                Console.WriteLine("Member <PathRepository> must not be null!");
                return false;
            }

            if (_pathSteam == null)
            {
                Console.WriteLine("Member <PathSteam> must not be null!");
                return false;
            }

            if(_resultFileName == null) 
            {
                Console.WriteLine("Member <ResultFileName> must not be null!");
                return false;
            }

            LocalisationEnglish = Utility.CreateTranslationFilesFromDirectory(_pathRepository);
            LocalisationEnglishUpdated = Utility.CreateTranslationFilesFromDirectory(_pathSteam);

            return CheckNewKeysUpdate();
        }

        protected bool CheckNewKeysUpdate()
        {
            Dictionary<string, LineObject> updated = GetKeys(LocalisationEnglishUpdated);
            Dictionary<string, LineObject> old = GetKeys(LocalisationEnglish);
            Dictionary<string, LineObject> toCreate = new Dictionary<string, LineObject>();

            if (old != null)
            {
                foreach (KeyValuePair<string, LineObject> pair in updated)
                {
                    if (old.ContainsKey(pair.Key))
                    {
                        continue;
                    }
                    toCreate.Add(pair.Key, pair.Value);
                }
            }
            else
            {
                toCreate = updated;
            }

            string directory = Utility.CreateDirectoryAnalysis();
            if (null == directory)
            {
                Console.WriteLine("Unable to create directory! " + directory);
                return false;
            }

            if (toCreate.Values.Count > 0)
            {
                Utility.WriteLinesPushFrontTranslationIdentifier(toCreate.Values.ToList(), Path.Combine(directory, ResultFileName));
            }
            else
            {
                Utility.WriteEmptyFileUTF8_BOM(Path.Combine(directory, "SteamDiff_NoMissingKeysFound.txt"));
            }

            return true;
        }

        private Dictionary<string, LineObject> GetKeys(List<TranslationFile> files)
        {
            if( null == files )
            {
                Console.WriteLine("No translation files found!");
                return null;
            }

            if (false == files.Any())
            {
                Console.WriteLine("No translation files found!");
                return null;
            }

            Dictionary<string, LineObject> keys = new Dictionary<string, LineObject>();
            foreach (TranslationFile translationFile in files)
            {
                keys = keys.Union(Utility.GetValidKeys(translationFile.Lines.Values.ToList()).Where(k => !keys.ContainsKey(k.Key))).ToDictionary(k => k.Key, v => v.Value);
            }

            return keys;
        }

    }
}
