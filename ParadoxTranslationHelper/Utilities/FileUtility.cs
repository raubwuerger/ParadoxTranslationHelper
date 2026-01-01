using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Serilog;

namespace ParadoxTranslationHelper.Utilities
{
    public class FileUtility
    {
        public static List<TranslationFile> CreateTranslationFilesFromDirectory(string directory, string filePattern = "*.yml")
        {
            if ( true == string.IsNullOrEmpty(directory) )
            {
                Log.Verbose("Parameter <Directory> must not be null!");
                return null;
            }

            if (false == Directory.Exists(directory))
            {
                Log.Verbose("Directory doesn't exist: " + directory);
                return null;
            }

            string[] files = Directory.GetFiles(directory, filePattern, SearchOption.AllDirectories);
            if (files.Length <= 0)
            {
                Log.Verbose("Directory contains no files: " + directory);
                return new List<TranslationFile>();
            }

            Log.Verbose("Parsing directory: " + directory);
            List<TranslationFile> translationFiles = new List<TranslationFile>();

            foreach (string file in files)
            {
                translationFiles.Add(CreateTranslationFileFromFile(file));
            }

            return translationFiles;
        }

        public static TranslationFile CreateTranslationFileFromFile( string fileName )
        {
            if( false == File.Exists(fileName) )
            {
                Log.Verbose("File not found! " + fileName);
                return null;
            }

            TranslationFileCreator translationFileCreator = new TranslationFileCreator();
            translationFileCreator.StringParserKey = StringParserFactory.Instance.CreateParserKey();
            translationFileCreator.StringParserNamespaces = StringParserFactory.Instance.CreateParserNamespaces();
            translationFileCreator.StringParserNestingStrings = StringParserFactory.Instance.CreateParserNestingStrings();
            translationFileCreator.StringParserIcons = StringParserFactory.Instance.CreateParserIcons();
            translationFileCreator.StringParserNewLine = StringParserFactory.Instance.CreateParserNewLine();
            translationFileCreator.StringParserColorCodes = StringParserFactory.Instance.CreateParserColorCodes();
            translationFileCreator.StringParserTabulator = StringParserFactory.Instance.CreateParserTabulator();

            return translationFileCreator.Create(fileName);
        }

        public static TranslationFile CreateTranslationFileFromFileResub(string fileName)
        {
            if (false == File.Exists(fileName))
            {
                Log.Verbose("File not found! " + fileName);
                return null;
            }

            TranslationFileCreator translationFileCreator = new TranslationFileCreator();
            translationFileCreator.StringParserKey = StringParserFactory.Instance.CreateParserKeyResub();
            translationFileCreator.StringParserNamespaces = StringParserFactory.Instance.CreateParserNamespacesResub();
            translationFileCreator.StringParserNestingStrings = StringParserFactory.Instance.CreateParserNestingStringsResub();
            translationFileCreator.StringParserIcons = StringParserFactory.Instance.CreateParserIconsResub();
            translationFileCreator.StringParserNewLine = StringParserFactory.Instance.CreateParserNewLineResub();
            translationFileCreator.StringParserColorCodes = StringParserFactory.Instance.CreateParserColorCodesResub();
            translationFileCreator.StringParserTabulator = StringParserFactory.Instance.CreateParserTabulatorResub();
            return translationFileCreator.Create(fileName);
        }

        public static void WriteLinesPushFrontTranslationIdentifier(List<LineObject> lineObjects, string fileName)
        {
            if( null == lineObjects )
            {
                Log.Verbose("Parameter <lineObjects> must not be null!");
                return;
            }

            if( true == string.IsNullOrEmpty(fileName) )
            {
                Log.Verbose("Parameter <fileName> must not be null!");
                return;
            }


            Log.Verbose("Writing file: " + fileName);
            try
            {
                ClearFileContent(fileName);
                using (Stream stream = File.OpenWrite(fileName))
                using (StreamWriter outputFile = new StreamWriter(stream, new UTF8Encoding(true)))
                {
                    string missingKeyFile = "";
                    foreach (LineObject line in lineObjects)
                    {
                        if (false == missingKeyFile.Equals(line.TranslationFile.FileNameWithBasePath))
                        {
                            missingKeyFile = line.TranslationFile.FileNameWithBasePath;
                            outputFile.WriteLine(Constants.TRANSLATION_FILE_IDENTIFIER + missingKeyFile);
                        }
                        outputFile.WriteLine(Utility.GetSubstitutedLineTabbed(line));
                    }
                }
            }
            catch(Exception ex) 
            {
                Log.Fatal(ex.ToString());
            }
        }

        public static void WriteLines(List<LineObject> lineObjects, string fileName)
        {
            if (null == lineObjects)
            {
                Log.Verbose("Parameter <lineObjects> must not be null!");
                return;
            }

            if (true == string.IsNullOrEmpty(fileName))
            {
                Log.Verbose("Parameter <fileName> must not be null!");
                return;
            }

            Log.Verbose("Writing file: " + fileName);
            try
            {
                ClearFileContent(fileName);
                using (Stream stream = File.OpenWrite(fileName))
                using (StreamWriter outputFile = new StreamWriter(stream, new UTF8Encoding(true)))
                {
                    foreach (LineObject line in lineObjects)
                    {
                        outputFile.WriteLine(Utility.GetSubstitutedLineTabbed(line));
                    }
                }
                Log.Verbose("Writing file: " + fileName + " successful.");
            }
            catch (Exception ex)
            {
                Log.Warning("Writing file: " + fileName + " failed!");
                Log.Fatal(ex.ToString());
            }
        }

        public static void WriteLines(Dictionary<string, string> strings, string fileName)
        {
            if (null == strings)
            {
                Log.Verbose("Parameter <lineObjects> must not be null!");
                return;
            }

            if (true == string.IsNullOrEmpty(fileName))
            {
                Log.Verbose("Parameter <fileName> must not be null!");
                return;
            }

            Log.Verbose("Writing file: " + fileName);
            try
            {
                ClearFileContent(fileName);
                using (Stream stream = File.OpenWrite(fileName))
                using (StreamWriter outputFile = new StreamWriter(stream, new UTF8Encoding(true)))
                {
                    foreach (KeyValuePair<string,string> line in strings)
                    {
                        outputFile.WriteLine(line);
                    }
                }
                Log.Verbose("Writing file: " + fileName + " successful.");
            }
            catch (Exception ex)
            {
                Log.Warning("Writing file: " + fileName + " failed!");
                Log.Fatal(ex.ToString());
            }
        }
        public static bool Write(TranslationFile translationFile)
        {
            if (translationFile == null)
            {
                Log.Verbose("Parameter <translationFile> must not be null!");
                return false;
            }

            return Write(translationFile, translationFile.FileNameWithBasePath);
        }

        public static bool Write(TranslationFile translationFile, string fileName)
        {
            if (translationFile == null)
            {
                Log.Verbose("Parameter <translationFile> must not be null!");
                return false;
            }

            if (true == string.IsNullOrEmpty(fileName))
            {
                Log.Verbose("Parameter <fileName> must not be null!");
                return false;
            }

            Dictionary<int, LineObject> _lines = translationFile.Lines;
            List<LineObject> lineObjects = _lines.Values.ToList();

            Log.Verbose($"Writing file: {fileName}");
            try
            {
                ClearFileContent(fileName);
                CreateDirectoryNotExists(fileName);
                using (Stream stream = File.OpenWrite(fileName))
                using (StreamWriter outputFile = new StreamWriter(stream, new UTF8Encoding(true)))
                {
                    foreach (LineObject line in lineObjects)
                    {
                        outputFile.WriteLine(Utility.GetSubstitutedLine(line));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Fatal($"Exception occured: {ex.ToString()}");
                return false;
            }
        }

        public static void ClearFileContent( string fileName )
        {
            if ( false == File.Exists(fileName) )
            {
                return;
            }

            using (FileStream fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                lock (fs)
                {
                    fs.SetLength(0);
                }
            }
        }

        public static void CreateDirectoryNotExists( string fileName )
        {
            if (true == File.Exists(fileName))
            {
                return;
            }

            DirectoryInfo directoryInfo = Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            Log.Debug($"Created directory: {directoryInfo}");
        }

        public static bool WriteEmptyFileUTF8_BOM(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Log.Verbose("Parameter <filename> must not be null or empty!");
                return false;
            }

            try
            {
                ClearFileContent(fileName);
                using (Stream stream = File.OpenWrite(fileName))
                using (var outputFile = new StreamWriter(stream, new UTF8Encoding(true)))
                {
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Fatal("Exception occurred: " + e.ToString());
                return false;
            }
        }

        public static bool Write(Dictionary<string, List<string>> doubleKeyFiles)
        {
            if (doubleKeyFiles == null)
            {
                Log.Verbose("Parameter <doubleKeyFiles> must not be null or empty!");
                return false;
            }

            if (doubleKeyFiles.Count == 0)
            {
                Log.Verbose("Parameter <doubleKeyFiles> must not be empty!");
                return false;
            }

            foreach (var keyFile in doubleKeyFiles)
            {
                try
                {
                    ClearFileContent(keyFile.Key);
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
                    Log.Fatal("Exception occurred: " + e.ToString());
                    continue;
                }
            }

            return true;
        }

        public static bool Write(Dictionary<string, List<LineObject>> doubleKeyFiles, string fileName)
        {
            if (doubleKeyFiles == null)
            {
                Log.Verbose("Parameter <doubleKeyFiles> must not be null or empty!");
                return false;
            }

            if (doubleKeyFiles.Count == 0)
            {
                Log.Verbose("Parameter <doubleKeyFiles> must not be empty!");
                return false;
            }

            try
            {
                ClearFileContent(fileName);
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
                Log.Fatal("Exception occurred: " + e.ToString());
                return false;
            }

            return true;
        }

        public static string? CreateDirectoryAnalysis()
        {
            if (false == Directory.Exists(ParadoxTranslationHelperConfig.PathResult))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(ParadoxTranslationHelperConfig.PathResult);
                if (null == directoryInfo)
                {
                    return null;
                }
            }
            return ParadoxTranslationHelperConfig.PathResult;
        }

        public static Dictionary<string,string>? ReadSuffixFile(string suffixFile)
        {
            if( true == string.IsNullOrWhiteSpace(suffixFile) )
            {
                Log.Warning("Parameter <suffix> must not be null!");
                return null;
            }

            Dictionary<string, string> suffixContent = new Dictionary<string, string>();

            if( false == File.Exists(suffixFile) )
            {
                Log.Warning($"File {suffixFile} doesn't exists.");
                return null;
            }
            var lines = File.ReadLines(suffixFile);
            foreach (var line in lines)
            {
                string[] splitted = line.Split(";");
                if( splitted.Length < 2 )
                {
                    continue;
                }
                suffixContent.Add(splitted[0], splitted[1]);
            }

            return suffixContent;
        }

    }
}
