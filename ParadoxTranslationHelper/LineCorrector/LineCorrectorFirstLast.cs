using ParadoxTranslationHelper.Helper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorFirstLast : ILineCorrector
    {
        string _correctSign = "\"";
        public static string _incorrectSign1 = "„";
        public static string _incorrectSign2 = "“";
        List<string> _incorrectLines = new List<string>();

        public string Name { get => "LineCorrectorFirstLast"; }

        public void Correct(List<string> lines)
        {
            _incorrectLines.Clear();
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
                    continue;
                }

                if( true == CorrectQuotationMarks(line) )
                {
                    continue;
                }

                _incorrectLines.Add(line);
            }
        }

        /**
         * return value == true: nothing to correct, or corrected!
         *              == false: unable to correct!
         */
        private bool CorrectQuotationMarks( string line )
        {
            int indexCorrectStart = line.IndexOf(Constants.QUOTATION_MARKS);
            int indexCorrectEnd = line.LastIndexOf(Constants.QUOTATION_MARKS);

            int indexStart = ContainsWrongStart(line);
            if ( indexStart == -1 && indexCorrectStart == -1 )
            {
                return true;
            }

            if( indexStart == -1 )
            {
                indexStart = indexCorrectStart;
            }

            int indexEnd = ContainsWrongEnd(line);
            if( indexEnd == -1 )
            {
                //TODO: 2025-12-31 - JHA - Wenn das Endezeichen aber korrekt ist?
                Log.Verbose("Only incorrect start sign detected!");
                return false;
            }

            //TODO: 2025-12-31 - JHA - Das funktioniert noch nicht ...
            if( indexStart == indexEnd && (indexCorrectStart != indexCorrectEnd || indexCorrectStart == -1 || indexCorrectEnd == -1) )
            {
                return false;
            }

            if( indexCorrectStart == -1 && indexStart != -1 )
            {
                line = Replace(line, indexStart, 1, _correctSign);
            }

            if( indexCorrectEnd == -1 && indexEnd != -1 )
            {
                line = Replace(line, indexEnd, 1, _correctSign);
            }

            return true;
        }

        /**
         * Returns -1 if not found!
         */
        int ContainsWrongStart(string line)
        {
            int indexFirst = line.IndexOf(_incorrectSign1);
            int indexSecond = line.IndexOf(_incorrectSign2);

            if( indexFirst == -1 && indexSecond == -1 )
            {
                return -1;
            }

            int returnIndex = indexFirst < indexSecond ? indexFirst : indexSecond;

            if( returnIndex == -1 )
            {
                returnIndex = indexSecond == -1 ? indexFirst : indexSecond;
            }

            return returnIndex;
        }

        int ContainsWrongEnd(string line)
        {
            int indexFirst = line.LastIndexOf(_incorrectSign1);
            int indexSecond = line.LastIndexOf(_incorrectSign2);

            if (indexFirst == -1 && indexSecond == -1)
            {
                return -1;
            }

            int returnIndex = indexFirst > indexSecond ? indexFirst : indexSecond;

            if (returnIndex == -1)
            {
                returnIndex = indexSecond == -1 ? indexFirst : indexSecond;
            }

            return returnIndex;
        }

        string Replace(string text, int start, int count, string replacement)
        {
            return text.Substring(0, start) + replacement + text.Substring(start + count);
        }
        public List<string> GetIncorrect()
        {
            return _incorrectLines;
        }

        public List<string> GetCorrected()
        {
            return new List<string>();
        }

        public string GetIncorrectFileExtension()
        {
            return "FL_incorrect";
        }

        public string GetCorrectFileExtension()
        {
            return "FL_correct";
        }
    }
}
