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
        public static IFunctionObject? CreateDiffFiles()
        {
            FunctionObjectDiffFiles functionObject = new FunctionObjectDiffFiles(FunctionTypes.DiffFiles);
            functionObject.Description = "write missing keys to file against steam path";

            functionObject.PathGerman = ParadoxTranslationHelperConfig.PathGerman;
            functionObject.PathSteam = ParadoxTranslationHelperConfig.PathSteam;
            functionObject.FileNameMissingKeys = Constants.FILE_NAME_STEAM_MISSING_KEYS;
            functionObject.FileNameKeysToDelete = Constants.FILE_NAME_STEAM_TO_DELETE_KEYS;

            return functionObject;
        }
        public static IFunctionObject? CreateRemoveKeys()
        {
            FunctionObjectRemoveKeys functionObject = new FunctionObjectRemoveKeys(FunctionTypes.RemoveKeys);
            functionObject.Description = "Deletes keys no longer available";

            functionObject.LocalizationFileNameKeysToDelete = Path.Combine(ParadoxTranslationHelperConfig.PathResult, Constants.FILE_NAME_STEAM_TO_DELETE_KEYS);
            functionObject.LocalizationFilePathGerman = ParadoxTranslationHelperConfig.PathGerman;
            functionObject.LocalizationFilePathAnalyze = ParadoxTranslationHelperConfig.PathResult;

            return functionObject;
        }
        public static IFunctionObject? CreateInsertKeys()
        {
            FunctionObjectInsertKeys functionObject = new FunctionObjectInsertKeys(FunctionTypes.Insert);
            functionObject.Description = "resubstitute translation file in folder analysis (MissingTranslationKeysSteam)";

            functionObject.LocalizationFileNameKeysToCreate = Path.Combine(ParadoxTranslationHelperConfig.PathResult, CreateFileNameDiffResubGerman());
            functionObject.LocalizationFilePathGerman = ParadoxTranslationHelperConfig.PathGerman;
            functionObject.LocalizationFilePathAnalyze = ParadoxTranslationHelperConfig.PathResult;

            return functionObject;
        }

        public static IFunctionObject? CreateSteamSubstitute()
        {
            FunctionObjectSubstitute functionObject = new FunctionObjectSubstitute(FunctionTypes.Sub);
            functionObject.Description = "substitute translation file in folder analysis (MissingTranslationKeysSteam)";

            functionObject.TranslationFileToIgnore = Constants.FILE_NAME_STEAM_TO_DELETE_KEYS_WITHOUT_EXTENSION;
            functionObject.PathToSubstitute = ParadoxTranslationHelperConfig.PathResult;
            functionObject.SubstituteAgainstSteam = true;

            return functionObject;
        }

        public static IFunctionObject? CreateSteamResubstitute()
        {
            FunctionObjectReSubstitute functionObject = new FunctionObjectReSubstitute(FunctionTypes.Resub);
            functionObject.Description = "resubstitute translation file in folder analysis (MissingTranslationKeysSteam)";

            functionObject.PathToReSubstitute = ParadoxTranslationHelperConfig.PathResult;
            functionObject.TranslationFileNameDiff = Constants.FILE_NAME_STEAM_MISSING_KEYS;
            functionObject.TranslationFileNameSub = Path.Combine(ParadoxTranslationHelperConfig.PathResult, CreateFileNameDiffSubGerman());
            functionObject.TranslationFileNameResub = Path.Combine(ParadoxTranslationHelperConfig.PathResult, CreateFileNameDiffResubGerman() );

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

        public static IFunctionObject? CreateKeysDiff()
        {
            FunctionObjectDiffKeys functionObject = new FunctionObjectDiffKeys(FunctionTypes.DiffKeys);
            functionObject.Description = "Diffs keys content (colors, icons, nested strings, namespaces ) file by file";

            functionObject.LocalizationFilePathGerman = ParadoxTranslationHelperConfig.PathGerman;
            functionObject.LocalizationFilePathSteam = ParadoxTranslationHelperConfig.PathSteam;

            return functionObject;
        }

        public static IFunctionObject? CreateAnalyse()
        {
            FunctionObjectAnalyse functionObject = new FunctionObjectAnalyse(FunctionTypes.Analyse);
            functionObject.Description = "Diff steam directory against local german directory";

            functionObject.PathGerman = ParadoxTranslationHelperConfig.PathGerman;
            functionObject.PathSteam = ParadoxTranslationHelperConfig.PathSteam;

            return functionObject;
        }

        private static string CreateFileNameDiffResubGerman()
        {
            return CreateFileNameDiffSubGerman() + FileSubstitutionConstants.FILE_SUFFIX_RESUBSTITUTED;
        }

        private static string CreateFileNameDiffSubGerman()
        {
            return CreateFileNameDiffSub() + FileSubstitutionConstants.FILE_SUFFIX_GERMAN;
        }

        private static string CreateFileNameDiffSub()
        {
            return Constants.FILE_NAME_STEAM_MISSING_KEYS + FileSubstitutionConstants.FILE_SUFFIX_SUBSTITUTED;
        }
    }
}
