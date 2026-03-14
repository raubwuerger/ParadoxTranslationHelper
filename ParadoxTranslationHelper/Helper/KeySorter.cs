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
        Dictionary<int, LineObject> _sortedKeys = new Dictionary<int, LineObject>();
        Dictionary<int, LineObject> _notFoundKeys = new Dictionary<int, LineObject>();
        Dictionary<int, LineObject> _ignoredKeys = new Dictionary<int, LineObject>();
        Dictionary<string, LineObject> _sortedByKey = new Dictionary<string, LineObject>();

        public Dictionary<int, LineObject> OriginalKeys { get => _originalKeys; set => _originalKeys = value; }
        public Dictionary<int, LineObject> TranslatedKeys { get => _translatedKeys; set => _translatedKeys = value; }
        public Dictionary<int, LineObject> SortedKeys { get => _sortedKeys; }
        public Dictionary<int, LineObject> NotFoundKeys { get => _notFoundKeys; }
        public Dictionary<string, LineObject> SortedByKey { get => _sortedByKey; }

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

            Dictionary<string, LineObject> translatedConverted = ConvertDictionary(_translatedKeys);
            List<String> notAKey = new List<string>();

            foreach ( KeyValuePair<int, LineObject> keyValuePair in _originalKeys )
            {
                if( true == IsLanguageLine(keyValuePair.Value) )
                {
                    keyValuePair.Value.OriginalLine = Constants.LOCALISATION_GERMAN_FILE_IDENTIFIER;
                    _sortedKeys.Add(keyValuePair.Key, keyValuePair.Value);
                    notAKey.Add(Constants.LOCALISATION_GERMAN_FILE_IDENTIFIER);
                    _sortedByKey.Add(Constants.LOCALISATION_GERMAN_FILE_IDENTIFIER, keyValuePair.Value);
                    continue;
                }

                if ( false == IsValidLine(keyValuePair.Value) )
                {
                    _sortedKeys.Add(keyValuePair.Key, keyValuePair.Value);
                    notAKey.Add(keyValuePair.Value.OriginalLine);
//                    sortedByKey.Add(keyValuePair.Value.Key, keyValuePair.Value);
                    continue;
                }

//                if (false == _translatedKeys.ContainsValue(keyValuePair.Value))
                if ( false == translatedConverted.ContainsKey(keyValuePair.Value.Key.Trim()) )
                {
                    keyValuePair.Value.OriginalLine += "##### NOT TRANSLATED #####";
                    _sortedKeys.Add(keyValuePair.Key, keyValuePair.Value);
                    _notFoundKeys.Add(keyValuePair.Key, keyValuePair.Value);
                    notAKey.Add(keyValuePair.Value.OriginalLine);
                    _sortedByKey.Add(keyValuePair.Value.Key.Trim(), keyValuePair.Value);
                    continue;
                }

//                    if ( false == _sortedKeys.ContainsValue(keyValuePair.Value) )
                if (false == _sortedByKey.ContainsKey(keyValuePair.Value.Key))
                {
                    _sortedByKey.Add(keyValuePair.Value.Key, keyValuePair.Value);

                    if( false == _translatedKeys.ContainsKey(keyValuePair.Key) )
                    {
                        keyValuePair.Value.OriginalLine += "##### NOT TRANSLATED #####";
                        _sortedKeys.Add(keyValuePair.Key, keyValuePair.Value);
                        continue;
                    }
                    
                    _sortedKeys.Add(keyValuePair.Key, _translatedKeys[keyValuePair.Key]);
                    continue;
                }

                _ignoredKeys.Add(keyValuePair.Key, keyValuePair.Value);
            }

            Log.Debug($"Original keys count: {_originalKeys.Count}");
            Log.Debug($"Keys to sort count: {_translatedKeys.Count}");
            Log.Debug($"Keys not found: {_notFoundKeys.Count}");

            _sortedKeys = ConvertSortedDictionary();

            if (_notFoundKeys.Count != 0)
            {
                Log.Debug($"Missing keys {_notFoundKeys.Count} in translation!");
                return false;
            }

            if( _originalKeys.Count != _translatedKeys.Count )
            {
                Log.Debug($"Key count doesn't match!");
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

        bool IsLanguageLine(LineObject lineObject)
        {
            if (true == lineObject.OriginalLine.TrimStart().StartsWith(Constants.LOCALISATION_ENGLISH_FILE_IDENTIFIER))
            {
                return true;
            }

            if (true == lineObject.OriginalLine.TrimStart().StartsWith(Constants.LOCALISATION_GERMAN_FILE_IDENTIFIER))
            {
                return true;
            }

            return false;
        }

        bool IsValidLine(LineObject lineObject)
        {
            if( null == lineObject )
            {
                return false;
            }

            if( lineObject.OriginalLine == null )
            {
                return false;
            }

            if( lineObject.Key == null )
            {
                return false;
            }

            if( true == lineObject.OriginalLine.TrimStart().StartsWith("#") )
            {
                return false;
            }

            if (true == (lineObject.OriginalLine.TrimStart().Length == 0))
            {
                return false;
            }

            return true;
        }

        private Dictionary<string,LineObject> ConvertDictionary( Dictionary<int,LineObject> toConvert )
        {
            Dictionary<string, LineObject> converted = new Dictionary<string, LineObject>();

            foreach( KeyValuePair<int, LineObject> keyValuePair in toConvert )
            {
                if ( true == converted.ContainsKey(keyValuePair.Value.Key.Trim()))
                {
                    Log.Debug($"Key already exists: {keyValuePair.Value.Key.Trim()}" );
                    continue;
                }

                converted.Add(keyValuePair.Value.Key, keyValuePair.Value);
            }

            return converted;
        }

        private Dictionary<int,LineObject> ConvertSortedDictionary()
        {
            Dictionary<int, LineObject> converted = new Dictionary<int, LineObject>();
            LineObject lineObject = new LineObject(1);
            lineObject.OriginalLine = Constants.LOCALISATION_GERMAN_FILE_IDENTIFIER;
            converted.Add(1, lineObject);

            int index = 1;
            foreach(KeyValuePair<String, LineObject> keyValuePair in _sortedByKey)
            {
                if( index == 1 )
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
