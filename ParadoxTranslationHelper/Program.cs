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
            functionObject.Work();
            Log.CloseAndFlush();
        }

        private static void LogInfosMods(string text)
        {
            Console.WriteLine(text + Environment.NewLine);
            Console.WriteLine("args[0] == mod name");
            Console.WriteLine("args[1] == function");

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Known mods (ParadoxTranslationHelper.xml): ");
            foreach (DataSetMod dataSetMod in ModSelector.ModList)
            {
                Console.WriteLine(dataSetMod.Name);
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Registered functions");
            Console.WriteLine("----- RECOMMENDED STEPS -----");
            Console.WriteLine(FunctionTypes.DiffFiles);
            Console.WriteLine(FunctionTypes.RemoveKeys);
            Console.WriteLine(FunctionTypes.Sub);
            Console.WriteLine(FunctionTypes.Resub);
            Console.WriteLine(FunctionTypes.Insert);
            Console.WriteLine("----- ADDITIONAL STEPS ------");
            Console.WriteLine(FunctionTypes.Analyse);
            Console.WriteLine(FunctionTypes.DiffKeys);
            Console.WriteLine(FunctionTypes.CheckForDoubleKeys);
            Console.WriteLine(FunctionTypes.CheckForDoubleKeysAllFiles);
            Console.WriteLine(FunctionTypes.CheckForDoubleKeysAllFilesFix);

            Console.WriteLine(Environment.NewLine);
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
                            .WriteTo.Console()
                            .WriteTo.File("./logs/ParadoxTranslationHelper.log", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}")
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

    }
}
