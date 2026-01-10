using ParadoxTranslationHelper.Helper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorSplitByKeys : ILineCorrector
    {
        string _correctStart = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.KEY_SUFFIX;
        string _correctEnd = FileSubstitutionConstants.SUBSTITUTION_END;
        int _correctEndLength = 0;
        int _correctLength = 16;
        List<string> _splittedLines = new List<string>();
        public string Name { get => "LineCorrectorSplitByKeys"; }

        public void Correct(List<string> lines)
        {
            _splittedLines.Clear();
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
                if( true == LineHelper.IgnoreLine(line) )
                {
                    _splittedLines.Add(line);
                    continue;
                }

                SplitByKey(line);
            }
        }

        void SplitByKey( string line )
        {
            int indexStart = line.IndexOf(_correctStart);
            if ( indexStart == -1 )
            {
                Log.Verbose($"Start string {_correctStart} not found!");
                _splittedLines.Add(line);
                return;
            }

            int indexEnd = line.IndexOf(_correctEnd);
            if( indexEnd == -1 )
            {
                Log.Verbose($"End string {_correctEnd} not found!");
                _splittedLines.Add(line);
                return;
            }

            int length = (indexEnd + _correctEndLength) - indexStart;
            if( length != _correctLength )
            {
                Log.Verbose($"Length mismatch: should={_correctLength}, is={length}");
                _splittedLines.Add(line);
                return;
            }

            int anotherKeyStartIndex = line.IndexOf(_correctStart, length);
            if( anotherKeyStartIndex == -1 )
            {
                _splittedLines.Add(line);
                return;
            }

            _splittedLines.Add(line.Substring(0,anotherKeyStartIndex));
            SplitByKey(line.Substring(anotherKeyStartIndex));
        }

        public List<string> GetIncorrect()
        {
            return new List<string>();
        }
        public List<string> GetCorrected()
        {
            return _splittedLines;
        }

        public string GetIncorrectFileExtension()
        {
            return "SBK_incorrect";
        }

        public string GetCorrectFileExtension()
        {
            return "SBK_correct";
        }
    }
}
