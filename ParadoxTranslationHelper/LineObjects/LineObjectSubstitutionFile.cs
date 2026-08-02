using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class LineObjectSubstitutionFile
    {
        string _substitute;
        string _substitutedValue;
        string _key;
        int _lineNumber;

        public string Substitute { get => _substitute; }
        public string SubstitutedValue { get => _substitutedValue; set => _substitutedValue = value; }
        public string Key { get => _key; set => _key = value; }
        public int LineNumber { get => _lineNumber; set => _lineNumber = value; }

        public LineObjectSubstitutionFile(string substitute)
        {
            _substitute = substitute;
        }

        public LineObjectSubstitutionFile(LineObjectSubstitutionFile @object)
        {
            if (@object == null)
            {
                throw new ArgumentNullException("Parameter <LineObjectSubstitutionFile> must not be null!");
            }

            this._substitute = @object._substitute;
            this._substitutedValue = @object._substitutedValue;
            this._key = @object.Key;
            this._lineNumber = @object.LineNumber;
        }

        private LineObjectSubstitutionFile() { }

    }
}
