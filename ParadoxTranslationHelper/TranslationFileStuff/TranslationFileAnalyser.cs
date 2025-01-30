using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    internal class TranslationFileAnalyser
    {
        static string FILE_PATTERN = "*.yml";

        static List<TranslationFile> _localisationEnglish = null;
        static List<TranslationFile> _localisationGerman = null;

        public static void ReSubstitueSourceFiles()
        {
            Analyse();
            foreach (TranslationFile translationFile in _localisationGerman)
            {
                FileSubstitutor fileSubstitutor = new FileSubstitutor();
                fileSubstitutor.ReSubstitute(Create(translationFile, FindCorrespondingTranslationFile(translationFile)));
            }
        }

        private static string FindCorrespondingTranslationFile( TranslationFile translationFile )
        {
            TranslationFile corresponding = _localisationEnglish.Find( x => x.FileNameWithoutLocalisation.Equals( translationFile.FileNameWithoutLocalisation ) );
            if( corresponding == null ) 
            { 
                return null;
            }

            return Utility.ReplaceWithAnalyseDirectory(corresponding, ParadoxTranslationHelperConfig.PathResult);
        }

        public static void SubstitueSourceFiles()
        {
            Analyse();

            foreach (TranslationFile translationFile in _localisationEnglish)
            {
                FileSubstitutor fileSubstitutor = new FileSubstitutor();
                fileSubstitutor.Substitute(translationFile);
            }
        }

        private static void Analyse()
        {
            _localisationEnglish = AnalyseDirectory(ParadoxTranslationHelperConfig.PathEnglish);
            _localisationGerman = AnalyseDirectory(ParadoxTranslationHelperConfig.PathGerman);
        }

        private static TranslationFileSetSubstitution Create(TranslationFile substitutedFile, string pathToSubstiteFile)
        {
            TranslationFileSetSubstitution translationFileSetSubstitution = new TranslationFileSetSubstitution();

            translationFileSetSubstitution.SubstitutedFile = substitutedFile;
            translationFileSetSubstitution.PathNestingStringsFile = pathToSubstiteFile +"." +FileSubstitutionConstants.NESTING_STRING_SUFFIX;
            translationFileSetSubstitution.PathNamespaceFile = pathToSubstiteFile + "." + FileSubstitutionConstants.NAMESPACE_SUFFIX;
            translationFileSetSubstitution.PathIconFile = pathToSubstiteFile + "." + FileSubstitutionConstants.ICON_SUFFIX;

            return translationFileSetSubstitution;
        }

        private static List<TranslationFile> AnalyseDirectory( string directory )
        {
            if( null  == directory )
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
