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


        [TestMethod]
        [DataRow(DisplayName = "DIFF_FILES")]
        public void TestMethod_DIFF_FILES()
        {
            const string MOD_NAME = "Test_DIFF_FILES";
            const string MOD_FUNCTION = "DIFF_FILES";

            Assert.IsTrue(ReadConfig());
            Assert.IsTrue(SetActiveMod(MOD_NAME));

            FunctionObjectRegistryInitialiser.Init();

            IFunctionObject function = FunctionObjectRegistry.Instance.GetFunctionObject(MOD_FUNCTION);
            Assert.IsNotNull(function);
            Assert.IsTrue(function.Work());
            Assert.IsFalse(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathGerman, "file_no_more_in_steam.yml")));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathGerman, "file_no_more_in_steam.yml.toRemove")));
            Assert.IsTrue(Directory.Exists(ParadoxTranslationHelperConfig.PathGerman + @"//replace"));

        }

        [TestMethod]
        [DataRow(DisplayName = "SUB")]
        public void TestMethod_SUB()
        {
            const string MOD_NAME = "Test_SUB";
            const string MOD_FUNCTION = "SUB";

            Assert.IsTrue(ReadConfig());
            Assert.IsTrue(SetActiveMod(MOD_NAME));

            FunctionObjectRegistryInitialiser.Init();

            IFunctionObject function = FunctionObjectRegistry.Instance.GetFunctionObject(MOD_FUNCTION);
            Assert.IsNotNull(function);
            Assert.IsTrue(function.Work());
            Assert.IsTrue(Directory.Exists(ParadoxTranslationHelperConfig.PathResult));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, "_SteamKeysToCreate.yml.CC")));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, "_SteamKeysToCreate.yml.IC")));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, "_SteamKeysToCreate.yml.KY")));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, "_SteamKeysToCreate.yml.NE")));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, "_SteamKeysToCreate.yml.NS")));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, "_SteamKeysToCreate.yml.sub")));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, "_SteamKeysToCreate.yml.sub.german")));

            Assert.AreEqual(GetLineCount("_SteamKeysToCreate.yml.KY"), CountOccurences("_SteamKeysToCreate.yml.sub", "___KY"));
            Assert.AreEqual(GetLineCount("_SteamKeysToCreate.yml.NS"), CountOccurences("_SteamKeysToCreate.yml.sub", "___NS"));
        }

        private int GetLineCount( string fileName )
        {
            return File.ReadLines(Path.Combine(ParadoxTranslationHelperConfig.PathResult, fileName)).ToList<string>().Count;
        }

        private int CountOccurences(string fileName, string toCount )
        {
            string subContent = File.ReadAllText(Path.Combine(ParadoxTranslationHelperConfig.PathResult, fileName));
            return Regex.Matches(subContent, toCount).Count;
        }
    }
}
