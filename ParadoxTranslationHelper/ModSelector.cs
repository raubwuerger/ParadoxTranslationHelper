using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;

namespace ParadoxTranslationHelper
{
    internal class ModSelector
    {
        private static List<DataSetMod> _modList = new List<DataSetMod>();

        internal static List<DataSetMod> ModList { get => _modList; set => _modList = value; }

        public bool SelectMod( string modName )
        {
            if( _modList.Any() == false )
            {
                Log.Warning($"ModSelector not initialized");
                return false;
            }

            DataSetMod found = _modList.Find( i => i.Name == modName );
            if( found == null ) 
            {
                Log.Information($"Mod not found: {modName}");
                return false;
            }
            Log.Information($"Analyzing Mod: {modName}");

            ParadoxTranslationHelperConfig.PathEnglish = Path.Combine(found.PathBase, found.PathEnglish);
            ParadoxTranslationHelperConfig.PathGerman = Path.Combine(found.PathBase, found.PathGerman);
            ParadoxTranslationHelperConfig.PathBase = found.PathBase;
            ParadoxTranslationHelperConfig.PathResult = Path.Combine(found.PathBase, found.PathResult);
            ParadoxTranslationHelperConfig.PathSteam = found.PathSteam;

            return true;
        }

    }
}
