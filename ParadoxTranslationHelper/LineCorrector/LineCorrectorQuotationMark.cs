using ParadoxTranslationHelper.Helper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorQuotationMark : ILineCorrector
    {
        public static List<string> _wrongQuotationMarks = new List<string> { "„", "“", "”", "‘", "‚" };

        List<string> _correctedLines = new List<string>();

        public string Name { get => "LineCorrectorQuotationMark"; }

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
                if ( true == LineHelper.IgnoreLine(line) )
                {
                    _correctedLines.Add(line);
                    continue;
                }

                if( true == CorrectQuotationMarks(line) )
                {
                    continue;
                }
            }
        }

        bool CorrectQuotationMarks( string line )
        {
            foreach( string wrongQM in _wrongQuotationMarks )
            {
                line = line.Replace(wrongQM, Constants.QUOTATION_MARKS);
            }
            _correctedLines.Add(line);
            return true;
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
            return "QM_incorrect";
        }

        public string GetCorrectFileExtension()
        {
            return "QM_correct";
        }

    }
}
