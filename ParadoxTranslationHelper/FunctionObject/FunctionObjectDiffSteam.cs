using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.FunctionObject
{
    public class FunctionObjectDiffSteam : FunctionObjectDiff
    {
        public FunctionObjectDiffSteam(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            LocalisationEnglishUpdated = Utility.AnalyseDirectory(ParadoxTranslationHelperConfig.PathSteam);
            if (null == LocalisationEnglishUpdated)
            {
                Console.WriteLine("Steam path not set!");
                return false;
            }

            LocalisationEnglish = Utility.AnalyseDirectory(ParadoxTranslationHelperConfig.PathEnglish);
            ResultFileName = "MissingTranslationKeysSteam.yml"; ;

            return CheckNewKeysUpdate();
        }

    }
}
