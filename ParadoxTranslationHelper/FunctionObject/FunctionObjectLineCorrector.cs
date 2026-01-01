using ParadoxTranslationHelper.LineCorrector;
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
        string _pathToAnalysis;
        string _translationFileNameSub;

        public string PathToReSubstitute { get => _pathToAnalysis; set => _pathToAnalysis = value; }
        public string TranslationFileNameSub { get => _translationFileNameSub; set => _translationFileNameSub = value; }

        public FunctionObjectLineCorrector(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            if (_pathToAnalysis == null) 
            {
                Log.Verbose("Member <PathToReSubstitute> must not be null!");
                return false;
            }

            if (_translationFileNameSub == null)
            {
                Log.Verbose("Member <TranslationFileNameSub> must not be null!");
                return false;
            }

            Task task = Task.Run(() => CorrectAnalysis());
            task.Wait();
            return true;
        }

        private async Task<bool> CorrectAnalysis()
        {
            List<string> fileSub = File.ReadAllLines(_translationFileNameSub).ToList<string>();

            //TODO: 2025-12-29 - JHA - Neue Klasse CorrectAnalyse erstellen
            //- Prüft ob alle Keys vorhanden sind
            //- Versucht falsche Keys zu korriegieren |___ ___|
            //- Korrigiert falsche Textanfänge '„' und Textende '“'
            LineCorrectorFactory lineCorrectorFactory = new LineCorrectorFactory();
            LineCorrectorController lineCorrector = new LineCorrectorController();
            lineCorrector.Lines = fileSub;
            lineCorrector.FileName = _translationFileNameSub;
            lineCorrector.Add(lineCorrectorFactory.CreateCorrectorSplitByKeys());
            lineCorrector.Add(lineCorrectorFactory.CreateCorrectorKeys());
            lineCorrector.Add(lineCorrectorFactory.CreateCorrectorQuotationMark());
            lineCorrector.Add(lineCorrectorFactory.CreateCorrectorMissingQuotationMark());

            lineCorrector.Work();

            //TODO: 2025-12-30 - JHA - Wie erkenne ich ob alle Keys korrekt waren?
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
