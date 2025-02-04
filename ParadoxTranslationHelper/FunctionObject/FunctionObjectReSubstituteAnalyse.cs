using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectReSubstituteAnalyse : FunctionObjectBase
    {
        public FunctionObjectReSubstituteAnalyse(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            LocalisationEnglish = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathResult);
            LocalisationGerman = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathResult, Constants.FILE_EXTENSION_PREFIX + Constants.FUNCTION_SUB);

            if ( null == LocalisationGerman )
            {
                Console.WriteLine("Member <LocalisationGerman> must not be null!");
                return false;
            }

            if (null == LocalisationEnglish)
            {
                Console.WriteLine("Member <LocalisationEnglish> must not be null!");
                return false;
            }

            foreach (TranslationFile translationFile in LocalisationGerman)
            {
                FileSubstitutor fileSubstitutor = new FileSubstitutor();
                translationFile.FileNameWithoutLocalisation = Utility.RemoveAllFileExtensions( translationFile.FileNameWithoutLocalisation );
                fileSubstitutor.ReSubstitute(Create(translationFile, FindCorrespondingTranslationFile(translationFile)));
            }

            return true;
        }

        private string FindCorrespondingTranslationFile(TranslationFile translationFile)
        {
            TranslationFile corresponding = LocalisationEnglish.Find(x => x.FileNameWithoutLocalisation.Equals(translationFile.FileNameWithoutLocalisation));
            if (corresponding == null)
            {
                return null;
            }

            return Utility.ReplaceWithAnalyseDirectory(corresponding, ParadoxTranslationHelperConfig.PathResult);
        }

        private TranslationFileSetSubstitution Create(TranslationFile substitutedFile, string pathToSubstiteFile)
        {
            TranslationFileSetSubstitution translationFileSetSubstitution = new TranslationFileSetSubstitution();

            translationFileSetSubstitution.SubstitutedFile = substitutedFile;
            translationFileSetSubstitution.PathNestingStringsFile = pathToSubstiteFile + "." + FileSubstitutionConstants.NESTING_STRING_SUFFIX;
            translationFileSetSubstitution.PathNamespaceFile = pathToSubstiteFile + "." + FileSubstitutionConstants.NAMESPACE_SUFFIX;
            translationFileSetSubstitution.PathIconFile = pathToSubstiteFile + "." + FileSubstitutionConstants.ICON_SUFFIX;

            return translationFileSetSubstitution;
        }
    }
}
