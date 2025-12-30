using Serilog;
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
        List<string> _lines = new List<string>();

        public List<string> Lines { get => _lines; set => _lines = value; }
        internal List<ILineCorrector> LineCorrectors { get => _lineCorrectors; set => _lineCorrectors = value; }

        public void Add(ILineCorrector lineCorrector)
        {
            if (null == lineCorrector)
            {
                return;
            }

            //TODO: 2025-12-29 - JHA - Add LogMessage ...
            _lineCorrectors.Add(lineCorrector);
        }

        public void Work()
        {
            if( null == _lines )
            {
                Log.Warning("Parameter Lines must not be empty!");
                return;
            }

            if ( _lines.Count() == 0 )
            {
                Log.Warning("Parameter Lines must not be empty!");
                return;
            }

            foreach ( ILineCorrector lineCorrector in _lineCorrectors )
            {
                lineCorrector.Correct(_lines);
            }
        }
    }
}
