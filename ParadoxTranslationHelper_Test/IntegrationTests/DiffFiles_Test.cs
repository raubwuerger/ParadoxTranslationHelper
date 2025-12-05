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
    }
}
