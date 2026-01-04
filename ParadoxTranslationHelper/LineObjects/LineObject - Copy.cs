using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class LineObjectSubstitutionFile
    {
        string _keySubstituted;
        string _substitute;
        string _key;
        int _lineNumber;

        public LineObjectSubstitutionFile(LineObjectSubstitutionFile @object)
        {
            if (@object == null)
            {
                throw new ArgumentNullException("Parameter <LineObjectSubstitutionFile> must not be null!");
            }

            this._keySubstituted = @object._keySubstituted;
            this._substitute = @object._substitute;
            this._lineNumber = @object.LineNumber;
            this._key = @object.Key;
        }

        public LineObjectSubstitutionFile(int lineNumber)
        {
            _lineNumber = lineNumber;
        }

        public LineObjectSubstitutionFile(int lineNumber, LineObjectSubstitutionFile lineObject)
        {
            _lineNumber = lineNumber;
            CopyEverythingExceptLineNumber(lineObject);
        }

        //TODO: 2025-11-14 - JHA - Extract Method anwenden
        private void CopyEverythingExceptLineNumber(LineObjectSubstitutionFile lineObject)
        {
            if (lineObject == null) 
            {
                _keySubstituted = null;
                _substitute = null;
                _lineNumber = -1;
                _key = null;
            }
            else 
            {
                _keySubstituted = lineObject.KeySubstituted;
                _substitute = lineObject.Substitute;
                _lineNumber = lineObject.LineNumber;
                _key = lineObject.Key;
            }
        }

        private LineObjectSubstitutionFile() { }

        public int LineNumber { get => _lineNumber; }
        public string Key { get => _key; set => _key = value; }
        public string KeySubstituted { get => _keySubstituted; set => _keySubstituted = value; }
        public string Substitute { get => _substitute; set => _substitute = value; }

        public bool HasKey() { return false == string.IsNullOrEmpty(_key); }
    }
}
