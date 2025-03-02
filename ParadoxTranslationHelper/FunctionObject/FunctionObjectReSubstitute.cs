using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectReSubstitute : FunctionObjectBase
    {
        string _pathToReSubstitute;
        string _translationFileNameDiff;
        string _translationFileNameSub;
        string _translationFileNameResub;

        public string PathToReSubstitute { get => _pathToReSubstitute; set => _pathToReSubstitute = value; }
        public string TranslationFileNameDiff { get => _translationFileNameDiff; set => _translationFileNameDiff = value; }
        public string TranslationFileNameResub { get => _translationFileNameResub; set => _translationFileNameResub = value; }
        public string TranslationFileNameSub { get => _translationFileNameSub; set => _translationFileNameSub = value; }

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

            if (_translationFileNameDiff == null)
            {
                Console.WriteLine("Member <TranslationFileNameSteamDiff> must not be null!");
                return false;
            }

            if (_translationFileNameResub == null)
            {
                Console.WriteLine("Member <TranslationFileNameResub> must not be null!");
                return false;
            }

            if (_translationFileNameSub == null)
            {
                Console.WriteLine("Member <TranslationFileNameSub> must not be null!");
                return false;
            }

            TranslationFile translationFile = FileUtility.CreateTranslationFileFromFile(_translationFileNameSub);
            if(translationFile == null) 
            {
                Console.WriteLine("Translation file zu resub not found! " + _translationFileNameSub);
                return false;
            }

            FileSubstitutor fileSubstitutor = new FileSubstitutor();
            translationFile.FileNameWithoutLocalisation = Utility.RemoveAllFileExtensions(translationFile.FileNameWithoutLocalisation);
            fileSubstitutor.ReSubstitute(CreateTranslationFileSetSubstitution(translationFile, Path.Combine(_pathToReSubstitute, _translationFileNameDiff)));

            return true;
        }

        private TranslationFileSetSubstitution CreateTranslationFileSetSubstitution(TranslationFile substitutedFile, string pathToSubstitedFileParts)
        {
            TranslationFileSetSubstitution translationFileSetSubstitution = new TranslationFileSetSubstitution();

            translationFileSetSubstitution.SubstitutedFile = substitutedFile;
            translationFileSetSubstitution.PathNestingStringsFile = pathToSubstitedFileParts + "." + FileSubstitutionConstants.NESTING_STRING_SUFFIX;
            translationFileSetSubstitution.PathNamespaceFile = pathToSubstitedFileParts + "." + FileSubstitutionConstants.NAMESPACE_SUFFIX;
            translationFileSetSubstitution.PathIconFile = pathToSubstitedFileParts + "." + FileSubstitutionConstants.ICON_SUFFIX;

            return translationFileSetSubstitution;
        }

    }
}
