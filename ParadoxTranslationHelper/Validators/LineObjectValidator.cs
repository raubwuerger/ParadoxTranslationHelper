using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Validators
{
    public static class LineObjectValidator
    {
        public static bool IsValid( LineObject lineObject )
        {
            if (lineObject == null)
            {
                Console.WriteLine("Parameter <lineObject> must not be null!");
                return false;
            }

            if( true == IsIgnoreLine(lineObject.OriginalLine) ) 
            {
                return true;
            }

            return true;
        }

        private static bool IsIgnoreLine( string originalLine )
        {
            if (true == string.IsNullOrWhiteSpace(originalLine))
            {
                return true;
            }

            if( true == originalLine.TrimStart().StartsWith('#') )
            {
                return true;
            }

            return false;
        }
    }
}
