using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public abstract class FunctionObjectBase : IFunctionObject
    {
        List<TranslationFile> _localisationEnglish = null;
        List<TranslationFile> _localisationEnglishUpdated = null;
        List<TranslationFile> _localisationGerman = null;

        string _resultFileName = null;

        string _name;
        public string Name { get => _name; }
        public List<TranslationFile> LocalisationEnglish { get => _localisationEnglish; set => _localisationEnglish = value; }
        public List<TranslationFile> LocalisationEnglishUpdated { get => _localisationEnglishUpdated; set => _localisationEnglishUpdated = value; }
        public List<TranslationFile> LocalisationGerman { get => _localisationGerman; set => _localisationGerman = value; }
        public string ResultFileName { get => _resultFileName; set => _resultFileName = value; }

        protected FunctionObjectBase(string name) 
        { 
            _name = name;
        }
        public abstract bool DoWork();

    }
}
