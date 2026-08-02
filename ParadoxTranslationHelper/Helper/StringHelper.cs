using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Helper
{
    internal class StringHelper
    {
        public static int CountCharsUsingLinqCount(string source, char toFind)
        {
            return source.Count(t => t == toFind);
        }
    }
}
