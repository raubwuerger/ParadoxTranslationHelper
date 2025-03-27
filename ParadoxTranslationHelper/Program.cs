using ParadoxTranslationHelper.FunctionObject;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace ParadoxTranslationHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            InitLogger();
            ReadConfig();

            if (args.Length < 2)
            {
                LogInfosMods("Too few arguments passed ...");
                return;
            }

            if ( false == SetActiveMod(args[0]))
            {
                LogInfosMods("No mod selected ...");
                return;
            }

            FunctionObjectRegistryInitialiser.Init();

            IFunctionObject functionObject = FunctionObjectRegistry.Instance.GetFunctionObject(args[1].ToUpper());
            if( functionObject == null ) 
            {
                LogInfosMods("Function not found ...");
                return;
            }
            functionObject.DoWork();
        }

        private static void LogInfosMods(string text)
        {
            Log.Information(text + Environment.NewLine);
            Log.Information("args[0] == mod name");
            Log.Information("args[1] == function");
            Log.Information(Environment.NewLine);
            Log.Information("Registered functions");
            Log.Information(FunctionTypes.SteamDiff);
            Log.Information(FunctionTypes.SteamRemove);
            Log.Information(FunctionTypes.SteamSub);
            Log.Information(FunctionTypes.SteamResub);
            Log.Information(FunctionTypes.SteamInsert);
            Log.Information(FunctionTypes.CheckForDoubleKeys);
            Log.Information(FunctionTypes.CheckForDoubleKeysAllFiles);
            Log.Information(FunctionTypes.CheckForDoubleKeysAllFilesFix);
            Log.Information("");

            Log.Information(Environment.NewLine);
            Log.Information("Known mods (ParadoxTranslationHelper.xml): ");
            foreach (DataSetMod dataSetMod in ModSelector.ModList)
            {
                Log.Information( dataSetMod.Name);
            }
            Log.Information(Environment.NewLine);
        }

        private static bool ReadConfig()
        {
            ConfigReader configReader = new ConfigReader();
            if (false == configReader.Read())
            {
                return false;
            }

            return true;
        }

        private static bool SetActiveMod(string modName)
        {
            ModSelector modSelector = new ModSelector();
            if( false == modSelector.SelectMod(modName.Trim()) )
            {
                return false;
            }

            return true;
        }

        private static void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                            // add console as logging target
                            .WriteTo.Console()
                            // add a logging target for warnings and higher severity  logs
                            // structured in JSON format
                            .WriteTo.File(new JsonFormatter(),
                                          "important.json",
                                          restrictedToMinimumLevel: LogEventLevel.Warning)
                            // add a rolling file for all logs
                            .WriteTo.File("all-.logs",
                                          rollingInterval: RollingInterval.Year)
                            // set default minimum level
                            .MinimumLevel.Debug()
                            .CreateLogger();
        }

    }
}
