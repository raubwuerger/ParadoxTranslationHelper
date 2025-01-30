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

        public static IFunctionObject? CreateReSubstitute()
        {
            FunctionObjectReSubstitute functionObject = new FunctionObjectReSubstitute(Constants.FUNCTION_RESUB);
            functionObject.Description = "resubstitute translation file";

            return functionObject;
        }
    }
}
