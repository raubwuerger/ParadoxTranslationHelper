using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Helper
{
    public class DictionaryHelper
    {
        /**
    * Double keys will be bypassed.
    */
        public static Dictionary<string, string> ConvertToDictionary(List<string> lines)
        {
            Dictionary<string, string> resubstitutes = new Dictionary<string, string>();
            if (lines == null)
            {
                return resubstitutes;
            }

            foreach (string line in lines)
            {
                string[] splitted = line.Split(';');
                if (splitted.Length < 2)
                {
                    continue;
                }

                if (true == string.IsNullOrEmpty(splitted[0]))
                {
                    continue;
                }
                try
                {
                    resubstitutes.Add(splitted[0], splitted[1]);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            return resubstitutes;
        }
        public static Dictionary<string, string> RemoveStringFromKey(Dictionary<string, string> list, string toRemove)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> s in list)
            {
                result.Add(s.Key.Replace(toRemove, ""), s.Value);
            }

            return result;
        }

        public static Dictionary<string, string> RemoveStringFromValue(Dictionary<string, string> list, string toRemove)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> s in list)
            {
                result.Add(s.Key, s.Value.Replace(toRemove, ""));
            }

            return result;
        }

        public static Dictionary<string, LineObject> GetValidKeys(List<LineObject> lines)
        {
            if (null == lines)
            {
                return null;
            }

            Dictionary<string, LineObject> keys = new Dictionary<string, LineObject>();

            foreach (LineObject line in lines)
            {
                if (string.IsNullOrEmpty(line.Key))
                {
                    continue;
                }

                keys[line.Key] = line;
            }

            return keys;
        }

        public static bool IsValid(Dictionary<string, string> dict)
        {
            if( null == dict )
            {
                return false;
            }

            if( dict.Count == 0 )
            {
                return false;
            }

            return true;
        }

        public static bool IsValid(Dictionary<string, LineObjectSubstitutionFile> dict)
        {
            if (null == dict)
            {
                return false;
            }

            if (dict.Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}
