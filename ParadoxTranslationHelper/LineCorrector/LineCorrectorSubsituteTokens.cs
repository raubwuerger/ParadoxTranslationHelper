using ParadoxTranslationHelper.Helper;
using ParadoxTranslationHelper.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorSubsituteTokens : ILineCorrector
    {
        List<string> _correctedLines = new List<string>();
        public string Name { get => "LineCorrectorSubsituteTokens"; }

        public void Correct(List<string> lines)
        {
            _correctedLines.Clear();
            foreach (string line in lines)
            {
                if( true == LineHelper.IsLineFileToStoreLine(line) )
                {
                    _correctedLines.Add(line);
                    continue;
                }
                SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
                string corrected = corrector.CorrectLine(line);
                if( corrected == null )
                {
                    if( true == corrector.ContainsAtLeastOneCorruptToken )
                    {
                        Log.Warning($"Line contains corrupt token: {line}");
                    }
                    corrected = line;
                }
                _correctedLines.Add(corrected);
            }
        }

        public List<string> GetCorrected()
        {
            return _correctedLines;
        }

        public string GetCorrectFileExtension()
        {
            return "LCST_Corrected";
        }

        public List<string> GetIncorrect()
        {
            return new List<string>();
        }

        public string GetIncorrectFileExtension()
        {
            return "LCST_NotCorrected";
        }
    }
}
