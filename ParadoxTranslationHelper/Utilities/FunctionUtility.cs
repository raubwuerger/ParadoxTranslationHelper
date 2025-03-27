using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;
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

        public static List<TranslationFile>? LoadFileAndCreateKeys(string pathKeys)
        {
            if (string.IsNullOrWhiteSpace(pathKeys))
            {
                Log.Debug("Parameter <pathKeys> must not be null or empty!");
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
                Log.Debug("_translationFileIdentifier not found!");
                return null;
            }

            string fileName = containsFileName.Substring(indexFileNameStart + Constants.TRANSLATION_FILE_IDENTIFIER.Length);
            int startFileExtension = fileName.IndexOf(Constants.LOCALISATION_EXTENSION);

            if (startFileExtension == -1)
            {
                Log.Debug("Not a valid localization file: LOCALISATION_EXTENSION not found!");
                return null;
            }

            string whatIsThis = fileName.Remove(startFileExtension + Constants.LOCALISATION_EXTENSION.Length);
            return fileName.Remove(startFileExtension + Constants.LOCALISATION_EXTENSION.Length);
        }

        public static LineObject? CreateLineObjectLanguageIdentifier(TranslationFile translationFileMissing)
        {
            if (translationFileMissing == null)
            {
                Log.Debug("Parameter <translationFileMissing> must not be null!");
                return null;
            }

            LineObject languageIdentifier = new LineObject(1);
            languageIdentifier.OriginalLine = Constants.LOCALISATION_GERMAN_FILE_IDENTIFIER;
            languageIdentifier.TranslationFile = translationFileMissing;
            return languageIdentifier;
        }

        public static TranslationFile? FindCorrespondingTranslationFile( List<TranslationFile> translationFiles, TranslationFile toFind )
        {
            if( null == translationFiles)
            {
                Log.Debug("Parameter <translationFiles> must not be null!");
                return null;
            }

            if( translationFiles.Count() == 0 )
            {
                Log.Debug("Parameter <translationFiles> contains no data!");
                return null;
            }

            if (null == toFind)
            {
                Log.Debug("Parameter <toFind> must not be null!");
                return null;
            }

            try
            {
                return translationFiles.Find(x => x.FileNameWithoutLocalisation.Equals(toFind.FileNameWithoutLocalisation));
            }
            catch (Exception ex) 
            { 
                Log.Fatal(ex.ToString());
                return null;
            }
        }

        public static LineObject FindCorrespondingLineObject( List<LineObject> lineObjects, LineObject toFind )
        {
            if (null == lineObjects)
            {
                Log.Debug("Parameter <lineObjects> must not be null!");
                return null;
            }

            if (lineObjects.Count() == 0)
            {
                Log.Debug("Parameter <lineObjects> contains no data!");
                return null;
            }

            if (null == toFind)
            {
                Log.Debug("Parameter <toFind> must not be null!");
                return null;
            }

            if( false == toFind.HasKey() )
            {
                Log.Debug("Parameter <toFind> is not a valid key!");
                return null;    
            }

            try
            {
                LineObject found = lineObjects.Find(x => x.Key.Equals(toFind.Key));
                if( found == null)
                {
                    Log.Debug("LineObject with Key not found: " +toFind.Key);
                    return null;
                }

                return found;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString());
                return null;
            }

        }

    }
}
