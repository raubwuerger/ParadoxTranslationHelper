using ParadoxTranslationHelper.LineObjects;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.SubResubstitution
{
    internal class SubstitutionFileReaderFactory
    {
        public static SubstitutionFileReader Create( string suffix )
        {
            SubstitutionFileReader substitutionFileReader = new SubstitutionFileReader();
            substitutionFileReader.BasePath = ParadoxTranslationHelperConfig.PathResult + Constants.FILE_NAME_STEAM_MISSING_KEYS;
            substitutionFileReader.SubstituteFile = "." +suffix;

            return substitutionFileReader;
        }
    }
}
