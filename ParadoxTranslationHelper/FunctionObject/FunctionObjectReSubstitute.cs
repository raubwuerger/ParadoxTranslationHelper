using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectReSubstitute : FunctionObjectBase
    {
        string _pathToReSubstitute;
        string _pathToReSubstituteCorresponding;

        bool _removeFileExtension = false;
        bool _readOnlyLocalizationFilesSub = false;

        public string PathToReSubstitute { get => _pathToReSubstitute; set => _pathToReSubstitute = value; }
        public string PathToReSubstituteCorresponding { get => _pathToReSubstituteCorresponding; set => _pathToReSubstituteCorresponding = value; }
        public bool RemoveFileExtension { get => _removeFileExtension; set => _removeFileExtension = value; }
        public bool ReadOnlyLocalizationFilesSub { get => _readOnlyLocalizationFilesSub; set => _readOnlyLocalizationFilesSub = value; }

        public FunctionObjectReSubstitute(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            if (_pathToReSubstitute == null) 
            {
                Console.WriteLine("Member <PathToReSubstitute> must not be null!");
                return false;
            }

            if (_pathToReSubstituteCorresponding == null)
            {
                Console.WriteLine("Member <PathToReSubstituteCorresponding> must not be null!");
                return false;
            }

            if ( ReadOnlyLocalizationFilesSub )
            {
                LocalisationGerman = FileUtility.CreateTranslationFilesFromDirectory(_pathToReSubstitute, Constants.FILE_EXTENSION_PREFIX + Constants.LOCALISATION_GERMAN);
            }
            else
            {
                LocalisationGerman = FileUtility.CreateTranslationFilesFromDirectory(_pathToReSubstitute);
            }

            LocalisationEnglish = FileUtility.CreateTranslationFilesFromDirectory(_pathToReSubstituteCorresponding);

            foreach (TranslationFile translationFile in LocalisationGerman)
            {
                FileSubstitutor fileSubstitutor = new FileSubstitutor();
                if( true == RemoveFileExtension )
                {
                    translationFile.FileNameWithoutLocalisation = Utility.RemoveAllFileExtensions(translationFile.FileNameWithoutLocalisation);
                    TranslationFileCreator creator = new TranslationFileCreator();
                    fileSubstitutor.ReSubstitute(Create(creator.CopyExceptFileName(translationFile.FileName, translationFile), FindCorrespondingTranslationFile(translationFile)));
                }
                else
                {
                    fileSubstitutor.ReSubstitute(Create(translationFile, FindCorrespondingTranslationFile(translationFile)));
                }
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

            return Utility.ReplaceWithAnalyseDirectory(corresponding);
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
