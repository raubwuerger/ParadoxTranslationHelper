using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class Serilog_Test
    {
        public static void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("./logs/ParadoxTranslationHelper.log",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}")
                .WriteTo.Map("KeysToDelete",
                    (name, wt) => wt.File($"./logs/KeysToDelete.log"),
                    sinkMapCountLimit: 10)
                .WriteTo.Map("KeysToCreate",
                    (name, wt) => wt.File($"./logs/KeysToCreate.log"),
                    sinkMapCountLimit: 10)
                .WriteTo.Map("KeysInWrongFile",
                    (name, wt) => wt.File($"./logs/KeysInWrongFile.log"),
                    sinkMapCountLimit: 10)
                .MinimumLevel.Debug()
                .CreateLogger();
        }

        [TestMethod]
        [DataRow(DisplayName = "Serilog Map: 0 --> 0")]
        public void SerilogMap_000()
        {
            InitLogger();
            const string MAP_LOG_KEYSTODELETE = "{KeysToDelete}";

            Log.Information("Just a plain log message!");
            Log.Information("{name} Just a plain log message!", "map name wrong");
            Log.Information("{Name} Just a plain log message!", "map name ok");
            Log.Information("{Name} Just a plain log message!", "MapLog", "map name ok");
            Log.Information("{NotInMap} Just a plain log message! {secondInfoNotInMap}", "MapLog", "map name ok");
            Log.Information("{KeyNotFound}: Key not found {Key}!", "KeyNotFound", "news.3.d:0 \"Äthiopien ist italienisch\"");

            Log.Information("{KeysToDelete}: Key not found {Key}!", "KeysToDelete_variable", "news.1.d:0 \"Äthiopien ist italienisch\"");
            Log.Information("{KeysToCreate}: Key not found {Key}!", "KeysToCreate_variable", "news.2.d:0 \"Äthiopien ist italienisch\"");
            Log.Information("{KeysWrongFile}: Key not found {Key}!", "KeysWrongFile_variable", "news.3.d:0 \"Äthiopien ist italienisch\"");

            Log.Information("{KeysToDelete} --> [ABS_l_german.yml][VOX_l_german.yml]", "news.1.d:0 \"Äthiopien ist italienisch\"");
            Log.Information("{KeysToCreate} --> [ABS_l_german.yml][SSW_l_german.yml]", "news.2.d:0 \"Äthiopien ist italienisch\"");
            Log.Information("{KeysInWrongFile} --> [TTBBS_l_german.yml][VOX_l_german.yml]", "news.3.d:0 \"Äthiopien ist italienisch\"");

            Log.Information(MAP_LOG_KEYSTODELETE + "{Key} --> variable Key", "news.1.d:0 \"Äthiopien ist italienisch\" (key)");
            Log.Information(MAP_LOG_KEYSTODELETE + "{KeysToDelete} --> variable KeysToDelete", "news.1.d:0 \"Äthiopien ist italienisch\" (KeysToDelete)");

            Log.Information(LoggerConstants.MAP_KEYS_TO_DELETE + "{Key} --> [ABS_l_german.yml][VOX_l_german.yml]", "news.10.d:0 \"Äthiopien ist italienisch\"");
            Log.Information(LoggerConstants.MAP_KEYS_TO_CREATE + "{Key} --> [ABS_l_german.yml][SSW_l_german.yml]", "news.20.d:0 \"Äthiopien ist italienisch\"");
            Log.Information(LoggerConstants.MAP_KEYS_IN_WRONG_FILE + "{Key} --> [TTBBS_l_german.yml][VOX_l_german.yml]", "news.30.d:0 \"Äthiopien ist italienisch\"");

            Log.CloseAndFlush();

            Assert.IsTrue(true);
        }

        [TestMethod]
        [DataRow(DisplayName = "Serilog Map: LoggerConstants --> 0")]
        public void SerilogMap_001()
        {
            InitLogger();

            const string MAP_LOG_KEYSTODELETE = "{KeysToDelete}";

            Log.Information(MAP_LOG_KEYSTODELETE + "{Key} --> variable Key", "news.1.d:0 \"Äthiopien ist italienisch\" (key)");
            Log.Information(MAP_LOG_KEYSTODELETE + "{KeysToDelete} --> variable KeysToDelete", "news.1.d:0 \"Äthiopien ist italienisch\" (KeysToDelete)");

            Log.Information(LoggerConstants.MAP_KEYS_TO_CREATE + " --> [ABS_l_german.yml][SSW_l_german.yml]", "news.20.d:0 \"Äthiopien ist italienisch\"");
            Log.Information(LoggerConstants.MAP_KEYS_TO_DELETE + " --> [ABS_l_german.yml][VOX_l_german.yml]", "news.10.d:0 \"Äthiopien ist italienisch\"");
            Log.Information(LoggerConstants.MAP_KEYS_IN_WRONG_FILE + " --> [TTBBS_l_german.yml][VOX_l_german.yml]", "news.30.d:0 \"Äthiopien ist italienisch\"");

            Log.CloseAndFlush();

            Assert.IsTrue(true);
        }
    }
}
