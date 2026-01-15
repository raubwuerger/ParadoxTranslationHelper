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
    [TestClass]
    public class Insert_Test
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
        string file_SteamKeysToCreate_yml_sub_german = "_SteamKeysToCreate.yml.sub.german";
        string file_SteamKeysToCreate_yml_sub_german_corrected = "_SteamKeysToCreate.yml.sub.german.corrected";


        [TestMethod]
        [DataRow(DisplayName = "INSERT")]
        public void TestMethod_INSERT()
        {
            const string MOD_NAME = "Test_INSERT";
            const string MOD_FUNCTION = FunctionTypes.Insert;

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
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german)));
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_corrected)));

            FunctionObjectRegistryInitialiser.Init();

            IFunctionObject function = FunctionObjectRegistry.Instance.GetFunctionObject(MOD_FUNCTION);
            Assert.IsNotNull(function);
            Assert.IsTrue(function.Work());


            //INFO: 2025-11-28 - JHA - Check if translated files are correct
            Assert.IsTrue(File.Exists(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_corrected)));
            Assert.IsTrue(new FileInfo(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_corrected)).Length != 0);

            Assert.AreEqual(0, CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_corrected), "___KY"));
            Assert.AreEqual(0, CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_corrected), "___CC"));
            Assert.AreEqual(0, CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_corrected), "___IC"));
            Assert.AreEqual(0, CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_corrected), "___NE"));
            Assert.AreEqual(0, CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_corrected), "___NS"));
            Assert.AreEqual(0, CountOccurences(Path.Combine(ParadoxTranslationHelperConfig.PathResult, file_SteamKeysToCreate_yml_sub_german_corrected), "___"));
        }

    }
}
