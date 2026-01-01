using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorMissingQuotationMark : ILineCorrector
    {
        List<string> _correctedLines = new List<string>();

        public void Correct(List<string> lines)
        {
            _correctedLines.Clear();
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
                if ( true == IgnoreLine(line) )
                {
                    _correctedLines.Add(line);
                    continue;
                }

                CorrectQuotationMarks(line);
            }
        }

        bool IgnoreLine(string line)
        {
            if (true == line.Trim().StartsWith(Constants.TRANSLATION_FILE_IDENTIFIER))
            {
                return true;
            }

            return false;
        }

        void CorrectQuotationMarks( string line )
        {
            if (true == string.IsNullOrWhiteSpace(line))
            {
                return;
            }
            line = CorrectQuotationMarkEnd(line);
            line = CorrectQuotationMarkStart(line);
            _correctedLines.Add(line);
        }

        string CorrectQuotationMarkEnd(string line)
        {
            line = line.TrimEnd();
            if (line[line.Length - 1] != Constants.QUOTATION_MARKS_CHAR)
            {
                Log.Verbose("Appended quotation mark at the end.");
                line += Constants.QUOTATION_MARKS_CHAR;
            }

            return line;
        }

        string CorrectQuotationMarkStart(string line)
        {
            const int KEY_LENGTH = 16;
            string SubLine = line.Substring(KEY_LENGTH);
            SubLine = SubLine.TrimStart();
            if (SubLine[0] != Constants.QUOTATION_MARKS_CHAR)
            {
                Log.Verbose("Appended quotation mark after key.");
                line = line.Insert(KEY_LENGTH, Constants.QUOTATION_MARKS);
            }

            return line;
        }

        public List<string> GetIncorrect()
        {
            return new List<string>();
        }

        public List<string> GetCorrected()
        {
            return _correctedLines;
        }

        public string GetIncorrectFileExtension()
        {
            return "MQM_incorrect";
        }

        public string GetCorrectFileExtension()
        {
            return "MQM_correct";
        }

    }
}
