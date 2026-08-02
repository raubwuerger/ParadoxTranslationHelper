using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal interface ILineCorrector
    {
        string Name
        {
            get;
        }
        void Correct(List<string> lines);
        List<string> GetIncorrect();
        string GetIncorrectFileExtension();
        List<string> GetCorrected();
        string GetCorrectFileExtension();

    }
}
