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
    public class LineCorrectorSubstitution_Test
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
        string file_SteamKeysToCreate_yml_NL = "_SteamKeysToCreate.yml.NL";
        string file_SteamKeysToCreate_yml_sub_german = "_SteamKeysToCreate.yml.sub.german";
        string file_SteamKeysToCreate_yml_sub_german_resub = "_SteamKeysToCreate.yml.sub.german.resub";


        [TestMethod]
        [DataRow(DisplayName = "LINE_CORRECTOR")]
        public void TestMethod_RESUB()
        {
            const string MOD_NAME = "Test_LINE_CORRECTOR_SUB";
            const string MOD_FUNCTION = FunctionTypes.LineCorrectorSubstitute;

            InitLogger();
            Assert.IsTrue(ReadConfig());
            Assert.IsTrue(SetActiveMod(MOD_NAME));

            //INFO: 2025-11-28 - JHA - Check if translated files are correct
            Assert.IsTrue(Directory.Exists(ParadoxTranslationHelperConfig.PathResult));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_KY)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_CC)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_IC)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_NE)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_NS)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_NL)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german)));

/* Das funktioniert nur wenn alle Keys korrekt übersetzt wurden
 * 
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_KY)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german), "___KY"));
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_CC)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german), "___CC"));
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_IC)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german), "___IC"));
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_NE)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german), "___NE"));
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_NS)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german), "___NS"));
            Assert.AreEqual(GetLineCount(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_NL)), CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german), "___NL"));
*/
            //INFO: 2025-11-28 - JHA - Do resubstitutiuon
            FunctionObjectRegistryInitialiser.Init();

            IFunctionObject function = FunctionObjectRegistry.Instance.GetFunctionObject(MOD_FUNCTION);
            Assert.IsNotNull(function);
            Assert.IsTrue(function.Work());


            //INFO: 2025-11-28 - JHA - Check if translated files are correct
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german)));
            Assert.IsTrue(new FileInfo(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german)).Length != 0);

            Assert.AreEqual(0, CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_resub), "___KY"));
            Assert.AreEqual(0, CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_resub), "___CC"));
            Assert.AreEqual(0, CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_resub), "___IC"));
            Assert.AreEqual(0, CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_resub), "___NE"));
            Assert.AreEqual(0, CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_resub), "___NS"));
            Assert.AreEqual(0, CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_resub), "___NL"));
            Assert.AreEqual(0, CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_resub), "___"));
        }

    }
}
