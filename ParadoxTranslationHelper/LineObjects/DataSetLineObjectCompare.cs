using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    internal class DataSetLineObjectCompare
    {
        public Dictionary<string, LineObject> keysUnique = new Dictionary<string, LineObject>();
        public List<LineObject> keysMultiple = new List<LineObject>();
    }
}
