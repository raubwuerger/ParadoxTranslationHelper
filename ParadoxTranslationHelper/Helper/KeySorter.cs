using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Helper
{
    internal class KeySorter
    {
        Dictionary<string, string> _originalKeys;
        Dictionary<string, string> _translatedKeys;
        Dictionary<string, string> _notTranslatedKeys;
        Dictionary<string, string> _sortedKeys = new Dictionary<string, string>();
        Dictionary<string, string> _notFoundKeys = new Dictionary<string, string>();

        public Dictionary<string, string> OriginalKeys { get => _originalKeys; set => _originalKeys = value; }
        public Dictionary<string, string> TranslatedKeys { get => _translatedKeys; set => _translatedKeys = value; }
        public Dictionary<string, string> NotTranslatedKeys { get => _notTranslatedKeys; set => _notTranslatedKeys = value; }
        public Dictionary<string, string> SortedKeys { get => _sortedKeys; }
        public Dictionary<string, string> NotFoundKeys { get => _notFoundKeys; set => _notFoundKeys = value; }

        public bool Sort()
        {
            if( false == IsCorrectInitialized() )
            {
                return false;
            }

            foreach( KeyValuePair<string,string> keyValuePair in _originalKeys )
            {
                if( true == _translatedKeys.ContainsKey(keyValuePair.Key) )
                {
                    _sortedKeys.Add(keyValuePair.Key, _translatedKeys[keyValuePair.Key]);
                    continue;
                }

                if( true == _notTranslatedKeys.ContainsKey(keyValuePair.Key) )
                {
                    _sortedKeys.Add(keyValuePair.Key, _notTranslatedKeys[keyValuePair.Key]);
                    continue;
                }

                _notFoundKeys.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return _notFoundKeys.Count == 0;
        }

        bool IsCorrectInitialized()
        {
            if( false == DictionaryHelper.IsValid(_originalKeys) )
            {
                Log.Warning($"Member OriginalKeys must not be null!");
                return false;
            }

            if (false == DictionaryHelper.IsValid(_translatedKeys))
            {
                Log.Warning($"Member TranslatedKeys must not be null!");
                return false;
            }

            if (false == DictionaryHelper.IsValid(_notTranslatedKeys))
            {
                Log.Warning($"Member NotTranslatedKeys must not be null!");
                return false;
            }

            return true;
        }
    }
}
