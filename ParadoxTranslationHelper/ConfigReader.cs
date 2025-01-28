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

                ParadoxTranslationHelperConfig.AnalysisPathAppendix = resultPath;
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
                string pathEnglish = Utility.FindNodeByName(mod.ChildNodes, Constants.CONFIG_NODE_PATH_ENGLISH);
                if (null != pathEnglish)
                {
                    dataSetMod.PathEnglish = pathEnglish;
                }

                string pathEnglishUpdated = Utility.FindNodeByName(mod.ChildNodes, Constants.CONFIG_NODE_PATH_ENGLISH_UPDATED);
                if (null != pathEnglishUpdated)
                {
                    dataSetMod.PathEnglishUpdated = pathEnglishUpdated;
                }

                string pathGerman = Utility.FindNodeByName(mod.ChildNodes, Constants.CONFIG_NODE_PATH_GERMAN);
                if (null != pathGerman)
                {
                    dataSetMod.PathGerman = pathGerman;
                }

                ModSelector.ModList.Add(dataSetMod);
            }
        }
    }
}
