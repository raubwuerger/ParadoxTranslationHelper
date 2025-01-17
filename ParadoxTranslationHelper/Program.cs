﻿using System;
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

            if( false == SetActiveMod(args[0]))
            {
                LogInfosMods("No mod selected ...");
                return;
            }

            AnalyseFunction(args[1]);

        }

        private static void LogInfosMods(string text)
        {
            Console.WriteLine(text + Environment.NewLine);
            Console.WriteLine("args[0] == mod name");
            Console.WriteLine("args[1] == function");
            Console.WriteLine("        => sub (substitute translation file)");
            Console.WriteLine("        => resub (resubstitute translation file)");
            Console.WriteLine("        => analyse (analyse translation file)");
            Console.WriteLine("Known mods (ParadoxTranslationHelper.xml): ");
            Console.WriteLine(Environment.NewLine);
            foreach (DataSetMod dataSetMod in ModSelector.ModList)
            {
                Console.WriteLine( dataSetMod.Name + Environment.NewLine);
            }
        }
        
        private static bool ReadConfig()
        {
            ConfigReader configReader = new ConfigReader();
            if (false == configReader.Read())
            {
                return false;
            }

            ModSelector.ModList = configReader.ModList;
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

        private static void AnalyseFunction(string text)
        { 
            if( text.Equals("sub",StringComparison.CurrentCultureIgnoreCase) )
            {
                TranslationFileAnalyser.SubstitueSourceFiles();
            }
            else if( text.Equals("resub", StringComparison.CurrentCultureIgnoreCase ) ) 
            {
                TranslationFileAnalyser.ReSubstitueSourceFiles();
            }
            else if( text.Equals("analyse", StringComparison.CurrentCultureIgnoreCase) )
            {
                TranslationFileAnalyser.Compare();
            }
        }

    }
}
