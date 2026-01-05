using ParadoxTranslationHelper.Helper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorSubstitution : ILineCorrector
    {
        string _substitutionSuffix;
        List<string> _correctedLines = new List<string>();
        Dictionary<string, LineObjectSubstitutionFile> _substitutions;

        public string Name { get => "LineCorrectorSubstitution"; }
        public string SubstitutionSuffix { get => _substitutionSuffix; set => _substitutionSuffix = value; }
        public Dictionary<string, LineObjectSubstitutionFile> Substitutions { get => _substitutions; set => _substitutions = value; }

        public void Correct(List<string> lines)
        {
            _correctedLines.Clear();
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
            return _substitutionSuffix +".incorrect";
        }

        public string GetCorrectFileExtension()
        {
            return _substitutionSuffix +".correct";
        }

    }
}
