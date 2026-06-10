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
    internal class LineCorrectorSubstitution : ILineCorrector
    {
        string _substitutionSuffix;
        List<string> _correctedLines = new List<string>();
        List<string> _incorrectLines = new List<string>();
        List<string> _keysNotFound = new List<string>();
        Dictionary<string, LineObjectSubstitutionFile> _substitutions;
        Dictionary<string, LineObjectSubstitutionFile> _substitutionsKey;

        public string Name { get => "LineCorrectorSubstitution"; }
        public string SubstitutionSuffix { get => _substitutionSuffix; set => _substitutionSuffix = value; }
        public Dictionary<string, LineObjectSubstitutionFile> Substitutions { get => _substitutions; set => _substitutions = value; }
        public Dictionary<string, LineObjectSubstitutionFile> SubstitutionsKey { get => _substitutionsKey; set => _substitutionsKey = value; }

        public void Correct(List<string> lines)
        {
            if( false == IsCorrectInitialized() )
            {
                Log.Warning($"{_substitutionSuffix}: Not correct initialized!");
                return;
            }

            _correctedLines.Clear();
            Dictionary<string, string> keys = LineHelper.LinesToKeyReal(lines);

            int replaced = 0;
            foreach ( KeyValuePair<string,LineObjectSubstitutionFile> substitutePair in _substitutions )
            {
                string substitute = substitutePair.Key;
                string key = substitutePair.Value.Key;
                string original = substitutePair.Value.SubstitutedValue;

                if( false == keys.ContainsKey(key) )
                {
                    _keysNotFound.Add($"{key};{substitute};{original}");
                    Log.Debug($"{_substitutionSuffix}: Key not found! {key}");
                    continue;
                }

                string line = keys[key];
                if( null == line )
                {
                    continue;
                }

                if( false == line.Contains(substitute) )
                {
                    Log.Debug($"{_substitutionSuffix}: Line doesn't contain substitute: {substitute} <> {line}");
                    _incorrectLines.Add(substitute);
                    continue;
                }

                keys[key] = line.Replace(substitute, original);
                replaced++;
            }

            _correctedLines = keys.Values.ToList<string>();
            WriteKeysNotFound(_keysNotFound);
            Log.Information($"{_substitutionSuffix}: Corrected {replaced} items out of {_substitutions.Count}");
        }

        void WriteKeysNotFound(List<string> keysNotFound)
        {
            FileUtility.WriteLines(keysNotFound, Path.Combine(ParadoxTranslationHelperConfig.PathResult, $"{Name}.{_substitutionSuffix}{FileSubstitutionConstants.FILE_SUFFIX_NOT_FOUND}"));
        }


        bool IsCorrectInitialized()
        {
            if( false == DictionaryHelper.IsValid(_substitutions) )
            {
                Log.Debug($"Member <Substitutions> isn't valid! Substitution: {_substitutionSuffix}");
                return false;
            }

            if (false == DictionaryHelper.IsValid(_substitutionsKey))
            {
                Log.Debug($"Member <SubstitutionsKey> isn't valid! Substitution: {_substitutionSuffix}");
                return false;
            }

            return true;
        }
        public List<string> GetIncorrect()
        {
            return _incorrectLines;
        }

        public List<string> GetCorrected()
        {
            return _correctedLines;
        }

        public string GetIncorrectFileExtension()
        {
            return _substitutionSuffix +".incorrect";
        }

        public string GetCorrectFileExtension()
        {
            return _substitutionSuffix +".correct";
        }

    }
}
