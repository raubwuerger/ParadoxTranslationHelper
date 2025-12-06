using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class StringParserFirstLast : StringParserBase
    {
        protected override List<string> GetToken(string source, List<string> tokens)
        {
            if (false == source.Contains(StartTag))
            {
                return tokens;
            }
            int startPos = source.IndexOf(StartTag, 0) + StartTag.Length;

            foreach (string endTag in EndTags)
            {
                string subString = source.Substring(startPos, source.Length - startPos);

                if (false == subString.Contains(endTag))
                {
                    continue;
                }
                int endPos = source.LastIndexOf(endTag, startPos);

                if (SubStringCount == 0)
                {
                    int count = source.Length - endPos - 1;
                    if (count > 0)
                    {
                        count--;
                    }
                    tokens.Add(source.Substring(startPos, count));
                }
                else
                {
                    tokens.Add(source.Substring(startPos, SubStringCount));
                }
            }

            return tokens;
        }

    }
}
