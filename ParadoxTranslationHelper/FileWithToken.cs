using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FileWithToken
    {
        private string _fileName;
        private string _fileNameWithoutLocalisation;
        private string _pathName;
        private string _pathNameToSave;
        private List<LineTextTupel> _lineTextTupels = new List<LineTextTupel>();

        public FileWithToken( List<LineTextTupel> lineTextTupels)
        {
            _lineTextTupels = lineTextTupels;
        }

        public string FileName { get => _fileName; set => _fileName = value; }

        public string PathName { get => _pathName; set => _pathName = value; }

        public string PathNameToSave { get => _pathNameToSave; set => _pathNameToSave = value; }

        public string FileNameWithoutLocalisation { get => _fileNameWithoutLocalisation; set => _fileNameWithoutLocalisation = value; }

        public List<LineTextTupel> GetLineTextTupels { get => _lineTextTupels; set => _lineTextTupels = value; }

        public int Count()
        {
            return _lineTextTupels.Count;
        }
    }
}
