using ParadoxTranslationHelper.Utilities;
using Serilog;
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

            Task task = Task.Run(() => DoResubstitutionPart());
//            Task task = Task.Run(() => Resubstitute());
            task.Wait();
            return true;
        }

        private async Task<bool> Resubstitute()
        {
            TranslationFile translationFile = FileUtility.CreateTranslationFileFromFile(_translationFileNameSub);
            if (translationFile == null)
            {
                Log.Warning("Translation file resub not found! " + _translationFileNameSub);
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
            translationFileSetSubstitution.PathKeyFile = $"{pathToSubstitedFileParts}{FileSubstitutionConstants.KEY_SUFFIX}";
            translationFileSetSubstitution.PathNestingStringsFile = $"{pathToSubstitedFileParts}{FileSubstitutionConstants.NESTING_STRING_SUFFIX}";
            translationFileSetSubstitution.PathNamespaceFile = $"{pathToSubstitedFileParts}{FileSubstitutionConstants.NAMESPACE_SUFFIX}";
            translationFileSetSubstitution.PathIconFile = $"{pathToSubstitedFileParts}{FileSubstitutionConstants.ICON_SUFFIX}";

            return translationFileSetSubstitution;
        }


        private async Task DoResubstitutionPart()
        {
            //TODO: 2025-11-18 - JHA - Verfahren zum Aufteilen Daten
            //Aufteilen in 8 Teile, wenn die Anzahl an Keys größer als 8000?! ist. -> Parametrierbar machen
            List<Task> allTasks = new List<Task>();
            int part = 1;
            for (int i = 0; i < 4; i++)
            {
                allTasks.Add(Task.Run(() => ResubstitutePart(part++)));
            }

            await Task.WhenAll(allTasks);
        }
        private async Task<bool> ResubstitutePart(int part)
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
