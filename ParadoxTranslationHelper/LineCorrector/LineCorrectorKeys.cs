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
        List<string> _possibleWrong = new List<string> { "" };
        string _correctStart = "|___KY";
        string _correctEnd = "___|";
        int _correctEndLength = 0;
        int _correctLength = 16;
        public void Correct(List<string> lines)
        {
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
                    continue;
                }

                if( true == TryToCorrect(line) )
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

            return false;
        }
    }
}
