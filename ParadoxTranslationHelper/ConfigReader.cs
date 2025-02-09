using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ParadoxTranslationHelper
{
    public class ConfigReader
    {
        public const string CONFIG_NODE_PATH_ENGLISH = "english";
        //public const string CONFIG_NODE_PATH_ENGLISH_UPDATED = "english_updated";
        public const string CONFIG_NODE_PATH_GERMAN = "german";
        public const string CONFIG_NODE_PATH_BASE = "basePath";
        public const string CONFIG_NODE_PATH_STEAM = "steam";

        public const string CONFIG_MODS = "/ParadoxTranslationHelper/Mods";
        public const string CONFIG_MOD = "/Mod";
        public const string CONFIG_MOD_NAME_ATTRIBUTE = "name";

        public const string CONFIG_GLOBAL = "/ParadoxTranslationHelper/Global";
        public const string CONFIG_GLOBAL_RESULT_PATH = "ResultPath";

        public const string CONFIG_FUNCTION = "/ParadoxTranslationHelper/Function";


        public bool Read()
        {
            XmlDocument config = XMLFileUtility.Load(Constants.CONFIG);
            if (null == config)
            {
                Console.WriteLine("Unable to load config: " + Constants.CONFIG + Environment.NewLine);
                return false;
            }

            ReadGlobal(config);
            ReadMods(config);

            return true;
        }



        private void ReadGlobal(XmlDocument config)
        {
            XmlNodeList global = config.SelectNodes(CONFIG_GLOBAL);
            foreach (XmlNode mod in global)
            {
                string resultPath = Utility.FindNodeByName(mod.ChildNodes, CONFIG_GLOBAL_RESULT_PATH);
                if (null == resultPath)
                {
                    continue;
                }

                ParadoxTranslationHelperConfig.PathResult = resultPath;
                return;
            }
        }

        private void ReadMods(XmlDocument config)
        {
            XmlNodeList modPaths = config.SelectNodes(CONFIG_MODS + CONFIG_MOD);

            foreach (XmlNode mod in modPaths)
            {
                string modName = Utility.GetAttributeValueByName(mod.Attributes, CONFIG_MOD_NAME_ATTRIBUTE);
                if (null == modName)
                {
                    continue;
                }

                DataSetMod dataSetMod = new DataSetMod(modName);

                if( false == SetItem(Utility.FindNodeByName(mod.ChildNodes, CONFIG_NODE_PATH_BASE), value => dataSetMod.PathBase = value) )
                {
                    continue;
                }

                if (false == SetItem(Utility.FindNodeByName(mod.ChildNodes, CONFIG_NODE_PATH_ENGLISH), value => dataSetMod.PathEnglish = value))
                {
                    continue;
                }

                if (false == SetItem(Utility.FindNodeByName(mod.ChildNodes, CONFIG_NODE_PATH_GERMAN), value => dataSetMod.PathGerman = value))
                {
                    continue;
                }

                if (false == SetItem(Utility.FindNodeByName(mod.ChildNodes, CONFIG_NODE_PATH_STEAM), value => dataSetMod.PathSteam = value))
                {
                    continue;
                }

                dataSetMod.PathResult = ParadoxTranslationHelperConfig.PathResult;

                ModSelector.ModList.Add(dataSetMod);
            }
        }

        private bool SetItem( string foundItem, Action<string> itemToSet )
        {
            if (foundItem == null)
            { 
                return false;
            }

            itemToSet(foundItem);

            return true;
        }
    }
}
