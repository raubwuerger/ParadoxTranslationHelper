using ParadoxTranslationHelper.FunctionObject;
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
            Console.WriteLine(text + Environment.NewLine);
            Console.WriteLine("args[0] == mod name");
            Console.WriteLine("args[1] == function");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Registered functions");
            Console.WriteLine(FunctionTypes.SteamDiff);
            Console.WriteLine(FunctionTypes.SteamRemove);
            Console.WriteLine(FunctionTypes.SteamSub);
            Console.WriteLine(FunctionTypes.SteamResub);
            Console.WriteLine(FunctionTypes.SteamInsert);
            Console.WriteLine(FunctionTypes.CheckForDoubleKeys);
            Console.WriteLine(FunctionTypes.CheckForDoubleKeysAllFiles);
            Console.WriteLine(FunctionTypes.CheckForDoubleKeysAllFilesFix);
            Console.WriteLine("");

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Known mods (ParadoxTranslationHelper.xml): ");
            foreach (DataSetMod dataSetMod in ModSelector.ModList)
            {
                Console.WriteLine( dataSetMod.Name);
            }
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

    }
}
