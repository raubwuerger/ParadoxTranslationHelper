using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Utilities
{
    public static class FunctionUtility
    {
        public static Dictionary<string, LineObject> FindToCreate(Dictionary<string, LineObject> repository, Dictionary<string, LineObject> steam)
        {
            if (repository == null)
            {
                return steam;
            }

            if (repository.Count == 0)
            {
                return steam;
            }

            Dictionary<string, LineObject> toCreate = new Dictionary<string, LineObject>();
            foreach (KeyValuePair<string, LineObject> pair in steam)
            {
                if (repository.ContainsKey(pair.Key))
                {
                    continue;
                }
                toCreate.Add(pair.Key, pair.Value);
            }

            return toCreate;
        }

        public static List<TranslationFile>? CreateKeys(string pathKeys)
        {
            if (string.IsNullOrWhiteSpace(pathKeys))
            {
                Console.WriteLine("Parameter <pathKeys> must not be null or empty!");
                return null;
            }

            List<TranslationFile> keys = new List<TranslationFile>();
            TranslationFileCreator translationFileCreator = new TranslationFileCreator();

            List<string> lines = Utility.ConvertToList(File.ReadAllLines(pathKeys));
            List<string> foundFile = new List<string>();
            foreach (string line in lines)
            {
                if (false == foundFile.Any() && true == ContainsFileName(line))
                {
                    string fileName = CreateFileName(line);
                    if (true == string.IsNullOrEmpty(fileName))
                    {
                        continue;
                    }

                    foundFile.Add(fileName);
                    continue;
                }

                if (true == foundFile.Any() && false == ContainsFileName(line))
                {
                    foundFile.Add(line);
                    continue;
                }

                if (true == ContainsFileName(line))
                {
                    keys.Add(translationFileCreator.Create(foundFile));
                    foundFile = new List<string>();
                    string fileName = CreateFileName(line);
                    if (true == string.IsNullOrEmpty(fileName))
                    {
                        continue;
                    }

                    foundFile.Add(fileName);
                }
            }

            if (true == foundFile.Any())
            {
                keys.Add(translationFileCreator.Create(foundFile));
            }

            return keys;
        }

        public static bool ContainsFileName(string fileName)
        {
            return fileName.Contains(Constants.TRANSLATION_FILE_IDENTIFIER);
        }

        public static string? CreateFileName(string line)
        {
            string fileName = ExtractFileNameFromString(line);
            if (true == string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            return fileName;
        }

        private static string? ExtractFileNameFromString(string containsFileName)
        {
            if (string.IsNullOrEmpty(containsFileName))
            {
                return null;
            }

            int indexFileNameStart = containsFileName.IndexOf(Constants.TRANSLATION_FILE_IDENTIFIER);
            if (indexFileNameStart == -1)
            {
                Console.WriteLine("_translationFileIdentifier not found!");
                return null;
            }

            string fileName = Path.GetFileName(containsFileName.Substring(indexFileNameStart));
            int startFileExtension = fileName.IndexOf(Constants.LOCALISATION_EXTENSION);

            if (startFileExtension == -1)
            {
                Console.WriteLine("Not a valid localization file: LOCALISATION_EXTENSION not found!");
                return null;
            }

            return fileName.Remove(startFileExtension + Constants.LOCALISATION_EXTENSION.Length);
        }

    }
}
