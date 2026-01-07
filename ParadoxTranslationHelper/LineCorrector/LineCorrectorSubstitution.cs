using ParadoxTranslationHelper.Helper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineCorrector
{
    internal class LineCorrectorSubstitution : ILineCorrector
    {
        string _substitutionSuffix;
        List<string> _correctedLines = new List<string>();
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
                Log.Warning("Not");
                return;
            }

            _correctedLines.Clear();

            Dictionary<string, string> keys = LinesToKey(lines);

            foreach(KeyValuePair<string,LineObjectSubstitutionFile> key in _substitutions )
            {
                //TODO: 2026-01-07 - JHA - Funktioniert nicht! In den lines stehen nur die substituierten Keys
                if( false == keys.ContainsKey( key.Value.Key ) )
                {
                    Log.Debug($"Key not found in key file {key.Value.Key}!");
                    continue;
                }


            }

        }

        Dictionary<string, string> LinesToKey(List<string> lines)
        {
            Dictionary<string, string> keys = new Dictionary<string, string>();
            foreach (string line in lines)
            {
                try
                {
                    if( line.Length < 16 )
                    {
                        continue;
                    }
                    //TODO: 2026-01-07 - JHA - Nur solange die Ausgangsdatei nicht korrekt ist
                    if (keys.ContainsKey(line.Substring(0, 16)))
                    {
                        continue;
                    }
                    keys.Add(line.Substring(0, 16), line.Substring(16, line.Length - 16));
                }
                catch (Exception ex)
                {
                    int what_the_fuck = 1;
                }
            }

            return keys;
        }

        bool IsCorrectInitialized()
        {
            if( false == DictionaryHelper.IsValid(_substitutions) )
            {
                Log.Debug("Member <Substitutions> isn't valid!");
                return false;
            }

            if (false == DictionaryHelper.IsValid(_substitutionsKey))
            {
                Log.Debug("Member <SubstitutionsKey> isn't valid!");
                return false;
            }

            return true;
        }
        public List<string> GetIncorrect()
        {
            return new List<string>();
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
