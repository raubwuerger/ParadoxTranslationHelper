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
        public FunctionObjectDiff(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            LocalisationEnglish = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathEnglish);
            LocalisationEnglishUpdated = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathSteam);
            ResultFileName = "MissingTranslationKeys.yml";

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

            string directory = CreateDirectory();
            if (null == directory)
            {
                Console.WriteLine("Unable to create directory! " + Path.Combine(ParadoxTranslationHelperConfig.PathBase, ParadoxTranslationHelperConfig.PathResult));
                return false;
            }

            Utility.WriteLinesPushFrontTranslationIdentifier(toCreate.Values.ToList(), Path.Combine(directory, ResultFileName ));

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

        private string? CreateDirectory()
        {
            string pathDiff = Path.Combine(ParadoxTranslationHelperConfig.PathBase, ParadoxTranslationHelperConfig.PathResult);
            if (false == Directory.Exists(pathDiff))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(pathDiff);
                if (null == directoryInfo)
                {
                    return null;
                }
            }
            return pathDiff;
        }

    }
}
