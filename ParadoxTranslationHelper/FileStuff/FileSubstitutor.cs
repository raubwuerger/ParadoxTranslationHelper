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
