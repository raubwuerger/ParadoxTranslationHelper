using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Utilities
{
    public class FileUtility
    {
        public static List<TranslationFile> CreateTranslationFilesFromDirectory(string directory, string filePattern = "*.yml")
        {
            if (null == directory)
            {
                return null;
            }

            if (false == Directory.Exists(directory))
            {
                Console.WriteLine("Directory doesn't exist: " + directory);
                return null;
            }

            string[] files = Directory.GetFiles(directory, filePattern, SearchOption.AllDirectories);
            if (files.Length <= 0)
            {
                return null;
            }

            List<TranslationFile> translationFiles = new List<TranslationFile>();
            TranslationFileCreator translationFileCreator = new TranslationFileCreator();

            foreach (string file in files)
            {
                translationFiles.Add(translationFileCreator.Create(file));
            }

            return translationFiles;
        }

        public static void WriteLinesPushFrontTranslationIdentifier(List<LineObject> lineObjects, string fileName)
        {
            Console.WriteLine("Writing file: " + fileName);
            using (Stream stream = File.OpenWrite(fileName))
            using (StreamWriter outputFile = new StreamWriter(stream, new UTF8Encoding(true)))
            {
                string missingKeyFile = "";
                foreach (LineObject line in lineObjects)
                {
                    if (false == missingKeyFile.Equals(line.TranslationFile.FileName))
                    {
                        missingKeyFile = line.TranslationFile.FileName;
                        outputFile.WriteLine(Constants.TRANSLATION_FILE_IDENTIFIER + missingKeyFile);
                    }
                    outputFile.WriteLine(Utility.GetSubstitutedLineTabbed(line));
                }
            }
        }

        public static void WriteLines(List<LineObject> lineObjects, string fileName)
        {
            Console.WriteLine("Writing file: " + fileName);
            using (Stream stream = File.OpenWrite(fileName))
            using (StreamWriter outputFile = new StreamWriter(stream, new UTF8Encoding(true)))
            {
                foreach (LineObject line in lineObjects)
                {
                    outputFile.WriteLine(Utility.GetSubstitutedLineTabbed(line));
                }
            }
        }

        public static void WriteTranslationFile(TranslationFile translationFile)
        {
            if (translationFile == null)
            {
                return;
            }

            WriteTranslationFile(translationFile, translationFile.FileName);
        }

        public static void WriteTranslationFile(TranslationFile translationFile, string fileName)
        {
            if (translationFile == null)
            {
                return;
            }

            Dictionary<int, LineObject> _lines = translationFile.Lines;
            List<LineObject> lineObjects = _lines.Values.ToList();

            Console.WriteLine("Writing file: " + fileName);
            using (Stream stream = File.OpenWrite(fileName))
            using (StreamWriter outputFile = new StreamWriter(stream, new UTF8Encoding(true)))
            {
                foreach (LineObject line in lineObjects)
                {
                    outputFile.WriteLine(Utility.GetSubstitutedLine(line));
                }
            }
        }

        public static bool WriteEmptyFileUTF8_BOM(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                Console.WriteLine("Parameter <filename> must not be null or empty!");
                return false;
            }

            try
            {
                using (Stream stream = File.OpenWrite(filename))
                using (var outputFile = new StreamWriter(stream, new UTF8Encoding(true)))
                {
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred: " + e.ToString());
                return false;
            }
        }

        public static bool Write(Dictionary<string, List<string>> doubleKeyFiles)
        {
            if (doubleKeyFiles == null)
            {
                Console.WriteLine("Parameter <doubleKeyFiles> must not be null or empty!");
                return false;
            }

            if (doubleKeyFiles.Count == 0)
            {
                Console.WriteLine("Parameter <doubleKeyFiles> must not be empty!");
                return false;
            }

            foreach (var keyFile in doubleKeyFiles)
            {
                try
                {
                    using (Stream stream = File.OpenWrite(keyFile.Key))
                    using (var outputFile = new StreamWriter(stream, new UTF8Encoding(true)))
                    {
                        List<string> temp = keyFile.Value;
                        foreach (string doubleKey in keyFile.Value)
                        {
                            outputFile.WriteLine(doubleKey);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception occurred: " + e.ToString());
                    continue;
                }
            }

            return true;
        }

        public static bool Write(Dictionary<string, List<LineObject>> doubleKeyFiles, string fileName)
        {
            if (doubleKeyFiles == null)
            {
                Console.WriteLine("Parameter <doubleKeyFiles> must not be null or empty!");
                return false;
            }

            if (doubleKeyFiles.Count == 0)
            {
                Console.WriteLine("Parameter <doubleKeyFiles> must not be empty!");
                return false;
            }

            try
            {
                using (Stream stream = File.OpenWrite(fileName))
                using (var outputFile = new StreamWriter(stream, new UTF8Encoding(true)))
                {
                    foreach (KeyValuePair<string, List<LineObject>> doubleKey in doubleKeyFiles)
                    {
                        outputFile.WriteLine(Utility.ToString(doubleKey));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred: " + e.ToString());
                return false;
            }

            return true;
        }

        public static string? CreateDirectoryAnalysis()
        {
            string pathAnalyze = Path.Combine(ParadoxTranslationHelperConfig.PathBase, ParadoxTranslationHelperConfig.PathResult);
            if (false == Directory.Exists(pathAnalyze))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(pathAnalyze);
                if (null == directoryInfo)
                {
                    return null;
                }
            }
            return pathAnalyze;
        }


    }
}
