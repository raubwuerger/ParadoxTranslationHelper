using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class HoI4_TranslationHelper_Config
    {
        private static string pathEnglish = Constants.PATH_DEFAULT_ENGLISH;
        private static string pathEnglishUpdated = Constants.PATH_DEFAULT_ENGLISH_UPDATED;
        private static string pathGerman = Constants.PATH_DEFAULT_GERMAN;

        public static string PathEnglish { get => pathEnglish; set => pathEnglish = value; }
        public static string PathGerman { get => pathGerman; set => pathGerman = value; }
        public static string PathEnglishUpdated { get => pathEnglishUpdated; set => pathEnglishUpdated = value; }
    }
}
