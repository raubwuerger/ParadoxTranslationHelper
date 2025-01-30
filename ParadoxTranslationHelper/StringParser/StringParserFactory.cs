using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class StringParserFactory
    {
        private static StringParserFactory instance;
        public static StringParserFactory Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new StringParserFactory();
                }
                return instance;
            }
        }

        private StringParserFactory() { }

        public static string NESTING_STRINGS_START = "$";
        public static string NESTING_STRINGS_END = "$";

        public static string COLOR_CODE_START = "§";
        public static string COLOR_CODE_END = "§!";

        public static string NAMESPACE_START = "[";
        public static string NAMESPACE_END = "]";

        public static string ICON_START = "£";
        public static string ICON_END = "£";

        public static string NEW_LINE_START = "\\";
        public static string NEW_LINE_END = "n";

        public IStringParser CreateParserNamespaces()
        {
            StringParser stringParser = new StringParser();
            stringParser.StartTag = NAMESPACE_START;
            stringParser.EndTags.Clear();
            stringParser.EndTags.Add(NAMESPACE_END);
            IgnoreCommentLines(stringParser);
            return stringParser;
        }

        public StringParser CreateParserIcons()
        {
            StringParser stringParser = new StringParser();
            stringParser.StartTag = ICON_START;
            stringParser.EndTags.Add(ICON_END);
            stringParser.EndTags.Add(" ");
            stringParser.EndTags.Add("\n");
            stringParser.EndTags.Add("\"");
            IgnoreCommentLines(stringParser);
            return stringParser;
        }

        public IStringParser CreateParserNestingStrings()
        {
            StringParser stringParser = new StringParser();
            stringParser.StartTag = NESTING_STRINGS_START;
            stringParser.EndTags.Add(NESTING_STRINGS_END);
            IgnoreCommentLines(stringParser);
            return stringParser;
        }

        public IStringParser CreateParserColorCodes()
        {
            StringParser stringParser = new StringParser();
            stringParser.StartTag = COLOR_CODE_START;
            stringParser.EndTags.Add(COLOR_CODE_END);
            IgnoreCommentLines(stringParser);
            stringParser.SubStringCount = 1;
            return stringParser;
        }

        public IStringParser CreateParserNewLine()
        {
            StringParser stringParser = new StringParser();
            stringParser.StartTag = NEW_LINE_START;
            stringParser.EndTags.Add(NEW_LINE_END);
            stringParser.SubStringCount = 2;
            stringParser.StartIndexShift = -1;
            IgnoreCommentLines(stringParser);
            return stringParser;
        }

        public IStringParser CreateParserInnerDoubleQuotes()
        {
            StringParserFirstLast stringParser = new StringParserFirstLast();
            stringParser.StartTag = "\"";
            IgnoreCommentLines(stringParser);
            stringParser.EndTags.Add("\"");
            return stringParser;
        }
        public IStringParser CreateParserKey()
        {
            StringParserKey stringParser = new StringParserKey();
            stringParser.StartTag = "\"";
            IgnoreCommentLines(stringParser);
            return stringParser;
        }

        private void IgnoreCommentLines(StringParserBase stringParserBase)
        {
            stringParserBase.LineIgnores.Add("#");
            stringParserBase.LineIgnores.Add(" #");
            stringParserBase.LineIgnores.Add("  #");
        }

    }
}
