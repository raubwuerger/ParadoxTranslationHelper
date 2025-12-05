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
    public class DiffFiles_Test
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

        string file_no_more_in_steam_yml_toRemove = "file_no_more_in_steam.yml.toRemove";
        string file_no_more_in_steam_yml = "file_no_more_in_steam.yml";
        string file_equipment_l_german = "equipment_l_german.yml";
        string file_aat_focus_l_german = "aat_focus_l_german.yml";
        string file_buildings_l_german = "buildings_l_german.yml";
        string file_air_l_german = "air_l_german.yml";
        string file_SteamKeysToCreate = "_SteamKeysToCreate.yml";
        string file_SteamKeysToDelete = "_SteamKeysToDelete.yml";
        string path_replace = @"//replace";

        [TestMethod]
        [DataRow(DisplayName = "DIFF_FILES")]
        public void TestMethod_DIFF_FILES()
        {
            const string MOD_NAME = "Test_DIFF_FILES";
            const string MOD_FUNCTION = "DIFF_FILES";

            InitLogger();
            Assert.IsTrue(ReadConfig());
            Assert.IsTrue(SetActiveMod(MOD_NAME));

            FunctionObjectRegistryInitialiser.Init();

            PrepareTest_DIFF_FILES();

            IFunctionObject function = FunctionObjectRegistry.Instance.GetFunctionObject(MOD_FUNCTION);
            Assert.IsNotNull(function);
            Assert.IsTrue(function.Work());
            Assert.IsTrue(Directory.Exists(ParadoxTranslationHelperConfig.PathGerman + path_replace));
            Assert.IsTrue(Directory.Exists(ParadoxTranslationHelperConfig.PathResult));
            Assert.IsFalse(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathGerman, file_no_more_in_steam_yml)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathGerman, file_no_more_in_steam_yml_toRemove)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathGerman + path_replace, file_equipment_l_german)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathGerman, file_aat_focus_l_german)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathGerman, file_buildings_l_german)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToDelete)));
        }

        void PrepareTest_DIFF_FILES()
        {
            File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathGerman, file_no_more_in_steam_yml_toRemove));
            File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathGerman, file_no_more_in_steam_yml));
            if ( true == Directory.Exists(ParadoxTranslationHelperConfig.PathGerman + path_replace) )
            {
                File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathGerman + path_replace, file_equipment_l_german));
                Directory.Delete(ParadoxTranslationHelperConfig.PathGerman + path_replace);
            }
            File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathGerman, file_aat_focus_l_german));
            File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathGerman, file_buildings_l_german));

            File.Copy(Path.Combine(ParadoxTranslationHelperConfig.PathGerman, file_air_l_german), Path.Combine(ParadoxTranslationHelperConfig.PathGerman, file_no_more_in_steam_yml));

            if( true == Directory.Exists(ParadoxTranslationHelperConfig.PathResult ) )
            {
                File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate));
                File.Delete(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToDelete));
                Directory.Delete(ParadoxTranslationHelperConfig.PathResult);
            }
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

            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult,file_SteamKeysToCreate_yml_KY)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_KY), "___KY"));
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult,file_SteamKeysToCreate_yml_CC)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_CC), "___CC"));
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult,file_SteamKeysToCreate_yml_IC)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_IC), "___IC"));
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult,file_SteamKeysToCreate_yml_NE)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_NE), "___NE"));
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult,file_SteamKeysToCreate_yml_NS)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_NS), "___NS"));
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

        [TestMethod]
        [DataRow(DisplayName = "RESUB")]
        public void TestMethod_RESUB()
        {
            const string MOD_NAME = "Test_RESUB";
            const string MOD_FUNCTION = "RESUB";

            InitLogger();
            Assert.IsTrue(ReadConfig());
            Assert.IsTrue(SetActiveMod(MOD_NAME));

            //INFO: 2025-11-28 - JHA - Check if translated files are correct
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, "_SteamKeysToCreate.yml.sub.german")));

//            Assert.AreEqual(GetLineCount("_SteamKeysToCreate.yml.KY"), CountOccurences("_SteamKeysToCreate.yml.sub.german", "___KY"));
//            Assert.AreEqual(GetLineCount("_SteamKeysToCreate.yml.NS"), CountOccurences("_SteamKeysToCreate.yml.sub.german", "___NS"));
//            Assert.AreEqual(GetLineCount("_SteamKeysToCreate.yml.NE"), CountOccurences("_SteamKeysToCreate.yml.sub.german", "___NE"));

            //INFO: 2025-11-28 - JHA - Do resubstitutiuon
            FunctionObjectRegistryInitialiser.Init();

            IFunctionObject function = FunctionObjectRegistry.Instance.GetFunctionObject(MOD_FUNCTION);
            Assert.IsNotNull(function);
            Assert.IsTrue(function.Work());


            //INFO: 2025-11-28 - JHA - Check if translated files are correct
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, "_SteamKeysToCreate.yml.sub.german.resub")));

            Assert.AreEqual(0, CountOccurences("_SteamKeysToCreate.yml.sub_1.german.resub", "___KY"));
            Assert.AreEqual(0, CountOccurences("_SteamKeysToCreate.yml.sub_1.german.resub", "___NS"));
            Assert.AreEqual(0, CountOccurences("_SteamKeysToCreate.yml.sub_1.german.resub", "___NE"));
        }

    }
}
