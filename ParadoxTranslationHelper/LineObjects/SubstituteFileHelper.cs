using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineObjects
{
    internal static class SubstituteFileHelper
    {
        public static string CreateSubKeyLineTripel(string sub, LineObject lineObject)
        {
            return $"{sub}{Constants.DEFAULT_SEPARATOR}{lineObject.Key}{Constants.DEFAULT_SEPARATOR}{lineObject.LineNumber}";
        }

//        public 

    }
}
