using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class StringParserFirstLast : StringParserBase
    {
        public override List<string> GetToken(string source, List<string> tokens)
        {
            if (false == source.Contains(StartTag))
            {
                return tokens;
            }
            int _startPos = source.IndexOf(StartTag, 0) + StartTag.Length;

            foreach (string endTag in EndTags)
            {
                string subString = source.Substring(_startPos, source.Length - _startPos);

                if (false == subString.Contains(endTag))
                {
                    continue;
                }
                int _endPos = source.LastIndexOf(endTag, _startPos);

                if (SubStringCount == 0)
                {
                    int count = source.Length - _endPos - 1;
                    if (count > 0)
                    {
                        count--;
                    }
                    tokens.Add(source.Substring(_startPos, count));
                }
                else
                {
                    tokens.Add(source.Substring(_startPos, SubStringCount));
                }
            }

            return tokens;
        }
    }
}
