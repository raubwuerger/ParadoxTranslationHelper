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
        string _keySubstituted;
        string _originalLine;
        string _originalLineSubstituted;
        List<string> _nameSpaces = new List<string>();
        List<string> _nestingStrings = new List<string>();
        List<string> _colorCodes = new List<string>();
        List<string> _icons = new List<string>();
        List<string> _newLines = new List<string>();
        List<string> _tabulators = new List<string>();

        public LineObject(LineObject lineObject)
        {
            if (lineObject == null)
            {
                throw new ArgumentNullException("Parameter <lineObject> must not be null!");
            }

            this._lineNumber = lineObject.LineNumber;
            this._translationFile = lineObject.TranslationFile;
            this._key = lineObject.Key;
            this._keySubstituted = lineObject._keySubstituted;
            this._originalLine = lineObject.OriginalLine;
            this._originalLineSubstituted = lineObject.OriginalLineSubstituted;
            this._nameSpaces = new List<string>(lineObject.NameSpaces);
            this._nestingStrings = new List<string>(lineObject._nestingStrings);
            this._colorCodes = new List<string>(lineObject._colorCodes);
            this._icons = new List<string>(lineObject._icons);
            this._newLines = new List<string>(lineObject._newLines);
            this._tabulators = new List<string>(lineObject._tabulators);
        }

        public LineObject(int lineNumber)
        {
            _lineNumber = lineNumber;
        }

        public LineObject(int lineNumber, LineObject lineObject)
        {
            _lineNumber = lineNumber;
            CopyEverythingExceptLineNumber(lineObject);
        }

        //TODO: 2025-11-14 - JHA - Extract Method anwenden
        private void CopyEverythingExceptLineNumber(LineObject lineObject)
        {
            if (lineObject == null) 
            {
                Key = null;
                KeySubstituted = null;
                TranslationFile = null;
                NameSpaces = null;
                NestingStrings = null;
                ColorCodes = null;
                Icons = null;
                OriginalLine = null;
                OriginalLineSubstituted = null;
                NewLines = null;
                Tabulators = null;
            }
            else 
            {
                Key = lineObject.Key;
                KeySubstituted = lineObject._keySubstituted;
                TranslationFile = lineObject.TranslationFile;
                NameSpaces = lineObject.NameSpaces;
                NestingStrings = lineObject.NestingStrings;
                ColorCodes = lineObject.ColorCodes;
                Icons = lineObject.Icons;
                OriginalLine = lineObject.OriginalLine;
                OriginalLineSubstituted = lineObject.OriginalLineSubstituted;
                NewLines = lineObject.NewLines;
                Tabulators = lineObject.Tabulators;
            }
        }

        private LineObject() { }

        public int LineNumber { get => _lineNumber; }
        public string Key { get => _key; set => _key = value; }
        public string KeySubstituted { get => _keySubstituted; set => _keySubstituted = value; }
        public TranslationFile TranslationFile { get => _translationFile; set => _translationFile = value; }
        public List<string> NameSpaces 
        { 
            get => _nameSpaces; 
            set => _nameSpaces = value; 
        }
        public List<string> NestingStrings { get => _nestingStrings; set => _nestingStrings = value; }
        public List<string> ColorCodes { get => _colorCodes; set => _colorCodes = value; }
        public List<string> Icons { get => _icons; set => _icons = value; }
        public string OriginalLine { get => _originalLine; set => _originalLine = value; }
        public string OriginalLineSubstituted { get => _originalLineSubstituted; set => _originalLineSubstituted = value; }
        public List<string> NewLines { get => _newLines; set => _newLines = value; }
        public List<string> Tabulators { get => _tabulators; set => _tabulators = value; }
        public bool HasKey() { return false == string.IsNullOrEmpty(_key); }
    }
}
