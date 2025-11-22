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
                Log.Verbose("Parameter <pathKeys> must not be null or empty!");
                return null;
            }

            if( false == File.Exists(pathKeys) )
            {
                Log.Warning($"File doesn't exist: {pathKeys}");
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
                Log.Verbose("_translationFileIdentifier not found!");
                return null;
            }

            string fileName = containsFileName.Substring(indexFileNameStart + Constants.TRANSLATION_FILE_IDENTIFIER.Length);
            int startFileExtension = fileName.IndexOf(Constants.LOCALISATION_EXTENSION);

            if (startFileExtension == -1)
            {
                Log.Verbose("Not a valid localization file: LOCALISATION_EXTENSION not found!");
                return null;
            }

            string whatIsThis = fileName.Remove(startFileExtension + Constants.LOCALISATION_EXTENSION.Length);
            return fileName.Remove(startFileExtension + Constants.LOCALISATION_EXTENSION.Length);
        }

        public static LineObject? CreateLineObjectLanguageIdentifier(TranslationFile translationFileMissing)
        {
            if (translationFileMissing == null)
            {
                Log.Verbose("Parameter <translationFileMissing> must not be null!");
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
                Log.Verbose("Parameter <translationFiles> must not be null!");
                return null;
            }

            if( translationFiles.Count() == 0 )
            {
                Log.Verbose("Parameter <translationFiles> contains no data!");
                return null;
            }

            if (null == toFind)
            {
                Log.Verbose("Parameter <toFind> must not be null!");
                return null;
            }

            try
            {
                TranslationFile translationFile = translationFiles.Find(x => x.FileNameWithoutLocalisation.Equals(toFind.FileNameWithoutLocalisation,StringComparison.CurrentCultureIgnoreCase));
                if ( null == translationFile)
                {
                    Log.Warning("### Corresponding translation file not found: " + toFind.FileName);
                }
                else 
                {
                    Log.Information("### Corresponding translation file found: " + toFind.FileName +" --> " + translationFile.FileName);
                }
                return translationFile;
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
                Log.Verbose("Parameter <lineObjects> must not be null!");
                return null;
            }

            if (lineObjects.Count() == 0)
            {
                Log.Verbose("Parameter <lineObjects> contains no data!");
                return null;
            }

            if (null == toFind)
            {
                Log.Verbose("Parameter <toFind> must not be null!");
                return null;
            }

            if( false == toFind.HasKey() )
            {
                Log.Verbose("Parameter <toFind> is not a valid key!");
                return null;    
            }

            try
            {
                LineObject found = lineObjects.Find(x => x.Key.Trim().Equals(toFind.Key.Trim()));
                if( found == null)
                {
                    Log.Verbose("LineObject with Key not found: " +toFind.Key);
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

        public static LineObject? FindLineObjectByKey(string key, List<LineObject> lines)
        {
            //TODO: 2025-06-12 - JHA - What happens if lines contains multiple keys which are equal?
            if( true == string.IsNullOrEmpty(key) )
            {
                Log.Warning("Parameter <string::key> must not be null or empty!");
                return null;
            }

            if( null == lines )
            {
                Log.Warning("Parameter <List<LineObject>::lines> must not be null!");
                return null;
            }

            if (false == lines.Any() )
            {
                Log.Warning("Parameter <List<LineObject>::lines> must not be empty!");
                return null;
            }

            return lines.Find( x => x.Key == key );
        }

        //TODO: 2025-06-12 - JHA - Wird anscheinend nicht mehr benötigt
        private static bool DiffNestingStringsTranslationFile(TranslationFile org, TranslationFile toVerify)
        {
            if (org == null)
            {
                Log.Verbose("Parameter <org> must not be null!");
                return false;
            }

            Log.Information("Analyzing file: " + org.FileName);

            if (org.Lines.Count() == 0)
            {
                Log.Verbose("Parameter <org> contains no Line-Objects!");
                return false;
            }

            if (toVerify == null)
            {
                Log.Verbose("Parameter <toVerify> must not be null!");
                return false;
            }

            if (toVerify.Lines.Count() == 0)
            {
                Log.Verbose("Parameter <toVerify> contains no Line-Objects!");
                return false;
            }

            List<LineObject> allLines = toVerify.Lines.Values.ToList<LineObject>();
            foreach (LineObject line in org.Lines.Values)
            {
                LineObject lineToDiff = FindLineObjectByKey(line.Key, allLines);
                if (lineToDiff == null)
                {
                    continue;
                }

//                lineToDiff
            }

            return true;
        }

        public static bool DiffNestingStrings( List<string> org, List<string> toVerify )
        {
            if (org == null)
            {
                Log.Verbose("Parameter <List<string>::org> must not be null!");
                return false;
            }

            if (toVerify == null)
            {
                Log.Verbose("Parameter <List<string>::toVerify> must not be null!");
                return false;
            }

            if ( org.Count() == 0 && toVerify.Count() == 0 )
            {
                return true;
            }

            if (toVerify.Count() == 0)
            {
                Log.Verbose("Parameter <List<string>::toVerify> must not be empty!");
                return false;
            }

            List<string> onlyInToVerify = toVerify.Except(org).ToList<string>();
            List<string> onlyInOrg = org.Except(toVerify).ToList<string>();

            return true;
        }
    }
}
