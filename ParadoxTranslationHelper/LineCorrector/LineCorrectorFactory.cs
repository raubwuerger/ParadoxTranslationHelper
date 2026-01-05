using ParadoxTranslationHelper.SubResubstitution;
using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorFactory
    {
        public ILineCorrector CreateCorrectorKeys()
        {
            LineCorrectorKeys lineCorrector = new LineCorrectorKeys();
            lineCorrector.AllOrginialKeys = FileUtility.ReadSuffixFile( Path.Combine(ParadoxTranslationHelperConfig.PathResult, Constants.FILE_NAME_STEAM_MISSING_KEYS) +"." +FileSubstitutionConstants.KEY_SUFFIX);
            lineCorrector.OriginalDiffFile = FileUtility.ReadFile(Path.Combine(ParadoxTranslationHelperConfig.PathResult, Constants.FILE_NAME_STEAM_MISSING_KEYS));
            return lineCorrector;
        }

        public ILineCorrector CreateCorrectorSplitByKeys()
        {
            return new LineCorrectorSplitByKeys();
        }

        public ILineCorrector CreateCorrectorFirstLast()
        {
            return new LineCorrectorFirstLast();
        }

        public ILineCorrector CreateCorrectorQuotationMark()
        {
            return new LineCorrectorQuotationMark();
        }

        public ILineCorrector CreateCorrectorMissingQuotationMark()
        {
            return new LineCorrectorMissingQuotationMark();
        }

        public ILineCorrector CreateCorrectorSubstitution(string suffix)
        {
            SubstitutionFileReader fileReader = SubstitutionFileReaderFactory.Create(suffix);

            LineCorrectorSubstitution lineCorrector = new LineCorrectorSubstitution();
            lineCorrector.SubstitutionSuffix = suffix;
            lineCorrector.Substitutions = fileReader.ReadFile(suffix);

            return lineCorrector;
        }

    }
}
