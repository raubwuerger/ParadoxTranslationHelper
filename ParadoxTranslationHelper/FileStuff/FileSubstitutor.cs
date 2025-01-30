using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    internal class FileSubstitutor
    {
        public void ReSubstitute(TranslationFileSetSubstitution translationFileSetSubstitution)
        {
            if (translationFileSetSubstitution == null)
            {
                return;
            }
            ReSubstitutor reSubstitutor = new ReSubstitutor();
            reSubstitutor.TranslationFileSetSubstitution = translationFileSetSubstitution;
            reSubstitutor.ReSubstitute();
        }

        public bool Substitute(TranslationFile translationFile)
        {
            if (translationFile == null)
            {
                return false;
            }

            Substitutor substitutor = new Substitutor();
            return substitutor.Substitute(translationFile);
        }

    }
}
