using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorFactory
    {
        public ILineCorrector CreateCorrectorKeys()
        {
            return new LineCorrectorKeys();
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
    }
}
