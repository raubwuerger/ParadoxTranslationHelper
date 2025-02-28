using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    internal class TranslationFileCreator
    {
        LineObjectCreator _lineObjectCreator = new LineObjectCreator();
        public TranslationFile CopyExceptFileName( string filename, TranslationFile other )
        {
            TranslationFile translationFile = new TranslationFile(filename);
            translationFile.FileNameWithoutLocalisation = CreateFileNameWithoutLocalisation(filename);
            translationFile.Lines = other.Lines;

            return translationFile;
        }

        public TranslationFile Create(string fileName)
        {
            if( string.IsNullOrEmpty(fileName) )
            {
                Console.WriteLine("Parameter <fileName> must not be null or empty!");
                return null;
            }

            TranslationFile translationFile = new TranslationFile(fileName);
            translationFile.FileNameWithoutLocalisation = CreateFileNameWithoutLocalisation(fileName);

            _lineObjectCreator.TranslationFile = translationFile;
            translationFile.Lines = CreateLineObjects(File.ReadAllLines(fileName));

            return translationFile;
        }

        /**
         * First entry must contain filename
         */
        public TranslationFile Create(List<string> lines)
        {
            if( lines == null ) 
            {
                Console.WriteLine("Parameter <lines> must not be null!");
                return null;
            }

            if (lines.Count == 0)
            {
                Console.WriteLine("Parameter <lines> must not be empty!");
                return null;
            }

            string fileName = lines[0];
            lines.RemoveAt(0);

            if (string.IsNullOrEmpty(fileName) ) 
            {
                Console.WriteLine("Parameter <lines>: First entry must contain valid file name!");
                return null;
            }

            TranslationFile translationFile = new TranslationFile(fileName);
            translationFile.FileNameWithoutLocalisation = CreateFileNameWithoutLocalisation(fileName);

            _lineObjectCreator.TranslationFile = translationFile;
            translationFile.Lines = CreateLineObjects(lines.ToArray());

            return translationFile;
        }


        private string CreateFileNameWithoutLocalisation(string fileName)
        {
            string fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
            int indexOf_LocalisationStartString = fileNameOnly.IndexOf(Constants.LOCALISATION_START_STRING, StringComparison.OrdinalIgnoreCase);
            if (indexOf_LocalisationStartString != -1)
            {
                return fileNameOnly.Substring(0, indexOf_LocalisationStartString);
            }
            else
            {
                return fileNameOnly;
            }
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
                    SetNewLine(line);
                }

                lineNumber++;
                LineObject lineObject = _lineObjectCreator.Create(lineNumber);
                lineObject.OriginalLine = ProcessOriginalLine(lineObject, line);
                lineObjects.Add(lineNumber, lineObject);
            }

            return lineObjects;
        }

        private string ProcessOriginalLine(LineObject lineObject, string line)
        {
            if(lineObject == null)
            {
                return line;
            }

            if( false == lineObject.HasKey() )
            {
                return line;
            }

            int lastIndex = line.LastIndexOf(Constants.SIGN_HASH_TAG);
            if( lastIndex == -1 )
            {
                return line;
            }

            return line.Substring(0, lastIndex);
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
    }
}
