using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorController
    {
        List<ILineCorrector> _lineCorrectors = new List<ILineCorrector>();
        List<string> _lines = new List<string>();
        string _fileName;

        public List<string> Lines { get => _lines; set => _lines = value; }
        public string FileName { get => _fileName; set => _fileName = value; }
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

            if( false == File.Exists(_fileName) )
            {
                Log.Warning("Parameter FileName is not valid!");
                return;
            }

            foreach ( ILineCorrector lineCorrector in _lineCorrectors )
            {
                Corrector.Correct(_lines);

                List<string> incorrect = lineCorrector.GetIncorrect();
                if (incorrect.Count() != 0)
                {
                    SaveIncorrect(lineCorrector);
                }

                List<string> corrected = lineCorrector.GetCorrected();
                if (corrected.Count() != 0)
                {
                    SaveCorrectedFile(corrected);
                    _lines = corrected;
                }
            }
        }

        private void SaveIncorrect(ILineCorrector lineCorrector)
        {
            if (null == lineCorrector)
            {
                return;
            }

            if (null == lineCorrector.GetIncorrect)
            {
                return;
            }

            using (StreamWriter outputFile = new StreamWriter(_fileName + "." + lineCorrector.GetIncorrectFileExtension() ))
            {
                foreach (string line in lineCorrector.GetIncorrect())
                {
                    outputFile.WriteLine(line);
                }
            }
        }

        private void SaveCorrectedFile(List<string>? corrected)
        {
            if (null == corrected)
            {
                return;
            }

            using (StreamWriter outputFile = new StreamWriter(_fileName + FileSubstitutionConstants.FILE_SUFFIX_CORRECTED))
            {
                foreach (string line in corrected)
                {
                    outputFile.WriteLine(line);
                }
            }
        }
    }
}
