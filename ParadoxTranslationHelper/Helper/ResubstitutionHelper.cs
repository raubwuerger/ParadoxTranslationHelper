using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Helper
{
    internal class ResubstitutionHelper
    {
        /*
        public bool ReSubstituteLines(List<LineObject> lineObjects, ref Dictionary<string, string> substituteTokens)
        {
            if( null == lineObjects )
            {
                Log.Debug("Parameter <List<LineObject>> must not be null!");
                return false;
            }

            if (null == substituteTokens)
            {
                Log.Debug("Parameter <Dictionary<string, string>> must not be null!");
                return false;
            }

            List<KeyValuePair<string, string>> substituteTokensList = substituteTokens.ToList();

            foreach (LineObject lineObject in lineObjects)
            {
                if (lineObject.OriginalLine.StartsWith(">"))
                {
                    continue;
                }

                foreach (KeyValuePair<string, string> item in substituteTokensList.ToList())
                {
                    if (false == lineObject.OriginalLineSubstituted.Contains(item.Key))
                    {
                        continue;
                    }
                    lineObject.OriginalLineSubstituted = lineObject.OriginalLineSubstituted.Replace(item.Key, item.Value);
                    Log.Debug($"Replaced item: {item.Key} --> {item.Value}");
                    substituteTokens.Remove(item.Key);
                    return true;
                }
            }

            return false;
        }
        */

        public List<KeyValuePair<string, string>>? ReSubstituteLines(List<LineObject> lineObjects, ref Dictionary<string, string> substituteTokens)
        {
            if (null == lineObjects)
            {
                Log.Debug("Parameter <List<LineObject>> must not be null!");
                return null;
            }

            if (null == substituteTokens)
            {
                Log.Debug("Parameter <Dictionary<string, string>> must not be null!");
                return null;
            }

            List<KeyValuePair<string, string>> substituteTokensList = substituteTokens.ToList();

            foreach (LineObject lineObject in lineObjects)
            {
                if (lineObject.OriginalLine.StartsWith(">"))
                {
                    continue;
                }

                foreach (KeyValuePair<string, string> item in substituteTokensList.ToList())
                {
                    if (false == lineObject.OriginalLineSubstituted.Contains(item.Key))
                    {
                        continue;
                    }
                    lineObject.OriginalLineSubstituted = lineObject.OriginalLineSubstituted.Replace(item.Key, item.Value);
                    Log.Debug($"Replaced item: {item.Key} --> {item.Value}");
                    substituteTokens.Remove(item.Key);
                    return substituteTokensList;
                }
            }

            return substituteTokensList;
        }
    }
}
