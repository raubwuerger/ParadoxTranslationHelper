﻿using ParadoxTranslationHelper.FunctionObject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectFactory
    {
        public static IFunctionObject? CreateDiffSteam()
        {
            FunctionObjectDiffSteam functionObject = new FunctionObjectDiffSteam(Constants.FUNCTION_DIFF_STEAM);
            functionObject.Description = "write missing keys to file against steam path";
            functionObject.ResultFileName = Constants.FUNCTION_FILE_NAME_STEAM;

            return functionObject;
        }

        public static IFunctionObject? CreateDiff()
        {
            FunctionObjectDiff functionObject = new FunctionObjectDiff(Constants.FUNCTION_DIFF);
            functionObject.Description = "write missing keys to file";

            return functionObject;
        }

        public static IFunctionObject? CreateAnalyse()
        {
            FunctionObjectAnalyse functionObject = new FunctionObjectAnalyse(Constants.FUNCTION_ANALYSIS);
            functionObject.Description = "analyse translation file";

            return functionObject;
        }

        public static IFunctionObject? CreateSubstitute()
        {
            FunctionObjectSubstitute functionObject = new FunctionObjectSubstitute(Constants.FUNCTION_SUB);
            functionObject.Description = "substitute translation file";

            return functionObject;
        }

        public static IFunctionObject? CreateSubstituteAnalyse()
        {
            FunctionObjectSubstituteAnalyse functionObject = new FunctionObjectSubstituteAnalyse(Constants.FUNCTION_SUB_ANALYSE);
            functionObject.Description = "substitute translation file in folder analysis (MissingTranslationKeysSteam)";
            functionObject.LocalisationEnglish = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathResult);

            return functionObject;
        }

        public static IFunctionObject? CreateReSubstitute()
        {
            FunctionObjectReSubstitute functionObject = new FunctionObjectReSubstitute(Constants.FUNCTION_RESUB);
            functionObject.Description = "resubstitute translation file";

            return functionObject;
        }

        public static IFunctionObject? CreateReSubstituteAnalyse()
        {
            FunctionObjectReSubstituteAnalyse functionObject = new FunctionObjectReSubstituteAnalyse(Constants.FUNCTION_RESUB_ANALYSE);
            functionObject.Description = "resubstitute translation file in folder analysis (MissingTranslationKeysSteam)";
            functionObject.LocalisationEnglish = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathResult);
            functionObject.LocalisationGerman = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathResult, Constants.FILE_EXTENSION_PREFIX +Constants.FUNCTION_SUB);

            string fileName = Path.GetFileNameWithoutExtension("FileWithNoExtension");

            return functionObject;
        }

    }
}
