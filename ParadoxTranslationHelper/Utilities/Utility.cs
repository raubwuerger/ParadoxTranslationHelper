using ParadoxTranslationHelper.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace ParadoxTranslationHelper
{
    public class Utility
    {
        public static string CreateStringFromTranslationFile(TranslationFile translationFile)
        {
            if (translationFile == null)
            {
                return null;
            }

            Dictionary<int, LineObject> _lines = translationFile.Lines;
            List<LineObject> lines = _lines.Values.ToList();

            string completeString = "";
            foreach ( LineObject lineObject in lines )
            {
                string substitutedLine = lineObject.OriginalLineSubstituted;
                if ( substitutedLine == null )
                {
                    substitutedLine = lineObject.OriginalLine;
                }
                completeString += substitutedLine;
                completeString += Environment.NewLine;
            }

            return completeString;
        }

        public static string ToString( KeyValuePair<string, List<LineObject>> keyValuePair )
        {
            string toString = keyValuePair.Key;
            toString += ";";

            foreach( LineObject lineObject in keyValuePair.Value) 
            {
                if( false == toString[toString.Length - 1].Equals(';') )
                {
                    toString += "|";
                }
                toString += lineObject.TranslationFile + ";" +lineObject.LineNumber;
            }
            return toString;
        }

        public static string GetSubstitutedLineTabbed(LineObject lineObject)
        {
            if (lineObject.OriginalLineSubstituted == null)
            {
                lineObject.OriginalLineSubstituted = lineObject.OriginalLine;
            }

            return GetStringTabbed(lineObject);
        }

        public static string GetSubstitutedLine(LineObject lineObject)
        {
            if (lineObject.OriginalLineSubstituted == null)
            {
                lineObject.OriginalLineSubstituted = lineObject.OriginalLine;
            }

            return lineObject.OriginalLineSubstituted;
        }

        private static string GetStringTabbed(LineObject lineObject)
        {
            if( string.IsNullOrEmpty(lineObject.Key) )
            {
                return lineObject.OriginalLineSubstituted;
            }

            int indexOfKay = lineObject.OriginalLineSubstituted.IndexOf(lineObject.Key);
            int endOfKey = indexOfKay +lineObject.Key.Length;

            //Remove space after key.
            if( lineObject.OriginalLineSubstituted[endOfKey] == ' ' )
            {
                endOfKey++;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(lineObject.Key);
            sb.Append("\t");
            sb.Append(lineObject.OriginalLineSubstituted.Substring(endOfKey));
            lineObject.OriginalLineSubstituted = sb.ToString();

            return lineObject.OriginalLineSubstituted;
        }

        public static string ReplaceWithAnalyseDirectory(TranslationFile translationFile )
        {
            string fullPath = Path.GetDirectoryName(translationFile.FileName);
            DirectoryInfo directoryInfo = Directory.GetParent(fullPath);
            string analysePath = Path.Combine(directoryInfo.FullName, ParadoxTranslationHelperConfig.PathResult);

            return Path.Combine(analysePath, Path.GetFileName(translationFile.FileName));
        }

        public static TranslationFile ConvertToGerman(TranslationFile translationFile)
        {
            if (null == translationFile)
            {
                Console.WriteLine("Parameter <translationFile> must not be null!");
                return null;
            }

            TranslationFileCreator translationFileCreator = new();

            TranslationFile translationConverted = translationFileCreator.CopyExceptFileName(ConvertLocalisationToGerman(translationFile.FileName), translationFile);
            translationConverted = ConvertFileContentIdentifierToGerman(translationConverted);

            return translationConverted;
        }

        public static string ConvertLocalisationToGerman( string localisation )
        {
            if ( true == string.IsNullOrEmpty(localisation) )
            {
                Console.WriteLine("Parameter <localisation> must not be null nor empty!");
                return null;
            }

            string path = Path.GetDirectoryName(localisation);
            string pathParent = "";
            string pathConverted = "";
            if ( false == string.IsNullOrEmpty(path) )
            {
                pathParent = Directory.GetParent(path).FullName;
                pathConverted = Path.Combine(pathParent, Constants.LOCALISATION_GERMAN);
            }

            string fileName = Path.GetFileName(localisation);
            string fileNameConverted = fileName.Replace(Constants.LOCALISATION_ENGLISH_FULL, Constants.LOCALISATION_GERMAN_FULL);

            return Path.Combine(pathConverted, fileNameConverted);
        }

        public static TranslationFile ConvertFileContentIdentifierToGerman( TranslationFile translationFile )
        {
            if (null == translationFile)
            {
                Console.WriteLine("Parameter <translationFile> must not be null!");
                return null;
            }

            foreach ( KeyValuePair<int,LineObject> lineObject in translationFile.Lines)
            {
                if( false == lineObject.Value.OriginalLine.Contains(Constants.LOCALISATION_ENGLISH_FILE_IDENTIFIER) )
                { 
                    continue; 
                }

                lineObject.Value.OriginalLine = lineObject.Value.OriginalLine.Replace(Constants.LOCALISATION_ENGLISH_FILE_IDENTIFIER, Constants.LOCALISATION_GERMAN_FILE_IDENTIFIER);
                break;
            }

            return translationFile;
        }

        public static string? RemoveAllFileExtensions( string fileName ) 
        { 
            if( string.IsNullOrEmpty(fileName) )
            {
                Console.WriteLine("Parameter <fileNam> must not be null or empty!");
                return null;
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if( fileName.Equals(Path.GetFileNameWithoutExtension(fileName)) )
            {
                return fileNameWithoutExtension;
            }

            return RemoveAllFileExtensions(fileNameWithoutExtension);
        }

        public static List<string>? ConvertToList(string[] lines)
        {
            if (lines.Length == 0)
            {
                Console.WriteLine("Parameter <lines> must not be null!");
                return null;
            }

            return new List<string>(lines);
        }

        public static string? CreateFileNameWithoutLocalisationGerman(TranslationFile translationFile)
        {
            if (null == translationFile)
            {
                Console.WriteLine("Parameter <translationFile> must not be null!");
                return null;
            }
            return translationFile.FileNameWithoutLocalisation + Constants.LOCALISATION_GERMAN_FULL + Constants.LOCALISATION_EXTENSION;
        }

        /**
         * Replaces localisationName and basePath
         * 
         * 
         * 
         */
        public static string? CreateFileNameGerman(TranslationFile translationFile, string basePath)
        {
            if (null == translationFile)
            {
                Console.WriteLine("Parameter <translationFile> must not be null!");
                return null;
            }

            if( string.IsNullOrEmpty(basePath) ) 
            {
                Console.WriteLine("Parameter <basePath> must not be null or empty!");
                return null;
            }

            return Path.Combine(basePath, translationFile.FileNameWithoutLocalisation + Constants.LOCALISATION_GERMAN_FULL +Constants.LOCALISATION_EXTENSION);
        }

        public static Dictionary<string, LineObject> ExtractKeys(List<TranslationFile> translationFiles)
        {
            if (null == translationFiles)
            {
                Console.WriteLine("Parameter <translationFiles> must not be null!");
                return null;
            }

            if (false == translationFiles.Any())
            {
                Console.WriteLine("Parameter <translationFiles> must not be empty!");
                return null;
            }

            Dictionary<string, LineObject> keys = new Dictionary<string, LineObject>();
            foreach (TranslationFile translationFile in translationFiles)
            {
                keys = keys.Union(DictionaryHelper.GetValidKeys(translationFile.Lines.Values.ToList()).Where(k => !keys.ContainsKey(k.Key))).ToDictionary(k => k.Key, v => v.Value);
            }

            return keys;
        }

    }

}
