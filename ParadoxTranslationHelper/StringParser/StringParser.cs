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
        public override List<string> GetToken(string source)
        {
            List<string> tokens = new List<string>();
            if (string.IsNullOrEmpty(source))
            {
                return tokens;
            }

            if (false == source.Contains(StartTag))
            {
                return tokens;
            }

            if (EndTags.Any() == false)
            {
                string subString = source;
                while (subString.Contains(StartTag))
                {
                    int startIndex = subString.IndexOf(StartTag, 0);
                    tokens.Add(subString.Substring(startIndex, SubStringCount));
                    subString = subString.Substring(startIndex + SubStringCount + StartIndexShift, subString.Length - (startIndex + SubStringCount + StartIndexShift));
                }
                return tokens;
            }
            else
            {
                int startIndex = source.IndexOf(StartTag, 0);
                foreach (string endTag in EndTags)
                {
                    try
                    {
                        string subString = source.Substring(startIndex + SubStringCount + StartIndexShift, source.Length - (startIndex + SubStringCount + StartIndexShift));

                        if (false == subString.Contains(endTag))
                        {
                            continue;
                        }

                        int startPosCalculated = startIndex + StartIndexShift;
                        if (startPosCalculated < 0)
                        {
                            startPosCalculated = startIndex;
                        }

                        int endPos = source.IndexOf(endTag, HasEndTag ? startIndex + StartTag.Length : startPosCalculated + subString.Length);
                        if (endPos == -1)
                        {
                            endPos = startPosCalculated + StartTag.Length;
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
                    catch (Exception ex)
                    {
                        Log.Error(source + " -> " + ex.Message, ex);
                        return new List<string>();
                    }
                }
            }
            return tokens;
        }

        protected override List<string> GetToken(string source, List<string> tokens)
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

            if (EndTags.Any() == false)
            {
                string subString = source;
                while (subString.Contains(StartTag))
                {
                    int startIndex = subString.IndexOf(StartTag, 0);
                    tokens.Add(subString.Substring(startIndex, SubStringCount));
                    subString = subString.Substring(startIndex + SubStringCount + StartIndexShift, subString.Length - (startIndex + SubStringCount + StartIndexShift));
                }
                return tokens;
            }
            else
            {
                int startIndex = source.IndexOf(StartTag, 0);
                foreach (string endTag in EndTags)
                {
                    try
                    {
                        string subString = source.Substring(startIndex + SubStringCount + StartIndexShift, source.Length - (startIndex + SubStringCount + StartIndexShift));

                        if (false == subString.Contains(endTag))
                        {
                            continue;
                        }

                        int startPosCalculated = startIndex + StartIndexShift;
                        if (startPosCalculated < 0)
                        {
                            startPosCalculated = startIndex;
                        }

                        int endPos = source.IndexOf(endTag, HasEndTag ? startIndex + StartTag.Length : startPosCalculated + subString.Length);
                        if (endPos == -1)
                        {
                            endPos = startPosCalculated + StartTag.Length;
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
                    catch (Exception ex)
                    {
                        Log.Error(source + " -> " + ex.Message, ex);
                        return new List<string>();
                    }
                }
            }
            return tokens;
        }
    }

}
