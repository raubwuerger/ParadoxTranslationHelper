using ParadoxTranslationHelper.Utilities;
using Serilog;
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
                Log.Verbose("Member <PathToReSubstitute> must not be null!");
                return false;
            }

            if (_translationFileNameDiff == null)
            {
                Log.Verbose("Member <TranslationFileNameSteamDiff> must not be null!");
                return false;
            }

            if (_translationFileNameResub == null)
            {
                Log.Verbose("Member <TranslationFileNameResub> must not be null!");
                return false;
            }

            if (_translationFileNameSub == null)
            {
                Log.Verbose("Member <TranslationFileNameSub> must not be null!");
                return false;
            }


            for( int part=1;part<=4;part++)
            {
                Log.Information($"Prozessing part {part}");
                string partSuffixSubstitutedFile = $"_{part}";

                TranslationFile translationFile = FileUtility.CreateTranslationFileFromFile(_translationFileNameSub.Insert(_translationFileNameSub.IndexOf(".german"), partSuffixSubstitutedFile));
                if (translationFile == null)
                {
                    Log.Warning("Translation file resub not found! " + _translationFileNameSub);
                    return false;
                }

                FileSubstitutor fileSubstitutor = new FileSubstitutor();
                translationFile.FileNameWithoutLocalisation = Utility.RemoveAllFileExtensions(translationFile.FileNameWithoutLocalisation);

                fileSubstitutor.ReSubstitute(CreateTranslationFileSetSubstitution(translationFile, Path.Combine(_pathToReSubstitute, _translationFileNameDiff), part));
            }

            return true;
        }

        private TranslationFileSetSubstitution CreateTranslationFileSetSubstitution(TranslationFile substitutedFile, string pathToSubstitedFileParts, int part)
        {
            TranslationFileSetSubstitution translationFileSetSubstitution = new TranslationFileSetSubstitution();

            string partSuffix = $"_{part}.";
            translationFileSetSubstitution.SubstitutedFile = substitutedFile;
            translationFileSetSubstitution.PathKeyFile = $"{pathToSubstitedFileParts}{partSuffix}{FileSubstitutionConstants.KEY_SUFFIX}";
            translationFileSetSubstitution.PathNestingStringsFile = $"{pathToSubstitedFileParts}{partSuffix}{FileSubstitutionConstants.NESTING_STRING_SUFFIX}";
            translationFileSetSubstitution.PathNamespaceFile = $"{pathToSubstitedFileParts}{partSuffix}{FileSubstitutionConstants.NAMESPACE_SUFFIX}";
            translationFileSetSubstitution.PathIconFile = $"{pathToSubstitedFileParts}{partSuffix}{FileSubstitutionConstants.ICON_SUFFIX}";

            return translationFileSetSubstitution;
        }

    }
}
