using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectDiffSteam : FunctionObjectBase
    {
        public FunctionObjectDiffSteam(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            return CheckNewKeysUpdate();
        }

        private bool CheckNewKeysUpdate()
        {
            Dictionary<string, LineObject> updated = GetKeys(LocalisationEnglishUpdated);
            Dictionary<string, LineObject> old = GetKeys(LocalisationEnglish);
            Dictionary<string, LineObject> toCreate = new Dictionary<string, LineObject>();

            foreach (KeyValuePair<string, LineObject> pair in updated)
            {
                if (old.ContainsKey(pair.Key))
                {
                    continue;
                }
                toCreate.Add(pair.Key, pair.Value);
            }

            string containedInFile = "";
            Console.WriteLine("Following keys (" + toCreate.Count + ") are new in update: " + Path.GetFullPath(LocalisationEnglishUpdated[0].FileName) + Environment.NewLine);
            foreach (KeyValuePair<string, LineObject> pair in toCreate)
            {
                if (false == containedInFile.Equals(pair.Value.TranslationFile.FileName))
                {
                    Console.WriteLine("##### " + pair.Value.TranslationFile);
                    containedInFile = pair.Value.TranslationFile.FileName;
                }
                Console.WriteLine(pair.Key + ";" + pair.Value.OriginalLine);
            }

            string directory = CreateDirectory();
            if (null == directory)
            {
                Console.WriteLine("Unable to create directory! " + Path.Combine(ParadoxTranslationHelperConfig.PathBase, ParadoxTranslationHelperConfig.PathResult));
                return false;
            }

            Utility.WriteLines(toCreate.Values.ToList(), Path.Combine(directory, "MissingTranslationKeys.yml"));

            Dictionary<string, LineObject> toDelete = new Dictionary<string, LineObject>();
            foreach (KeyValuePair<string, LineObject> pair in old)
            {
                if (updated.ContainsKey(pair.Key))
                {
                    continue;
                }
                toDelete.Add(pair.Key, pair.Value);
            }

            Console.WriteLine();
            Console.WriteLine();
            containedInFile = "";
            Console.WriteLine("Following keys (" + toDelete.Count + ") should be deleted: " + Path.GetFullPath(LocalisationEnglishUpdated[0].FileName) + Environment.NewLine);
            foreach (KeyValuePair<string, LineObject> pair in toDelete)
            {
                if (false == containedInFile.Equals(pair.Value.TranslationFile.FileName))
                {
                    Console.WriteLine("##### " + pair.Value.TranslationFile);
                    containedInFile = pair.Value.TranslationFile.FileName;
                }
                Console.WriteLine(pair.Key + ";" + pair.Value.OriginalLine);
            }

            return true;
        }

        private Dictionary<string, LineObject> GetKeys(List<TranslationFile> files)
        {
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
