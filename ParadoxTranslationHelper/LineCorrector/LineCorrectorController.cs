using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorController
    {


        List<ILineCorrector> _lineCorrectors = new List<ILineCorrector>();

        internal List<ILineCorrector> LineCorrectors { get => _lineCorrectors; set => _lineCorrectors = value; }

        void Add( ILineCorrector lineCorrector )
        {
            if( null == lineCorrector )
            {
                return;
            }

            //TODO: 2025-12-29 - JHA - Add LogMessage ...
            _lineCorrectors.Add(lineCorrector);
        }
    }
}
