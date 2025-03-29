using Serilog;
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
        List<TranslationFile> _localisationFilesSteam = null;
        List<TranslationFile> _localisationFilesGerman = null;

        string _name;

        string _description;
        public string Name { get => _name; }
        public List<TranslationFile> LocalisationFilesSteam { get => _localisationFilesSteam; set => _localisationFilesSteam = value; }
        public List<TranslationFile> LocalisationFilesGerman { get => _localisationFilesGerman; set => _localisationFilesGerman = value; }
        public string Description { get => _description; set => _description = value; }

        protected FunctionObjectBase(string name) 
        { 
            _name = name;
        }
        public bool Work()
        {
            Log.Information("##### Function {functionName} started #####", _name );
            bool returnValue = DoWork();
            Log.Information("##### Function {functionName} stopped #####", _name);
            return returnValue;
        }
        public abstract bool DoWork();
    }
}
