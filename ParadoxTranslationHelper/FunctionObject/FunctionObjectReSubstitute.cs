using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectReSubstitute : FunctionObjectBase
    {
        public FunctionObjectReSubstitute(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            LocalisationEnglish = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathEnglish);
            LocalisationGerman = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathGerman);

            foreach (TranslationFile translationFile in LocalisationGerman)
            {
                FileSubstitutor fileSubstitutor = new FileSubstitutor();
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
