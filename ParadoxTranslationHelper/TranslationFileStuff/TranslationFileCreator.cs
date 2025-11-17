using System;
using System.Collections.Generic;
using System.IO;
using Serilog;

namespace ParadoxTranslationHelper
{
    internal class TranslationFileCreator
    {
        LineObjectCreator _lineObjectCreator = new LineObjectCreator();
        public TranslationFile? Create(string completeFileName)
        {
            if (string.IsNullOrEmpty(completeFileName))
            {
                Log.Verbose("Parameter <completeFileName> must not be null or empty!");
                return null;
            }

            TranslationFile translationFile = FileNameSetter(completeFileName);

            _lineObjectCreator.TranslationFile = translationFile;
            translationFile.Lines = CreateLineObjects(File.ReadAllLines(completeFileName));

            return translationFile;
        }

        public TranslationFile CopyExceptFileName( string filename, TranslationFile other )
        {
            TranslationFile translationFile = new TranslationFile(filename);
            translationFile.BasePath = other.BasePath;
            translationFile.FileNameWithoutLocalisation = Utility.CreateFileNameWithoutLocalisation(filename);
            translationFile.Lines = other.Lines;

            return translationFile;
        }

        public static TranslationFile? CreateEmpty( string completeFileName)
        {
            if( string.IsNullOrEmpty(completeFileName))
            {
                Log.Verbose("Parameter <completeFileName> must not be null or empty!");
                return null;
            }

            return FileNameSetter(completeFileName);
        }

        /**
         * First entry must contain filename
         */
        public TranslationFile Create(List<string> lines)
        {
            if( lines == null ) 
            {
                Log.Verbose("Parameter <lines> must not be null!");
                return null;
            }

            if (lines.Count == 0)
            {
                Log.Verbose("Parameter <lines> must not be empty!");
                return null;
            }

            string fileName = lines[0];
            lines.RemoveAt(0);

            if (string.IsNullOrEmpty(fileName) ) 
            {
                Log.Verbose("Parameter <lines>: First entry must contain valid file name!");
                return null;
            }

            TranslationFile translationFile = new TranslationFile(fileName);
            translationFile.BasePath = GetBasePath(fileName);
            translationFile.FileNameWithoutLocalisation = Utility.CreateFileNameWithoutLocalisation(fileName);

            _lineObjectCreator.TranslationFile = translationFile;
            translationFile.Lines = CreateLineObjects(lines.ToArray());

            return translationFile;
        }

        private Dictionary<int,LineObject> CreateLineObjects(string[] lines)
        {
            if (lines == null || lines.Length == 0)
            {  
                return null;
            }   

            Dictionary<int, LineObject> lineObjects = new Dictionary<int, LineObject>();
            List<LineTextTupel> lineTextTupels = new List<LineTextTupel>();

            int lineNumber = 0;
            foreach (string line in lines)
            {
                if( false == IgnoreLine(line) )
                {
                    SetKey(line);
                    SetNamespaces(line);
                    SetNestingStrings(line);
                    SetIcons(line);
//TODO: 2025-11-14 - JHA - Prüfen warum das nicht mehr funktioniert!
//                    SetNewLine(line);
                    SetColorCodes(line);
                }

                lineNumber++;
                LineObject lineObject = _lineObjectCreator.Create(lineNumber);
                lineObject.OriginalLine = Utility.TruncateOriginalLine(lineObject, line);
                lineObjects.Add(lineNumber, lineObject);
            }

            return lineObjects;
        }

        private bool IgnoreLine(string line) 
        {
            if( true == string.IsNullOrEmpty(line) )
            {
                return true;
            }
            string lineTrimmed = line.Trim();
            if (lineTrimmed.StartsWith(Constants.SIGN_HASH_TAG) )
            {
                return true;
            }

            return false;
        }

        private void SetKey(string line)
        {
            IStringParser stringParser = StringParserFactory.Instance.CreateParserKey();
            List<string> token = new List<string>();
            token = stringParser.GetToken(line, token);
            if (token.Count > 0)
            {
                _lineObjectCreator.Key = token[0];
            }
        }

      
        private void SetNamespaces(string line)
        {
            IStringParser stringParser = StringParserFactory.Instance.CreateParserNamespaces();
            List<string> token = new List<string>();
            _lineObjectCreator.NameSpace = stringParser.GetToken(line, token);
        }

        private void SetNestingStrings(string line)
        {
            IStringParser stringParser = StringParserFactory.Instance.CreateParserNestingStrings();
            List<string> token = new List<string>();
            _lineObjectCreator.NestingStrings = stringParser.GetToken(line, token);
        }

        private void SetColorCodes(string line)
        {
            IStringParser stringParser = StringParserFactory.Instance.CreateParserColorCodes(); 
            List<string> token = new List<string>();
            _lineObjectCreator.ColorCodes = stringParser.GetToken(line, token);
        }

        private void SetIcons(string line) 
        {
            IStringParser stringParser = StringParserFactory.Instance.CreateParserIcons();
            List<string> token = new List<string>();
            _lineObjectCreator.Icons = stringParser.GetToken(line, token);
        }

        private void SetNewLine(string line)
        {
            IStringParser stringParser = StringParserFactory.Instance.CreateParserNewLine();
            List<string> token = new List<string>();
            _lineObjectCreator.NewLines = stringParser.GetToken(line, token);
        }

        private static TranslationFile FileNameSetter(string fileNameComplete)
        {
            if (true == string.IsNullOrEmpty(fileNameComplete))
            {
                return null;
            }

            string fileName = Path.GetFileName(fileNameComplete);
            if ( true == string.IsNullOrEmpty(fileName) )
            {
                return null;
            }

            TranslationFile translationFile = new TranslationFile(fileName);
            translationFile.BasePath = GetBasePath(fileNameComplete);
            translationFile.FileNameWithoutLocalisation = Utility.CreateFileNameWithoutLocalisation(fileNameComplete);

            return translationFile;
        }

        private static string GetBasePath(string fileNameComplete)
        {
            if (true == string.IsNullOrEmpty(fileNameComplete))
            {
                return null;
            }

            return Path.GetDirectoryName(fileNameComplete);
        }

        private static string GetSubDirectory(string fileNameComplete)
        {
            if (true == string.IsNullOrEmpty(fileNameComplete))
            {
                return null;
            }

            string fileName = Path.GetFileName(fileNameComplete);
            string subDirectory = Path.GetDirectoryName(fileNameComplete);

            return fileNameComplete.Replace(Path.GetFileName(fileNameComplete), "").Replace(Path.GetDirectoryName(fileNameComplete), "");
        }
    }
}
