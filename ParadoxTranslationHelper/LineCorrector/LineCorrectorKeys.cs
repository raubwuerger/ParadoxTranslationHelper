using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorKeys : ILineCorrector
    {
        List<string> _doubleKeys = new List<string> { "" };
        Dictionary<string,string> _uniqueKeys = new Dictionary<string,string>();
        string _correctStart = "|___KY";
        string _correctEnd = "___|";
        int _correctEndLength = 0;
        int _correctLength = 16;
        List<string> _incorrectLines = new List<string>();

        public void Correct(List<string> lines)
        {
            _incorrectLines.Clear();
            _doubleKeys.Clear();
            _uniqueKeys.Clear();

            _correctEndLength = _correctEnd.Length;

            if ( null == lines )
            {
                Log.Warning("Parameter lines must not be null!");
                return;
            }

            if ( lines.Count() == 0 )
            {
                Log.Warning("Parameter lines must not be empty!");
                return;
            }

            foreach (string line in lines)
            {
                if( true == IgnoreLine(line) )
                {
                    continue;
                }

                if( true == IsKeyCorrect(line) )
                {
                    if(_uniqueKeys.ContainsKey(line))
                    {
                        _doubleKeys.Add(line);
                    }
                    else
                    {
                        _uniqueKeys.Add(line, line);
                    }
                    continue;
                }

                _incorrectLines.Add(line);

                if ( true == TryToCorrect(line) )
                {
                    continue;
                }
            }
        }

        bool IsKeyCorrect( string line )
        {
            int indexStart = line.IndexOf(_correctStart);
            if ( indexStart == -1 )
            {
                Log.Verbose($"Start string {_correctStart} not found!");
                return false;
            }

            int indexEnd = line.IndexOf(_correctEnd);
            if( indexEnd == -1 )
            {
                Log.Verbose($"End string {_correctEnd} not found!");
                return false;
            }

            int length = (indexEnd + _correctEndLength) - indexStart;
            if( length != _correctLength )
            {
                Log.Verbose($"Length mismatch: should={_correctLength}, is={length}");
                return false;
            }

            return true;
        }

        bool TryToCorrect(string line)
        {
            //TODO: 2025-12-30 - JHA - To implement
            return false;
        }

        bool IgnoreLine(string line)
        {
            if( true == line.Trim().StartsWith(Constants.TRANSLATION_FILE_IDENTIFIER) )
            {
                return true;
            }

            if( true == string.IsNullOrWhiteSpace(line) )
            {
                return true;
            }

            return false;
        }

        public List<string> GetIncorrect()
        {
            return _doubleKeys;
        }
        public List<string> GetCorrected()
        {
            return _uniqueKeys.Keys.ToList<string>();
        }
        public string GetIncorrectFileExtension()
        {
            return "dbl";
        }

        public string GetCorrectFileExtension()
        {
            return "Key_correct";
        }

    }
}
