using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class StringParserKey : StringParserBase
    {
        public override List<string> GetToken(string source, List<string> tokens)
        {
            if (false == source.Contains(StartTag))
            {
                return tokens;
            }

            if (true == IgnoreLine(source))
            {
                return tokens;
            }

            int startPos = source.IndexOf(StartTag, 0) + StartTag.Length;
            int endPos = startPos;
            startPos = 0;

            string tokenToAdd;

            if (SubStringCount == 0)
            {
                int count = endPos;
                if (count > 0)
                {
                    count--;
                }
                tokenToAdd = source.Substring(startPos, count).TrimEnd();
            }
            else
            {
                tokenToAdd = source.Substring(startPos, SubStringCount).TrimEnd();
            }

            tokens.Add(tokenToAdd.TrimStart());

            return tokens;
        }

    }
}
