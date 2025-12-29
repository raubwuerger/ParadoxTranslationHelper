using ParadoxTranslationHelper.Utilities;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectLineCorrector : FunctionObjectBase
    {
        string _pathToReSubstitute;
        string _translationFileNameDiff;
        string _translationFileNameSub;
        string _translationFileNameResub;

        public string PathToReSubstitute { get => _pathToReSubstitute; set => _pathToReSubstitute = value; }
        public string TranslationFileNameDiff { get => _translationFileNameDiff; set => _translationFileNameDiff = value; }
        public string TranslationFileNameResub { get => _translationFileNameResub; set => _translationFileNameResub = value; }
        public string TranslationFileNameSub { get => _translationFileNameSub; set => _translationFileNameSub = value; }

        public FunctionObjectLineCorrector(string name) : base(name)
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

            Task task = Task.Run(() => CorrectAnalyse());
            task.Wait();
            return true;
        }

        private async Task<bool> CorrectAnalyse()
        {
            TranslationFile translationFile = FileUtility.CreateTranslationFileFromFileResub(_translationFileNameSub);
            if (translationFile == null)
            {
                Log.Warning("Translation file resub not found! " + _translationFileNameSub);
                return false;
            }

            //TODO: 2025-12-29 - JHA - Neue Klasse CorrectAnalyse erstellen
            //- Prüft ob alle Keys vorhanden sind
            //- Versucht falsche Keys zu korriegieren |___ ___|
            //- Korrigiert falsche Textanfänge '„' und Textende '“'
            FileSubstitutor fileSubstitutor = new FileSubstitutor();
            translationFile.FileNameWithoutLocalisation = Utility.RemoveAllFileExtensions(translationFile.FileNameWithoutLocalisation);

            fileSubstitutor.ReSubstitute(CreateTranslationFileSetSubstitution(translationFile, Path.Combine(_pathToReSubstitute, _translationFileNameDiff)));
            return true;
        }
        private TranslationFileSetSubstitution CreateTranslationFileSetSubstitution(TranslationFile substitutedFile, string pathToSubstitedFileParts)
        {
            TranslationFileSetSubstitution translationFileSetSubstitution = new TranslationFileSetSubstitution();

            translationFileSetSubstitution.SubstitutedFile = substitutedFile;
            translationFileSetSubstitution.PathKeyFile = $"{pathToSubstitedFileParts}.{FileSubstitutionConstants.KEY_SUFFIX}";
            translationFileSetSubstitution.PathNestingStringsFile = $"{pathToSubstitedFileParts}.{FileSubstitutionConstants.NESTING_STRING_SUFFIX}";
            translationFileSetSubstitution.PathNamespaceFile = $"{pathToSubstitedFileParts}.{FileSubstitutionConstants.NAMESPACE_SUFFIX}";
            translationFileSetSubstitution.PathIconFile = $"{pathToSubstitedFileParts}.{FileSubstitutionConstants.ICON_SUFFIX}";
            translationFileSetSubstitution.PathColorFile = $"{pathToSubstitedFileParts}.{FileSubstitutionConstants.COLOR_CODE_SUFFIX}";
            translationFileSetSubstitution.PathNewLineFile = $"{pathToSubstitedFileParts}.{FileSubstitutionConstants.NEW_LINE_SUFFIX}";

            return translationFileSetSubstitution;
        }
    }
}
