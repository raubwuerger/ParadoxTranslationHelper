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
            FunctionObjectDiffSteam functionObjectDiffSteam = new FunctionObjectDiffSteam(Constants.FUNCTION_DIFF_STEAM);

            functionObjectDiffSteam.LocalisationEnglishUpdated = AnalyseDirectory(ParadoxTranslationHelperConfig.PathSteam);
            if( null == functionObjectDiffSteam.LocalisationEnglishUpdated)
            {
                Console.WriteLine("Steam path not set!");
                return null;
            }

            functionObjectDiffSteam.LocalisationEnglish = AnalyseDirectory(ParadoxTranslationHelperConfig.PathEnglish);

            return functionObjectDiffSteam;
        }

        private static List<TranslationFile> AnalyseDirectory(string directory)
        {
            if (null == directory)
            {
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
