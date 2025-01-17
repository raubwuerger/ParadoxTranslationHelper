using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class Constants
    {
        public static string CONFIG = @".\ParadoxTranslationHelper.xml";
        public static string CONFIG_NODE_PATH = "Paths";
        public static string CONFIG_NODE_PATH_ENGLISH = "english";
        public static string CONFIG_NODE_PATH_ENGLISH_UPDATED = "english_updated";
        public static string CONFIG_NODE_PATH_GERMAN = "german";

        public static string PATH_DEFAULT_ENGLISH = @"C:\Projects\HoI4-BlackICE_de\1137372539\localisation\english\";
        public static string PATH_DEFAULT_ENGLISH_UPDATED = @"C:\Projects\HoI4-BlackICE_de\1137372539\localisation\english_updated\";
        public static string PATH_DEFAULT_GERMAN = @"C:\Projects\HoI4-BlackICE_de\1137372539\localisation\german\";

        public static string LOCALISATION_ENGLISH_FULL = "_l_english";
        public static string LOCALISATION_GERMAN_FULL = "_l_german";

        public static string LOCALISATION_EXTENSION = ".yml";
        public static string LOCALISATION_START_STRING = "_l_";

    }
}
