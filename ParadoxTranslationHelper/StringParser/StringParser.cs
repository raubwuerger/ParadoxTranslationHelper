using Serilog;
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

            int startIndex = source.IndexOf(StartTag, 0);
            foreach (string endTag in EndTags)
            {
                try
                {
                    string subString = source.Substring(startIndex + SubStringCount + StartIndexShift, source.Length - (startIndex + SubStringCount + StartIndexShift));

                    if (false == subString.Contains(endTag))
                    {
                        /**                        if( false == IsEndOfLine( string, ) )
                                                {
                                                    continue;
                                                }
*/
                        continue;
                    }
                    int endPos = source.IndexOf(endTag, startIndex + StartTag.Length);

                    int startPosCalculated = startIndex + StartIndexShift;
                    if (startPosCalculated < 0 )
                    {
                        startPosCalculated = startIndex;
                    }

                    if (SubStringCount == 0)
                    {
                        tokens.Add(source.Substring(startPosCalculated, endPos - (startIndex + StartIndexShift)));
                    }
                    else
                    {
                        //TODO: 2025-03-27 - JHA - Do bounding check
                        tokens.Add(source.Substring(startPosCalculated, SubStringCount));
                    }

                    string remainingContent = source.Substring(endPos + SubStringCount + StartIndexShift);

                    return GetToken(remainingContent, tokens);
                }
                catch( Exception ex) 
                {
                    Log.Error( source +" -> " +ex.Message, ex);
                    return new List<string>();
                }
            }

            return tokens;
        }
    }
}
