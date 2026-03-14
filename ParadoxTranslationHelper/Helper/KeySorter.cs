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

        public Dictionary<int, LineObject> OriginalKeys { get => _originalKeys; set => _originalKeys = value; }
        public Dictionary<int, LineObject> TranslatedKeys { get => _translatedKeys; set => _translatedKeys = value; }
        public Dictionary<int, LineObject> SortedKeys { get => _sortedKeys; }

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

            Dictionary<string, LineObject> translatedConverted = DictionaryHelper.ConvertDictionary(_translatedKeys);

            foreach ( KeyValuePair<int, LineObject> keyValuePair in _originalKeys )
            {
                if( true == IsLanguageLine(keyValuePair.Value) )
                {
                    keyValuePair.Value.OriginalLine = Constants.LOCALISATION_GERMAN_FILE_IDENTIFIER;
                    _sortedKeys.Add(keyValuePair.Key, keyValuePair.Value);
                    continue;
                }

                if ( false == IsValidLine(keyValuePair.Value) )
                {
                    _sortedKeys.Add(keyValuePair.Key, keyValuePair.Value);
                    continue;
                }

                if ( false == translatedConverted.ContainsKey(keyValuePair.Value.Key.Trim()) )
                {
                    keyValuePair.Value.OriginalLine += Constants.LINE_NOT_TRANSLATED;
                    _sortedKeys.Add(keyValuePair.Key, keyValuePair.Value);
                    continue;
                }

                if ( false == _sortedKeys.ContainsValue(keyValuePair.Value) )
                {
                    if( false == _translatedKeys.ContainsKey(keyValuePair.Key) )
                    {
                        keyValuePair.Value.OriginalLine += Constants.LINE_NOT_TRANSLATED;
                        _sortedKeys.Add(keyValuePair.Key, keyValuePair.Value);
                        continue;
                    }
                    
                    _sortedKeys.Add(keyValuePair.Key, _translatedKeys[keyValuePair.Key]);
                    continue;
                }
            }

            Log.Debug($"Original keys count: {_originalKeys.Count}");
            Log.Debug($"Keys to sort count: {_translatedKeys.Count}");

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
    }
}
