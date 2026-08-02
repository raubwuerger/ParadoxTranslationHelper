using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Helper
{
    internal class MultipleKeyRemover
    {
        public Dictionary<int, LineObject> RemoveMultipleKeys(Dictionary<int, LineObject> sortedByLineNumber )
        {
            Dictionary<string, LineObject> keyBased =  DictionaryHelper.ConvertDictionary(sortedByLineNumber);
            Dictionary<int, LineObject> processed = ConvertSortedDictionary(keyBased);
            return processed;
        }

        private Dictionary<int, LineObject> ConvertSortedDictionary(Dictionary<string, LineObject> sortedByKey)
        {
            Dictionary<int, LineObject> converted = new Dictionary<int, LineObject>();
            LineObject lineObject = new LineObject(1);
            lineObject.OriginalLine = Constants.LOCALISATION_GERMAN_FILE_IDENTIFIER;
            converted.Add(1, lineObject);

            int index = 1;
            foreach (KeyValuePair<String, LineObject> keyValuePair in sortedByKey)
            {
                if (index == 1)
                {
                    index++;
                    continue;
                }

                converted.Add((converted.Count) + 1, keyValuePair.Value);
            }

            return converted;
        }

    }
}
