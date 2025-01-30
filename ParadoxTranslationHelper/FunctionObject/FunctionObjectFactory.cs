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
            FunctionObjectDiffSteam functionObjectDiffSteam = new FunctionObjectDiffSteam(Constants.FUNCTION_DIFF_STEAM);

            return functionObjectDiffSteam;
        }

        public static IFunctionObject? CreateDiff()
        {
            FunctionObjectDiff functionObjectDiff = new FunctionObjectDiff(Constants.FUNCTION_DIFF);

            return functionObjectDiff;
        }

        public static IFunctionObject? CreateAnalyse()
        {
            FunctionObjectAnalyse functionObjectAnalyse = new FunctionObjectAnalyse(Constants.FUNCTION_ANALYSIS);

            return functionObjectAnalyse;
        }

        public static IFunctionObject? CreateSubstitute()
        {
            FunctionObjectSubstitute functionObjectSubstitute = new FunctionObjectSubstitute(Constants.FUNCTION_SUB);

            return functionObjectSubstitute;
        }

        public static IFunctionObject? CreateReSubstitute()
        {
            FunctionObjectReSubstitute functionObjectReSubstitute = new FunctionObjectReSubstitute(Constants.FUNCTION_RESUB);

            return functionObjectReSubstitute;
        }
    }
}
