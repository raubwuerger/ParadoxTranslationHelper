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
        public static string FILE_PATTERN = "*.yml";
        public static IFunctionObject? CreateDiffSteam()
        {
            FunctionObjectDiff functionObject = new FunctionObjectDiff(Constants.FUNCTION_DIFF_STEAM);
            functionObject.Description = "Writes missing keys against steam path to file";

            functionObject.LocalisationEnglishUpdated = AnalyseDirectory(ParadoxTranslationHelperConfig.PathSteam);
            if( null == functionObject.LocalisationEnglishUpdated)
            {
                Console.WriteLine("Steam path not set!");
                return null;
            }

            functionObject.LocalisationEnglish = AnalyseDirectory(ParadoxTranslationHelperConfig.PathEnglish);
            functionObject.ResultFileName = "MissingTranslationKeysSteam.yml"; ;

            return functionObject;
        }

        public static IFunctionObject? CreateDiff()
        {
            FunctionObjectDiff functionObject = new FunctionObjectDiff(Constants.FUNCTION_DIFF);
            functionObject.Description = "write missing keys to file";

            functionObject.LocalisationEnglish = AnalyseDirectory(ParadoxTranslationHelperConfig.PathEnglish);
            functionObject.LocalisationEnglishUpdated = AnalyseDirectory(ParadoxTranslationHelperConfig.PathEnglishUpdated);
            functionObject.ResultFileName = "MissingTranslationKeys.yml"; ;

            return functionObject;
        }

        public static IFunctionObject? CreateAnalyse()
        {
            FunctionObjectAnalyse functionObject = new FunctionObjectAnalyse(Constants.FUNCTION_ANALYSIS);
            functionObject.Description = "Analyze translation files";

            functionObject.LocalisationEnglish = AnalyseDirectory(ParadoxTranslationHelperConfig.PathEnglish);
            functionObject.LocalisationEnglishUpdated = AnalyseDirectory(ParadoxTranslationHelperConfig.PathEnglishUpdated);
            functionObject.LocalisationGerman = AnalyseDirectory(ParadoxTranslationHelperConfig.PathGerman);

            return functionObject;
        }

        public static IFunctionObject? CreateSubstitute()
        {
            FunctionObjectSubstitute functionObject = new FunctionObjectSubstitute(Constants.FUNCTION_SUB);
            functionObject.Description = "Substitute translation file";

            functionObject.LocalisationEnglish = AnalyseDirectory(ParadoxTranslationHelperConfig.PathEnglish);

            return functionObject;
        }

        public static IFunctionObject? CreateReSubstitute()
        {
            FunctionObjectReSubstitute functionObject = new FunctionObjectReSubstitute(Constants.FUNCTION_RESUB);
            functionObject.Description = "resubstitute translation file";

            functionObject.LocalisationEnglish = AnalyseDirectory(ParadoxTranslationHelperConfig.PathEnglish);
            functionObject.LocalisationGerman = AnalyseDirectory(ParadoxTranslationHelperConfig.PathGerman);

            return functionObject;
        }

        private static List<TranslationFile> AnalyseDirectory(string directory)
        {
            if (null == directory)
            {
                return null;
            }

            if( false == Directory.Exists(directory)) 
            {
                Console.WriteLine("Directory doesn't exist: " + directory);
                return null;
            }

            string[] files = Directory.GetFiles(directory, FILE_PATTERN, SearchOption.AllDirectories);
            if (files.Length <= 0)
            {
                return null;
            }

            List<TranslationFile> translationFiles = new List<TranslationFile>();
            TranslationFileCreator translationFileCreator = new TranslationFileCreator();

            foreach (string file in files)
            {
                translationFiles.Add(translationFileCreator.Create(file));
            }

            return translationFiles;
        }

    }
}
