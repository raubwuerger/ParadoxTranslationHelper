using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class LineObjectCreator
    {
        TranslationFile _translationFile;
        public TranslationFile TranslationFile { get => _translationFile; set => _translationFile = value; }
        string _key = "";
        public string Key { get => _key; set => _key = value; }
        List<string> _nameSpace = new List<string>();
        public List<string> NameSpace { get => _nameSpace; set => _nameSpace = value; }
        List<string> _nestingStrings = new List<string>();
        public List<string> NestingStrings { get => _nestingStrings; set => _nestingStrings = value; }
        List<string> _colorCodes = new List<string>();
        public List<string> ColorCodes { get => _colorCodes; set => _colorCodes = value; }
        List<string> _icons = new List<string>();
        public List<string> Icons { get => _icons; set => _icons = value; }
        public List<string> NewLines { get => _newLines; set => _newLines = value; }

        List<string> _newLines = new List<string>();


        public LineObject Create( int lineNumber )
        {
            LineObject lineObject = new LineObject( lineNumber );
            lineObject.TranslationFile = _translationFile;
            lineObject.Key = _key;
            lineObject.NameSpaces = _nameSpace;
            lineObject.NestingStrings = _nestingStrings;
            lineObject.ColorCodes = _colorCodes;
            lineObject.Icons = _icons;
            lineObject.NewLines = _newLines;

            CleanUp();

            return lineObject;
        }

        private void CleanUp()
        {
            _key = "";
            _nameSpace = new List<string>();
            _nestingStrings = new List<string>();
            _colorCodes = new List<string>();
            _icons = new List<string>();
            _newLines = new List<string>();
        }
    }
}
