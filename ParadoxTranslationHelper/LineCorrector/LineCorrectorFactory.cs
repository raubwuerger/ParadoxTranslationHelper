using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorFactory
    {
        public ILineCorrector CreateValidateKeys()
        {
            return new LineCorrectorKeys();
        }

        public ILineCorrector CreateFirstLast()
        {
            return new LineCorrectorFirstLast();
        }
    }
}
