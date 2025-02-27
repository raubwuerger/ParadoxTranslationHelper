using ParadoxTranslationHelper.FunctionObject;
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
        public static IFunctionObject? CreateSteamDiff()
        {
            FunctionObjectDiffSteam functionObject = new FunctionObjectDiffSteam(FunctionTypes.SteamDiff);
            functionObject.Description = "write missing keys to file against steam path";
            functionObject.PathRepository = ParadoxTranslationHelperConfig.PathGerman;
            functionObject.PathSteam = ParadoxTranslationHelperConfig.PathSteam;
            functionObject.ResultFileName = Constants.FUNCTION_FILE_NAME_STEAM;

            return functionObject;
        }
        public static IFunctionObject? CreateSteamSubstitute()
        {
            FunctionObjectSubstitute functionObject = new FunctionObjectSubstitute(FunctionTypes.SteamSub);
            functionObject.Description = "substitute translation file in folder analysis (MissingTranslationKeysSteam)";
            functionObject.PathToSubstitute = ParadoxTranslationHelperConfig.PathResult;
            functionObject.SubstituteAgainstSteam = true;

            return functionObject;
        }

        public static IFunctionObject? CreateSteamResubstitute()
        {
            FunctionObjectReSubstitute functionObject = new FunctionObjectReSubstitute(FunctionTypes.SteamResub);
            functionObject.Description = "resubstitute translation file in folder analysis (MissingTranslationKeysSteam)";
            functionObject.PathToReSubstitute = ParadoxTranslationHelperConfig.PathResult;
            functionObject.PathToReSubstituteCorresponding = ParadoxTranslationHelperConfig.PathResult;
            functionObject.ReadOnlyLocalizationFilesSub = true;
            functionObject.RemoveFileExtension = true;

            return functionObject;
        }
        public static IFunctionObject? CreateSteamInsertDiff()
        {
            FunctionObjectInsertIntoLocalizationFiles functionObject = new FunctionObjectInsertIntoLocalizationFiles(FunctionTypes.SteamInsert);
            functionObject.Description = "resubstitute translation file in folder analysis (MissingTranslationKeysSteam)";
            functionObject.LocalizationFileNameDiff = Path.Combine(ParadoxTranslationHelperConfig.PathResult, "MissingTranslationKeysSteam.yml.sub.german.resub");
            functionObject.LocalizationFilePathGerman = ParadoxTranslationHelperConfig.PathGerman;
            functionObject.LocalizationFilePathAnalyze = ParadoxTranslationHelperConfig.PathResult;

            return functionObject;
        }

        public static IFunctionObject? CreateDiff()
        {
            FunctionObjectDiff functionObject = new FunctionObjectDiff(FunctionTypes.Diff);
            functionObject.Description = "write missing keys to file";

            return functionObject;
        }

        public static IFunctionObject? CreateSubstitute()
        {
            FunctionObjectSubstitute functionObject = new FunctionObjectSubstitute(FunctionTypes.Sub);
            functionObject.Description = "substitute translation file";
            functionObject.PathToSubstitute = ParadoxTranslationHelperConfig.PathEnglish;

            return functionObject;
        }

        public static IFunctionObject? CreateAnalyse()
        {
            FunctionObjectAnalyse functionObject = new FunctionObjectAnalyse(FunctionTypes.Analyse);
            functionObject.Description = "analyse translation file";
            functionObject.PathEnglish = ParadoxTranslationHelperConfig.PathEnglish;
            functionObject.PathSteam = ParadoxTranslationHelperConfig.PathSteam;
            functionObject.PathGerman = ParadoxTranslationHelperConfig.PathGerman;

            return functionObject;
        }


        public static IFunctionObject? CreateReSubstitute()
        {
            FunctionObjectReSubstitute functionObject = new FunctionObjectReSubstitute(FunctionTypes.Resub);
            functionObject.Description = "resubstitute translation file";
            functionObject.PathToReSubstitute = ParadoxTranslationHelperConfig.PathGerman;
            functionObject.PathToReSubstituteCorresponding = ParadoxTranslationHelperConfig.PathEnglish;

            return functionObject;
        }

        public static IFunctionObject? CreateCheckForDoubleKeys()
        {
            FunctionCheckForDoubleKeys functionObject = new FunctionCheckForDoubleKeys(FunctionTypes.CheckForDoubleKeys);
            functionObject.Description = "Compare for duplicate keys. File by file.";
            functionObject.PathGerman = ParadoxTranslationHelperConfig.PathGerman;
            functionObject.ResultFileNameAppendix = Constants.FUNCTION_FILE_NAME_APPENDIX;
            functionObject.PathAnalyze = ParadoxTranslationHelperConfig.PathResult;

            return functionObject;
        }

        public static IFunctionObject? CreateCheckForDoubleKeysAllFiles()
        {
            FunctionCheckForDoubleKeysAllFiles functionObject = new FunctionCheckForDoubleKeysAllFiles(FunctionTypes.CheckForDoubleKeysAllFiles);
            functionObject.Description = "Compare for duplicate keys, over all files.";
            functionObject.PathGerman = ParadoxTranslationHelperConfig.PathGerman;
            functionObject.PathAnalyze = ParadoxTranslationHelperConfig.PathResult;

            return functionObject;
        }

        public static IFunctionObject? CreateCheckForDoubleKeysAllFilesFix()
        {
            FunctionCheckForDoubleKeysAllFiles functionObject = new FunctionCheckForDoubleKeysAllFiles(FunctionTypes.CheckForDoubleKeysAllFilesFix);
            functionObject.Description = "Compare for duplicate keys, over all files.";
            functionObject.PathGerman = ParadoxTranslationHelperConfig.PathGerman;
            functionObject.PathAnalyze = ParadoxTranslationHelperConfig.PathResult;
            functionObject.DeleteDoubleKeys = true;

            return functionObject;
        }

    }
}
