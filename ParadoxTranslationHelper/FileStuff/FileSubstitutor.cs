using Serilog;
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
                Log.Information("Parameter <TranslationFileSetSubstitution> must not be null!");
                return;
            }

            if( null == translationFileSetSubstitution.SubstitutedFile.Lines )
            {
                Log.Information("Parameter <TranslationFileSetSubstitution.SubstitutedFile.Lines> must not be null!");
                return;
            }

            if (null == translationFileSetSubstitution.SubstitutedFile.Lines.Count <= 0)
            {
                Log.Information("Parameter <TranslationFileSetSubstitution.SubstitutedFile.Lines> must have at least on LineObject!");
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
