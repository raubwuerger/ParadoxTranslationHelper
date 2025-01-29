using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine("ModSelector not initialized");
                return false;
            }

            DataSetMod found = _modList.Find( i => i.Name == modName );
            if( found == null ) 
            {
                Console.WriteLine("Mod not found: " + modName);
                return false;
            }
            Console.WriteLine("Mod found: " + modName);

            ParadoxTranslationHelperConfig.PathEnglish = Path.Combine(found.PathBase, found.PathEnglish);
            ParadoxTranslationHelperConfig.PathEnglishUpdated = Path.Combine(found.PathBase, found.PathEnglishUpdated);
            ParadoxTranslationHelperConfig.PathGerman = Path.Combine(found.PathBase, found.PathGerman);
            ParadoxTranslationHelperConfig.PathBase = found.PathBase;
            ParadoxTranslationHelperConfig.PathResult = Path.Combine(found.PathBase, found.PathResult);
            ParadoxTranslationHelperConfig.PathCompare = found.PathCompare;

            return true;
        }

    }
}
