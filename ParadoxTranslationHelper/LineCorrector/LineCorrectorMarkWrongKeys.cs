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
    internal class LineCorrectorMarkWrongKeys : ILineCorrector
    {
        List<string> _correctedKeys = new List<string>();

        public string Name { get => "LineCorrectorMarkWrongKeys"; }

        public void Correct(List<string> linesToCorrect)
        {
            foreach(string line in linesToCorrect)
            {
                if (false == LineHelper.HasLineCorrectKey(line))
                {
                    _correctedKeys.Add(LineHelper.SetLineToIgnore(line));
                    continue;
                }
                _correctedKeys.Add(line);
            }
        }

        List<string> ILineCorrector.GetCorrected()
        {
            return _correctedKeys;
        }

        string ILineCorrector.GetCorrectFileExtension()
        {
            return "LineCorrectorMarkWrongKeys.correct";
        }

        List<string> ILineCorrector.GetIncorrect()
        {
            return new List<string>();
        }

        string ILineCorrector.GetIncorrectFileExtension()
        {
            return "LineCorrectorMarkWrongKeys.incorrect";
        }
    }
}
