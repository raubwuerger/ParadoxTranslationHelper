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
        public const string CONFIG_BASEPATH = "/ParadoxTranslationHelper/Paths";
        public const string CONFIG_MOD = "/Mod";
        public const string CONFIG_FUNCTION = "/ParadoxTranslationHelper/Function";
        private List<DataSetMod> _modList = new List<DataSetMod>();
        public bool Read()
        {
            _modList.Clear();
            XmlDocument config = XMLFileUtility.Load(Constants.CONFIG);
            if (null == config)
            {
                Console.WriteLine("Unable to load config: " + Constants.CONFIG + Environment.NewLine);
                return false;
            }
            
            XmlNodeList configPaths = config.SelectNodes(CONFIG_BASEPATH + CONFIG_MOD);

            foreach (XmlNode configPath in configPaths) 
            {
                string modName = Utility.GetAttributeValueByName(configPath.Attributes, "name");
                if (null == modName )
                {
                    continue;
                }

                DataSetMod dataSetMod = new DataSetMod(modName);
                string pathEnglish = Utility.FindNodeByName(configPath.ChildNodes, Constants.CONFIG_NODE_PATH_ENGLISH);
                if (null != pathEnglish)
                {
                    dataSetMod.PathEnglish = pathEnglish;
                }

                string pathEnglishUpdated = Utility.FindNodeByName(configPath.ChildNodes, Constants.CONFIG_NODE_PATH_ENGLISH_UPDATED);
                if (null != pathEnglishUpdated)
                {
                    dataSetMod.PathEnglishUpdated = pathEnglishUpdated;
                }

                string pathGerman = Utility.FindNodeByName(configPath.ChildNodes, Constants.CONFIG_NODE_PATH_GERMAN);
                if (null != pathGerman)
                {
                    dataSetMod.PathGerman = pathGerman;
                }

                _modList.Add(dataSetMod);
            }

            return true;
        }

        internal List<DataSetMod> ModList { get => _modList; }
    }
}
