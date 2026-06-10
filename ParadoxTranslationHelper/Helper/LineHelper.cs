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

        public static bool HasLineKeySub(string line)
        {
            if (null == line)
            {
                return false;
            }

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

        public static string? FindKeySub(string line)
        {
            if( null == line )
            {
                return null;
            }

            int indexStart = line.IndexOf(_correctStart);
            if (indexStart == -1)
            {
                Log.Verbose($"Start string {_correctStart} not found!");
                return null;
            }

            int indexEnd = line.IndexOf(_correctEnd);
            if (indexEnd == -1)
            {
                Log.Verbose($"End string {_correctEnd} not found!");
                return null;
            }

            int length = (indexEnd + _correctEndLength) - indexStart;
            if (length != _correctLength)
            {
                Log.Verbose($"Length mismatch: should={_correctLength}, is={length}");
                return null;
            }

            return line.Substring(0, _correctLength);
        }

        public static string SetLineToIgnore(string line)
        {
            if( null == line )
            {
                return Constants.IGNORE_LINE;
            }
            return line.Insert(0, Constants.IGNORE_LINE + " ");
        }

        public static bool IsLineFileToStoreLine(string line)
        {
            if( null == line )
            {
                return false;
            }
            return line.Contains(Constants.TRANSLATION_FILE_IDENTIFIER);
        }

        public static bool IsLanguageLine(LineObject lineObject)
        {
            if (true == lineObject.OriginalLine.TrimStart().StartsWith(Constants.LOCALISATION_ENGLISH_FILE_IDENTIFIER))
            {
                return true;
            }

            if (true == lineObject.OriginalLine.TrimStart().StartsWith(Constants.LOCALISATION_GERMAN_FILE_IDENTIFIER))
            {
                return true;
            }

            return false;
        }

        public static bool HasLineKeyReal(string line)
        {
            if( line == null )
            {
                return false;
            }

            int endPos = line.IndexOf(Constants.SIGN_TABULATOR);
            if (endPos == -1)
            {
                Log.Debug($"Not a valid line: {line}");
                return false;
            }

            string key = line.Substring(0, endPos);
            if (true == string.IsNullOrWhiteSpace(key))
            {
                Log.Debug($"Not a valid key: IsNullOrWhiteSpace");
                return false;
            }

            return true;
        }

        public static string? FindKeyReal(string line)
        {
            if (line == null)
            {
                return null;
            }

            int endPos = line.IndexOf(Constants.SIGN_TABULATOR);
            if (endPos == -1)
            {
                endPos = line.IndexOf(@": """);
                if (endPos == -1)
                {
                    Log.Debug($"Not a valid line: {line}");
                    return null;
                }
            }

            string key = line.Substring(0, endPos);
            if (true == string.IsNullOrWhiteSpace(key))
            {
                Log.Debug($"Not a valid key: IsNullOrWhiteSpace");
                return null;
            }

            return key;
        }

        public static Dictionary<string, string>? LinesToKeySub(List<string> lines)
        {
            if( lines == null )
            {
                return null;
            }

            Dictionary<string, string> keys = new Dictionary<string, string>();
            foreach (string line in lines)
            {
                if (true == LineHelper.IgnoreLine(line))
                {
                    keys.Add(line, line);
                    continue;
                }

                string key = LineHelper.FindKeySub(line);
                if (key == null)
                {
                    Log.Debug($"Line contains no key: {line}");
                    continue;
                }

                keys.Add(key, line);
            }

            return keys;
        }

        public static Dictionary<string, string>? LinesToKeyReal(List<string> lines)
        {
            if (lines == null)
            {
                return null;
            }

            Dictionary<string, string> keys = new Dictionary<string, string>();
            foreach (string line in lines)
            {
                if (true == LineHelper.IgnoreLine(line))
                {
                    keys.Add(line, line);
                    continue;
                }

                string key = LineHelper.FindKeyReal(line);
                if (key == null)
                {
                    Log.Debug($"Line contains no key: {line}");
                    continue;
                }

                keys.Add(key, line);
            }

            return keys;
        }

    }
}
