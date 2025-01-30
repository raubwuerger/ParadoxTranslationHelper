using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class StringParser : StringParserBase
    {
        public override List<string> GetToken(string source, List<string> tokens)
        {
            if (tokens == null)
            {
                return new List<string>();
            }

            if (string.IsNullOrEmpty(source))
            {
                return tokens;
            }

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
                int _endPos = source.IndexOf(endTag, _startPos);

                int startPosCalculated = _startPos + StartIndexShift;
                if (startPosCalculated < 0 || startPosCalculated >= subString.Length)
                {
                    startPosCalculated = _startPos;
                }

                if (SubStringCount == 0)
                {
                    tokens.Add(source.Substring(startPosCalculated, _endPos - _startPos));
                }
                else
                {
                    tokens.Add(source.Substring(startPosCalculated, SubStringCount));
                }

                string remainingContent = source.Substring(_endPos + 1, source.Length - _endPos - 1);

                return GetToken(remainingContent, tokens);
            }

            return tokens;
        }
    }
}
