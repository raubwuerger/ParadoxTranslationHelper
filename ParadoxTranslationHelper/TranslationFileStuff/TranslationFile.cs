using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace ParadoxTranslationHelper
{
    public class TranslationFile
    {
        private string _fileName;
        private string _fileNameWithoutLocalisation;
        private Dictionary<int, LineObject> _lines = new Dictionary<int, LineObject>();
        public TranslationFile(string filename)
        {
            _fileName = filename;
        }

        public TranslationFile(TranslationFile translationFile)
        {
            if (translationFile == null)
            {
                throw new ArgumentNullException("Parameter <translationFile> must not be null!");
            }

            this._fileName = translationFile._fileName;
            this._fileNameWithoutLocalisation = translationFile._fileNameWithoutLocalisation;
            foreach( KeyValuePair<int,LineObject> keyValuePair in translationFile.Lines )
            {
                _lines.Add( keyValuePair.Key, keyValuePair.Value ); 
            }
        }
        protected TranslationFile() 
        {
        }

        public string FileName { get => _fileName; }
        public string FileNameWithoutLocalisation { get => _fileNameWithoutLocalisation; set => _fileNameWithoutLocalisation = value; }
        internal Dictionary<int, LineObject> Lines { get => _lines; set => _lines = value; }

        public override string ToString()
        {
            return _fileName;
        }
        public override bool Equals(object obj) => this.Equals(obj as TranslationFile);

        public bool Equals(TranslationFile translationFile)
        {
            if (translationFile is null)
            {
                return false;
            }

            if (System.Object.ReferenceEquals(this, translationFile))
            {
                return true;
            }

            if (this.GetType() != translationFile.GetType())
            {
                return false;
            }

            return this._fileName == translationFile._fileName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_fileName);
        }

    }
}
