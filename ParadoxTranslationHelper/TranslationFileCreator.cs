using System;
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
        public TranslationFile CopyExceptFileName( string filename, TranslationFile other )
        {
            TranslationFile translationFile = new TranslationFile(filename);
            translationFile.FileNameWithoutLocalisation = CreateFileNameWithoutLocalisation(filename);
            translationFile.Lines = other.Lines;

            return translationFile;
        }

        LineObjectCreator lineObjectCreator = new LineObjectCreator();
        public TranslationFile Create(string fileName)
        {
            TranslationFile translationFile = new TranslationFile(fileName);
            translationFile.FileNameWithoutLocalisation = CreateFileNameWithoutLocalisation(fileName);

            lineObjectCreator.TranslationFile = translationFile;
            translationFile.Lines = CreateLineObjects(File.ReadAllLines(fileName));

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
                    SetColorCodes(line);
                    SetIcons(line);
                    SetNewLine(line);
                }

                lineNumber++;
                LineObject lineObject = lineObjectCreator.Create(lineNumber);
                lineObject.OriginalLine = line;
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
            if (lineTrimmed.StartsWith("#") )
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
                lineObjectCreator.Key = token[0];
            }
        }

        private void SetNamespaces(string line)
        {
            IStringParser stringParser = StringParserFactory.Instance.CreateParserNamespaces();
            List<string> token = new List<string>();
            lineObjectCreator.NameSpace = stringParser.GetToken(line, token);
        }

        private void SetNestingStrings(string line)
        {
            IStringParser stringParser = StringParserFactory.Instance.CreateParserNestingStrings();
            List<string> token = new List<string>();
            lineObjectCreator.NestingStrings = stringParser.GetToken(line, token);
        }

        private void SetColorCodes(string line)
        {
            IStringParser stringParser = StringParserFactory.Instance.CreateParserColorCodes(); 
            List<string> token = new List<string>();
            lineObjectCreator.ColorCodes = stringParser.GetToken(line, token);
        }

        private void SetIcons(string line) 
        {
            IStringParser stringParser = StringParserFactory.Instance.CreateParserIcons();
            List<string> token = new List<string>();
            lineObjectCreator.Icons = stringParser.GetToken(line, token);
        }

        private void SetNewLine(string line)
        {
            IStringParser stringParser = StringParserFactory.Instance.CreateParserNewLine();
            List<string> token = new List<string>();
            lineObjectCreator.NewLines = stringParser.GetToken(line, token);
        }
    }
}
