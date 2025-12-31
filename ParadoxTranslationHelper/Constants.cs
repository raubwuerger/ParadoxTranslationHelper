using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public static class Constants
    {
        public const string CONFIG = @".\ParadoxTranslationHelper.xml";

        public const string LOCALISATION_ENGLISH_FULL = "_l_english";
        public const string LOCALISATION_ENGLISH_FULL_I = "_i_english";
        public const string LOCALISATION_ENGLISH_FILE_IDENTIFIER = "l_english:";
        public const string LOCALISATION_ENGLISH = "english";

        public const string LOCALISATION_GERMAN_FULL = "_l_german";
        public const string LOCALISATION_GERMAN_FULL_I = "_i_german";
        public const string LOCALISATION_GERMAN_FILE_IDENTIFIER = "l_german:";
        public const string LOCALISATION_GERMAN = "german";

        public const string LOCALISATION_EXTENSION = ".yml";
        public const string LOCALISATION_START_STRING = "_l_";

        public const string FILE_NAME_STEAM_TO_DELETE_KEYS = "_SteamKeysToDelete.yml";
        //TODO: 2025-03-01 - JHA - Remove hack!!!
        public const string FILE_NAME_STEAM_TO_DELETE_KEYS_WITHOUT_EXTENSION = "_SteamKeysToDelete";
        public const string FILE_NAME_STEAM_MISSING_KEYS = "_SteamKeysToCreate.yml";
        public const string FUNCTION_FILE_NAME_APPENDIX = ".DoubleKey.txt";

        public const string FILE_EXTENSION_PREFIX = "*.";
        private const string FILE_EXTENSION_SUB = "sub";

        public const string TRANSLATION_FILE_IDENTIFIER = ">>>>> ";

        public const string FILE_BACKUP_EXTENSION = ".bak";
        public const string SIGN_HASH_TAG = "#";
        public const string QUOTATION_MARKS = "\"";
        public const char QUOTATION_MARKS_CHAR = '"';

        public const string SIGN_TABULATOR = "\t";
        public const string SIGN_NEW_LINE = "\n";
    }
}
