using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public static class StringExtensionMethods
    {
        public static string? ReplaceFirst(this string text, string search, string replace)
        {
            if( null == text )
            {
                return null;
            }

            if( null == search )
            {
                return null;
            }

            if( null == replace )
            {
                return null;
            }

            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
