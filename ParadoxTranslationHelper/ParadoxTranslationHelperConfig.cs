using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public static class ParadoxTranslationHelperConfig
    {
        private static string _pathResult = "";
        private static string _pathBase = "";
        private static string _pathEnglish = "";
        private static string _pathEnglishUpdated = "";
        private static string _pathGerman = "";
        private static string _pathCompare = "";

        public static string PathEnglish { get => _pathEnglish; set => _pathEnglish = value; }
        public static string PathGerman { get => _pathGerman; set => _pathGerman = value; }
        public static string PathEnglishUpdated { get => _pathEnglishUpdated; set => _pathEnglishUpdated = value; }
        public static string PathBase { get => _pathBase; set => _pathBase = value; }
        public static string PathResult { get => _pathResult; set => _pathResult = value; }
        public static string PathCompare { get => _pathCompare; set => _pathCompare = value; }
    }
}
