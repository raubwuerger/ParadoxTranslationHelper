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
    public class FunctionObjectLineCorrectorSubstitute : FunctionObjectBase
    {
        string _pathToAnalysis;
        string _translationFileNameSub;

        public string PathToReSubstitute { get => _pathToAnalysis; set => _pathToAnalysis = value; }
        public string TranslationFileNameSub { get => _translationFileNameSub; set => _translationFileNameSub = value; }

        public FunctionObjectLineCorrectorSubstitute(string name) : base(name)
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

            LineCorrectorFactory lineCorrectorFactory = new LineCorrectorFactory();
            LineCorrectorController lineCorrector = new LineCorrectorController();
            lineCorrector.Lines = fileSub;
            lineCorrector.FileName = _translationFileNameSub;
            lineCorrector.Add(lineCorrectorFactory.CreateCorrectorSubstitution(FileSubstitutionConstants.COLOR_CODE_SUFFIX));
            lineCorrector.Add(lineCorrectorFactory.CreateCorrectorSubstitution(FileSubstitutionConstants.ICON_SUFFIX));
            lineCorrector.Add(lineCorrectorFactory.CreateCorrectorSubstitution(FileSubstitutionConstants.NAMESPACE_SUFFIX));
            lineCorrector.Add(lineCorrectorFactory.CreateCorrectorSubstitution(FileSubstitutionConstants.NESTING_STRING_SUFFIX));
            lineCorrector.Add(lineCorrectorFactory.CreateCorrectorSubstitution(FileSubstitutionConstants.NEW_LINE_SUFFIX));

            lineCorrector.Work();

            //TODO: 2025-12-30 - JHA - Wie erkenne ich ob alle Keys korrekt waren?
            return true;
        }
    }
}
