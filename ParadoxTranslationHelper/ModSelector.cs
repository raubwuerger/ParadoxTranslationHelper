using System;
using System.Collections.Generic;
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

            HoI4_TranslationHelper_Config.PathEnglish = found.PathEnglish;
            HoI4_TranslationHelper_Config.PathEnglishUpdated = found.PathEnglishUpdated;
            HoI4_TranslationHelper_Config.PathGerman = found.PathGerman;

            return true;
        }

    }
}
