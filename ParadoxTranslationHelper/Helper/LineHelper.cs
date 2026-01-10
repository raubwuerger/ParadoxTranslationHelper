using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Helper
{
    internal class LineHelper
    {
        public static bool IgnoreLine( string line )
        {
            if( null == line )
            {
                return true;
            }

            string trimmedLine = line.Trim();
            if (true == trimmedLine.StartsWith(Constants.TRANSLATION_FILE_IDENTIFIER))
            {
                return true;
            }

            if( true == trimmedLine.StartsWith(Constants.SIGN_HASH_TAG))
            {
                return true;
            }

            if (true == string.IsNullOrWhiteSpace(trimmedLine))
            {
                return true;
            }

            return false;

        }

        static string _correctStart = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.KEY_SUFFIX;
        static string _correctEnd = FileSubstitutionConstants.SUBSTITUTION_END;
        static int _correctEndLength = _correctEnd.Length;
        static int _correctLength = 16;

        public static int CorrectLength { get => _correctLength; }

        public static bool HasLineCorrectKey(string line)
        {
            int indexStart = line.IndexOf(_correctStart);
            if (indexStart == -1)
            {
                Log.Verbose($"Start string {_correctStart} not found!");
                return false;
            }

            int indexEnd = line.IndexOf(_correctEnd);
            if (indexEnd == -1)
            {
                Log.Verbose($"End string {_correctEnd} not found!");
                return false;
            }

            int length = (indexEnd + _correctEndLength) - indexStart;
            if (length != _correctLength)
            {
                Log.Verbose($"Length mismatch: should={_correctLength}, is={length}");
                return false;
            }

            return true;
        }

        public static string SetLineToIgnore(string line)
        {
            return line.Insert(0, Constants.IGNORE_LINE + " ");
        }

        public static bool IsLineFileToStoreLine(string line)
        {
            return line.Contains(Constants.TRANSLATION_FILE_IDENTIFIER);
        }

    }
}
