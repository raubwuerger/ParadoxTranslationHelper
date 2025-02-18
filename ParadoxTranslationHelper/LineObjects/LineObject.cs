using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class LineObject
    {
        int _lineNumber;
        TranslationFile _translationFile;
        string _key;
        List<string> _nameSpaces = new List<string>();
        List<string> _nestingStrings = new List<string>();
        List<string> _colorCodes = new List<string>();
        List<string> _icons = new List<string>();
        List<string> _newLines = new List<string>();
        string _originalLine;
        string _originalLineSubstituted;
        public LineObject(int lineNumber)
        {
            _lineNumber = lineNumber;
        }

        public LineObject(int lineNumber, LineObject lineObject)
        {
            _lineNumber = lineNumber;
            CopyEverythingExceptLineNumber(lineObject);
        }

        private void CopyEverythingExceptLineNumber(LineObject lineObject)
        {
            if (lineObject == null) 
            {
                Key = null;
                TranslationFile = null;
                NameSpaces = null;
                NestingStrings = null;
                ColorCodes = null;
                Icons = null;
                OriginalLine = null;
                OriginalLineSubstituted = null;
                NewLines = null;
            }
            else 
            {
                Key = lineObject.Key;
                TranslationFile = lineObject.TranslationFile;
                NameSpaces = lineObject.NameSpaces;
                NestingStrings = lineObject.NestingStrings;
                ColorCodes = lineObject.ColorCodes;
                Icons = lineObject.Icons;
                OriginalLine = lineObject.OriginalLine;
                OriginalLineSubstituted = lineObject.OriginalLineSubstituted;
                NewLines = lineObject.NewLines;
            }
        }

        private LineObject() { }

        public int LineNumber { get => _lineNumber; }
        public string Key { get => _key; set => _key = value; }
        public TranslationFile TranslationFile { get => _translationFile; set => _translationFile = value; }
        public List<string> NameSpaces { get => _nameSpaces; set => _nameSpaces = value; }
        public List<string> NestingStrings { get => _nestingStrings; set => _nestingStrings = value; }
        public List<string> ColorCodes { get => _colorCodes; set => _colorCodes = value; }
        public List<string> Icons { get => _icons; set => _icons = value; }
        public string OriginalLine { get => _originalLine; set => _originalLine = value; }
        public string OriginalLineSubstituted { get => _originalLineSubstituted; set => _originalLineSubstituted = value; }
        public List<string> NewLines { get => _newLines; set => _newLines = value; }
    }
}
