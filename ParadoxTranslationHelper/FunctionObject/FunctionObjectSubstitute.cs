using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectSubstitute : FunctionObjectBase
    {
        public FunctionObjectSubstitute(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            LocalisationEnglish = Utility.AnalyseDirectory(ParadoxTranslationHelperConfig.PathEnglish);

            foreach (TranslationFile translationFile in LocalisationEnglish)
            {
                FileSubstitutor fileSubstitutor = new FileSubstitutor();
                fileSubstitutor.Substitute(translationFile);
            }

            return true;
        }
    }
}
