using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using ParadoxTranslationHelper.Comparator;
using ParadoxTranslationHelper.FunctionObject;
using ParadoxTranslationHelper.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper_Test
{
    /**
     * Dateiübersicht
     * steam\replace\equipment_l_english.yml -> german\replace\>muss angelegt werden<
     * steam\aat_focus_l_english.yml -> german\>muss angelegt werden<
     * steam\air_l_english.yml -> german\>es müssen 10 Zeilen entfernt werden<
     * steam\bba_focus_l_english.yml -> german\>identisch<
     * steam\bftb_decisions_l_english.yml -> >10 Zeilen zu viel, 10 Zeilen zu wenig<
     * steam\lar_events_l_english.yml -> german\>Es fehlen 20 Zeilen<
     */

    [TestClass]
    public class Sub_Test
    {
        void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                            .WriteTo.Console()
                            .WriteTo.File("./logs/ParadoxTranslationHelper.log", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}")
                            .MinimumLevel.Debug()
                            .CreateLogger();
        }

        bool ReadConfig()
        {
            ConfigReader configReader = new ConfigReader();
            if (false == configReader.Read())
            {
                return false;
            }

            return true;
        }

        bool SetActiveMod(string modName)
        {
            ModSelector modSelector = new ModSelector();
            if (false == modSelector.SelectMod(modName.Trim()))
            {
                return false;
            }

            return true;
        }
        private int GetLineCount(string fileName)
        {
            return File.ReadLines(fileName).ToList<string>().Count;
        }

        private int CountOccurences(string fileName, string toCount)
        {
            string subContent = File.ReadAllText(fileName);
            MatchCollection matchCollection = Regex.Matches(subContent, toCount);
            
            List<Match> matches = matchCollection.ToList();
            foreach( Match match in matches )
            {
                string value = match.Value;
            }

            return matchCollection.Count;
        }

        string file_SteamKeysToCreate_yml_KY = "_SteamKeysToCreate.yml.KY";
        string file_SteamKeysToCreate_yml_CC = "_SteamKeysToCreate.yml.CC";
        string file_SteamKeysToCreate_yml_IC = "_SteamKeysToCreate.yml.IC";
        string file_SteamKeysToCreate_yml_NE = "_SteamKeysToCreate.yml.NE";
        string file_SteamKeysToCreate_yml_NS = "_SteamKeysToCreate.yml.NS";
        string file_SteamKeysToCreate_yml_sub = "_SteamKeysToCreate.yml.sub";
        string file_SteamKeysToCreate_yml_sub_german = "_SteamKeysToCreate.yml.sub.german";

        [TestMethod]
        [DataRow(DisplayName = "SUB")]
        public void TestMethod_SUB()
        {
            const string MOD_NAME = "Test_SUB";
            const string MOD_FUNCTION = "SUB";

            InitLogger();
            Assert.IsTrue(ReadConfig());
            Assert.IsTrue(SetActiveMod(MOD_NAME));

            FunctionObjectRegistryInitialiser.Init();

            PrepareTest_SUB();

            IFunctionObject function = FunctionObjectRegistry.Instance.GetFunctionObject(MOD_FUNCTION);
            Assert.IsNotNull(function);
            Assert.IsTrue(function.Work());
            Assert.IsTrue(Directory.Exists(ParadoxTranslationHelperConfig.PathResult));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_KY)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_CC)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_IC)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_NE)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_NS)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german)));

            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult,file_SteamKeysToCreate_yml_KY)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub), "___KY"));
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult,file_SteamKeysToCreate_yml_CC)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub), "___CC"));
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult,file_SteamKeysToCreate_yml_IC)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub), "___IC"));
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult,file_SteamKeysToCreate_yml_NE)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub), "___NE"));
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult,file_SteamKeysToCreate_yml_NS)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub), "___NS"));
        }

        void PrepareTest_SUB()
        {
            Assert.IsTrue(Directory.Exists(ParadoxTranslationHelperConfig.PathResult));
            File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_KY));
            File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_CC));
            File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_IC));
            File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_NE));
            File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_NS));
            File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub));
            File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german));
        }
    }
}
