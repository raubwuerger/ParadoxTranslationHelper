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

        public static string? ReplaceFirst(this string text, string search, string replace, int startIndex )
        {
            if (null == text)
            {
                return null;
            }

            if (null == search)
            {
                return null;
            }

            if (null == replace)
            {
                return null;
            }

            int pos = text.IndexOf(search, startIndex);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        // Source - https://stackoverflow.com/a/767827
        // Posted by Prashant Cholachagudda, modified by community. See post 'Timeline' for change history
        // Retrieved 2025-12-31, License - CC BY-SA 2.5

//        var indexs = "Prashant".MultipleIndex('a');

        //Extension Method's Class
        static int i = 0;
        public static int[] MultipleIndex(this string StringValue, char chChar)
        {
            var indexs = from rgChar in StringValue
                            where rgChar == chChar && i != StringValue.IndexOf(rgChar, i + 1)
                            select new { Index = StringValue.IndexOf(rgChar, i + 1), Increament = (i = i + StringValue.IndexOf(rgChar)) };
            i = 0;
            return indexs.Select(p => p.Index).ToArray<int>();
        }
    }
}
