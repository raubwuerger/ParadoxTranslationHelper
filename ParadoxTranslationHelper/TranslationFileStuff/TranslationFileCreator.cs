using System;
using System.Collections.Generic;
using System.IO;
using ParadoxTranslationHelper.Helper;
using Serilog;

namespace ParadoxTranslationHelper
{
    internal class TranslationFileCreator
    {
        IStringParser _stringParserKey = null;
        IStringParser _stringParserNamespaces = null;
        IStringParser _stringParserNestingStrings = null;
        IStringParser _stringParserIcons = null;
        IStringParser _stringParserNewLine = null;
        IStringParser _stringParserColorCodes = null;
        IStringParser _stringParserTabulator = null;

        LineObjectCreator _lineObjectCreator = new LineObjectCreator();

        public IStringParser StringParserKey { get => _stringParserKey; set => _stringParserKey = value; }
        public IStringParser StringParserNamespaces { get => _stringParserNamespaces; set => _stringParserNamespaces = value; }
        public IStringParser StringParserNestingStrings { get => _stringParserNestingStrings; set => _stringParserNestingStrings = value; }
        public IStringParser StringParserIcons { get => _stringParserIcons; set => _stringParserIcons = value; }
        public IStringParser StringParserNewLine { get => _stringParserNewLine; set => _stringParserNewLine = value; }
        public IStringParser StringParserColorCodes { get => _stringParserColorCodes; set => _stringParserColorCodes = value; }
        public IStringParser StringParserTabulator { get => _stringParserTabulator; set => _stringParserTabulator = value; }

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

            if( null == _stringParserKey )
            {
                Log.Debug($"Member <StringParserKey> must not be null!");
                return null;
            }

            Dictionary<int, LineObject> lineObjects = new Dictionary<int, LineObject>();
            List<LineTextTupel> lineTextTupels = new List<LineTextTupel>();

            int lineNumber = 0;
            foreach (string line in lines)
            {
                if( false == LineHelper.IgnoreLine(line) )
                {
                    //TODO: 2025-12-06 - JHA - Macht bei Substituierter Datei keinen Sinn!
                    SetKey(line, _stringParserKey);
                    SetNamespaces(line, _stringParserNamespaces);
                    SetNestingStrings(line, _stringParserNestingStrings);
                    SetIcons(line, _stringParserIcons);
                    SetNewLine(line, _stringParserNewLine);
                    SetColorCodes(line, _stringParserColorCodes);
                    SetTabulator(line, _stringParserTabulator);
                }

                lineNumber++;
                LineObject lineObject = _lineObjectCreator.Create(lineNumber);
                lineObject.OriginalLine = Utility.TruncateOriginalLine(lineObject, line);
                lineObjects.Add(lineNumber, lineObject);
            }

            return lineObjects;
        }

        private List<string> FindToken(string line, IStringParser parser )
        {
            return parser.GetToken(line);
        }

        private void SetKey(string line, IStringParser stringParser)
        {
            List<string> token = stringParser.GetToken(line);
            if (token.Count > 0)
            {
                _lineObjectCreator.Key = token[0];
            }
        }

        private void SetNamespaces(string line, IStringParser stringParser)
        {
            _lineObjectCreator.NameSpace = stringParser.GetToken(line);
        }

        private void SetNestingStrings(string line, IStringParser stringParser)
        {
            _lineObjectCreator.NestingStrings = stringParser.GetToken(line);
        }

        private void SetColorCodes(string line, IStringParser stringParser)
        {
            _lineObjectCreator.ColorCodes = stringParser.GetToken(line);
        }

        private void SetIcons(string line, IStringParser stringParser) 
        {
            _lineObjectCreator.Icons = stringParser.GetToken(line);
        }

        private void SetNewLine(string line, IStringParser stringParser)
        {
            _lineObjectCreator.NewLines = stringParser.GetToken(line);
        }

        private void SetTabulator(string line, IStringParser stringParser)
        {
            _lineObjectCreator.Tabulators = stringParser.GetToken(line);
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
