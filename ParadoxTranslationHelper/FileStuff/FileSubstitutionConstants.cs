using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    internal class FileSubstitutionConstants
    {
        public static string SUBSTITUTION_START = "|___";
        public static string SUBSTITUTION_END = "___|";

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

        public static string KEY_SUFFIX = "KY";
        public static string KEY_END_SIGN = "\"";

        public static string NEW_LINE_SUFFIX = "NL";
        public static string NEW_LINE = "\\n";

        public static string TABULATOR_SUFFIX = "TAB";
        public static string TABULATOR = "\\t";

        public static string FILE_SUFFIX_SUBSTITUTED = ".sub";
        public static string FILE_SUFFIX_CORRECTED = ".corrected";

        public static string FILE_SUFFIX_GERMAN = ".german";

        public static string NOT_FOUND = ".notFound";

    }
}
