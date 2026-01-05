using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Helper
{
    internal class LineHelper
    {
        public static bool IgnoreLine( string line )
        {
            if( null == line )
            {
                return true;
            }

            string trimmedLine = line.Trim();
            if (true == trimmedLine.StartsWith(Constants.TRANSLATION_FILE_IDENTIFIER))
            {
                return true;
            }

            if( true == trimmedLine.StartsWith(Constants.SIGN_HASH_TAG))
            {
                return true;
            }

            if (true == string.IsNullOrWhiteSpace(trimmedLine))
            {
                return true;
            }

            return false;

        }
    }
}
