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
            functionObject.PathToSubstitute = ParadoxTranslationHelperConfig.PathEnglish;

            return functionObject;
        }

        public static IFunctionObject? CreateSubstituteAnalyse()
        {
            FunctionObjectSubstitute functionObject = new FunctionObjectSubstitute(Constants.FUNCTION_SUB_ANALYSE);
            functionObject.Description = "substitute translation file in folder analysis (MissingTranslationKeysSteam)";
            functionObject.PathToSubstitute = ParadoxTranslationHelperConfig.PathResult;
            functionObject.SubstituteAgainstSteam = true;

            return functionObject;
        }

        public static IFunctionObject? CreateReSubstitute()
        {
            FunctionObjectReSubstitute functionObject = new FunctionObjectReSubstitute(Constants.FUNCTION_RESUB);
            functionObject.Description = "resubstitute translation file";
            functionObject.PathToReSubstitute = ParadoxTranslationHelperConfig.PathGerman;
            functionObject.PathToReSubstituteCorresponding = ParadoxTranslationHelperConfig.PathEnglish;

            return functionObject;
        }

        public static IFunctionObject? CreateReSubstituteAnalyse()
        {
            FunctionObjectReSubstitute functionObject = new FunctionObjectReSubstitute(Constants.FUNCTION_RESUB_ANALYSE);
            functionObject.Description = "resubstitute translation file in folder analysis (MissingTranslationKeysSteam)";
            functionObject.PathToReSubstitute = ParadoxTranslationHelperConfig.PathResult;
            functionObject.PathToReSubstituteCorresponding = ParadoxTranslationHelperConfig.PathResult;
            functionObject.ReadOnlyLocalizationFilesSub = true;
            functionObject.RemoveFileExtension = true;

            return functionObject;
        }

        public static IFunctionObject? CreateInsertIntoLocalizationFiles()
        {
            FunctionObjectInsertIntoLocalizationFiles functionObject = new FunctionObjectInsertIntoLocalizationFiles(Constants.FUNCTION_INSERT);
            functionObject.Description = "resubstitute translation file in folder analysis (MissingTranslationKeysSteam)";

            return functionObject;
        }

    }
}
