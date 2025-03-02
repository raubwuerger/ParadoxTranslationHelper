using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public static class SubstitutionHelper
    {
        public static string? CreateFileNameResub(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            return fileName + FileSubstitutionConstants.FILE_SUFFIX_SUBSTITUTED + FileSubstitutionConstants.FILE_SUFFIX_GERMAN;
        }

    }
}
