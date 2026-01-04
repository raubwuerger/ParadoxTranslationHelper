using ParadoxTranslationHelper.Helper;
using ParadoxTranslationHelper.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorKeys : ILineCorrector
    {
        List<string> _doubleKeys = new List<string> { "" };
        Dictionary<string,string> _uniqueKeys = new Dictionary<string,string>();
        string _correctStart = "|___KY";
        string _correctEnd = "___|";
        int _correctEndLength = 0;
        int _correctLength = 16;
        Dictionary<string, string> _missingKeys = new Dictionary<string, string>();
        Dictionary<string, string> _allOrginialKeys;
        List<string> _originalDiffFile = new List<string>();
        List<string> _correctedKeys = new List<string>();

        public Dictionary<string, string> AllOrginialKeys { get => _allOrginialKeys; set => _allOrginialKeys = value; }
        public List<string> OriginalDiffFile { get => _originalDiffFile; set => _originalDiffFile = value; }

        public string Name { get => "LineCorrectorKeys"; }

        public void Correct(List<string> linesToCorrect)
        {
            ClearData();

            _correctEndLength = _correctEnd.Length;

            if ( false == IsCorrectInitialized(linesToCorrect) )
            {
                Log.Warning("Not correct initialzed!");
                return;
            }

            CorrectLines(linesToCorrect);

            _missingKeys = CheckMissingKeys(_allOrginialKeys, _uniqueKeys);
            FileUtility.WriteLines(CreateLinesNotTranslated(_missingKeys, _originalDiffFile), Path.Combine(ParadoxTranslationHelperConfig.PathResult, Constants.FILE_NAME_STEAM_MISSING_KEYS) + ".notTranslated");

            KeySorter keySorter = new KeySorter();
            keySorter.OriginalKeys = _allOrginialKeys;
            keySorter.NotTranslatedKeys = _missingKeys;
            keySorter.TranslatedKeys = _uniqueKeys;

            if( false == keySorter.Sort() )
            {
                //TODO: 2026-01-04 - JHA - Speichern!?
            }

            _correctedKeys = keySorter.SortedKeys.Values.ToList<string>();

            string subGerman = Path.Combine(ParadoxTranslationHelperConfig.PathResult, Constants.FILE_NAME_STEAM_MISSING_KEYS) + FileSubstitutionConstants.FILE_SUFFIX_SUBSTITUTED + FileSubstitutionConstants.FILE_SUFFIX_GERMAN;
            FileUtility.BackupFile(subGerman);
            FileUtility.WriteLines(_correctedKeys, subGerman);
        }

        private void ClearData()
        {
            _missingKeys.Clear();
            _doubleKeys.Clear();
            _uniqueKeys.Clear();
            _correctedKeys.Clear();
        }

        private void CorrectLines(List<string> linesToCorrect)
        {
            foreach (string line in linesToCorrect)
            {
                if (true == IgnoreLine(line))
                {
                    continue;
                }

                if (true == IsKeyCorrect(line))
                {
                    string key = line.Substring(0, _correctLength);
                    if (_uniqueKeys.ContainsKey(key))
                    {
                        _doubleKeys.Add(line);
                    }
                    else
                    {
                        _uniqueKeys.Add(key, line);
                        _correctedKeys.Add(line);
                    }
                    continue;
                }
            }
        }

        bool IsKeyCorrect( string line )
        {
            int indexStart = line.IndexOf(_correctStart);
            if ( indexStart == -1 )
            {
                Log.Verbose($"Start string {_correctStart} not found!");
                return false;
            }

            int indexEnd = line.IndexOf(_correctEnd);
            if( indexEnd == -1 )
            {
                Log.Verbose($"End string {_correctEnd} not found!");
                return false;
            }

            int length = (indexEnd + _correctEndLength) - indexStart;
            if( length != _correctLength )
            {
                Log.Verbose($"Length mismatch: should={_correctLength}, is={length}");
                return false;
            }

            return true;
        }

        Dictionary<string, string>? CheckMissingKeys(Dictionary<string, string> allOrginialKeys, Dictionary<string, string> uniqueKeys)
        {
            if( allOrginialKeys == null )
            {
                Log.Warning("Parameter <allOrginialKeys> must not be null!");
                return null;
            }

            if (uniqueKeys == null)
            {
                Log.Warning("Parameter <uniqueKeys> must not be null!");
                return null;
            }

            Dictionary<string, string> missingKeys = new Dictionary<string, string>();

            foreach( string originalKey in allOrginialKeys.Keys.ToList<string>() )
            {
                if( true == uniqueKeys.ContainsKey(originalKey) )
                {
                    continue;
                }

                missingKeys.Add(originalKey, allOrginialKeys[originalKey]);
            }

            return missingKeys;
        }

        List<string>? CreateLinesNotTranslated(Dictionary<string, string>  missingKeys, List<string> originalDiffFile )
        {
            if( missingKeys.Count == 0 )
            {
                Log.Debug("No missing keys found!");
                return null;
            }

            List<string> keysNotTranslated = new List<string>();

            //TODO: 2026-01-02 - JHA - Suche in Liste ist langsam
            foreach (KeyValuePair<string,string> keyValuePair in missingKeys )
            {
                string found = originalDiffFile.Find(x => x.Contains(keyValuePair.Value));
                if( null == found )
                {
                    Log.Warning($"Key {keyValuePair.Value} not found in source file!");
                    continue;
                }
                keysNotTranslated.Add(found);
            }

            return keysNotTranslated;
        }


        bool IgnoreLine(string line)
        {
            if( true == line.Trim().StartsWith(Constants.TRANSLATION_FILE_IDENTIFIER) )
            {
                return true;
            }

            if( true == string.IsNullOrWhiteSpace(line) )
            {
                return true;
            }

            return false;
        }

        public List<string> GetIncorrect()
        {
            return _missingKeys.Keys.ToList<string>();
        }
        public List<string> GetCorrected()
        {
            return _correctedKeys;
        }
        public string GetIncorrectFileExtension()
        {
            return "Keys_missing";
        }

        public string GetCorrectFileExtension()
        {
            return "Keys_correct";
        }

        private bool IsCorrectInitialized(List<string> lines)
        {
            if (false == DictionaryHelper.IsValid(_allOrginialKeys) )
            {
                Log.Warning("Member <AllOrginialKeys> must not be null!");
                return false;
            }

            if (null == lines)
            {
                Log.Warning("Parameter <lines> must not be null!");
                return false;
            }

            if (lines.Count() == 0)
            {
                Log.Warning("Parameter <lines> must not be empty!");
                return false;
            }

            if (null == _originalDiffFile)
            {
                Log.Warning("Parameter <OriginalDiffFile> must not be null!");
                return false;
            }

            if (_originalDiffFile.Count() == 0)
            {
                Log.Warning("Parameter <OriginalDiffFile> must not be empty!");
                return false;
            }
            
            return true;
        }

    }
}
