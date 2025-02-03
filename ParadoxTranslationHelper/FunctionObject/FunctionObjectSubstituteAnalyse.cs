using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectSubstituteAnalyse : FunctionObjectBase
    {
        public FunctionObjectSubstituteAnalyse(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            LocalisationEnglish = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathResult);

            foreach (TranslationFile translationFile in LocalisationEnglish)
            {
                FileSubstitutor fileSubstitutor = new FileSubstitutor();
                fileSubstitutor.Substitute(translationFile);
            }

            return true;
        }
    }
}
