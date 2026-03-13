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
        Dictionary<int, LineObject> _originalKeys;
        Dictionary<int, LineObject> _translatedKeys;
        List<LineObject> _multipleKeys;
        Dictionary<int, LineObject> _sortedKeys = new Dictionary<int, LineObject>();
        Dictionary<int, LineObject> _notFoundKeys = new Dictionary<int, LineObject>();

        public Dictionary<int, LineObject> OriginalKeys { get => _originalKeys; set => _originalKeys = value; }
        public Dictionary<int, LineObject> TranslatedKeys { get => _translatedKeys; set => _translatedKeys = value; }
        public List<LineObject> MultipleKeys { get => _multipleKeys; }
        public Dictionary<int, LineObject> SortedKeys { get => _sortedKeys; }
        public Dictionary<int, LineObject> NotFoundKeys { get => _notFoundKeys; }

        /*
         * returns true if alle keys, in both dictionarys, match each other
         * 
         */
        public bool Sort()
        {
            if( false == IsCorrectInitialized() )
            {
                return false;
            }

            foreach( KeyValuePair<int, LineObject> keyValuePair in _originalKeys )
            {
                if( false == _translatedKeys.Any(value => value.Value.Key == keyValuePair.Value.Key) )
                {
                    _notFoundKeys.Add(keyValuePair.Key, keyValuePair.Value);
                    continue;
                }

                if ( false == _sortedKeys.Any(value => value.Value.Key == keyValuePair.Value.Key) )
                {
                    _sortedKeys.Add(keyValuePair.Key, keyValuePair.Value);
                    continue;
                }

                _multipleKeys.Add(keyValuePair.Value);
            }

            Log.Debug($"Keys sorted: {_originalKeys.Count}");
            Log.Debug($"Keys not found: {_notFoundKeys.Count}");
            Log.Debug($"Keys multiple: {_multipleKeys.Count}");

            if(_notFoundKeys.Count != 0)
            {
                return false;
            }

            if(_multipleKeys.Count != 0)
            {
                return false;
            }
            
            return true;
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

            return true;
        }
    }
}
