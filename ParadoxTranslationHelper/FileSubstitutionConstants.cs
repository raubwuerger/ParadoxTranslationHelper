using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    internal class FileSubstitutionConstants
    {
        public static string SUBSTITUTION_START = "___";
        public static string SUBSTITUTION_END = "___";

        public static string NESTING_STRING_SUFFIX = "NE";
        public static string NESTING_STRING_SIGN_START = StringParserFactory.NESTING_STRINGS_START;
        public static string NESTING_STRING_SIGN_END = StringParserFactory.NESTING_STRINGS_END;

        public static string COLOR_CODE_SUFFIX = "CC";
        public static string COLOR_CODE_SIGN_START = StringParserFactory.COLOR_CODE_START;
        public static string COLOR_CODE_SIGN_END = StringParserFactory.COLOR_CODE_END;

        public static string NAMESPACE_SUFFIX = "NS";
        public static string NAMESPACE_START_SIGN_START = StringParserFactory.NAMESPACE_START;
        public static string NAMESPACE_START_SIGN_END = StringParserFactory.NAMESPACE_END;

        public static string ICON_SUFFIX = "IC";
        public static string ICON_START_SIGN_START = StringParserFactory.ICON_START;
        public static string ICON_START_SIGN_END = StringParserFactory.ICON_END;

        public static string NEW_LINE_SUFFIX = "NL";
        public static string NEW_LINE_SIGN_START = StringParserFactory.NEW_LINE_START;
        public static string NEW_LINE_SIGN_END = StringParserFactory.NEW_LINE_END;
    }
}
