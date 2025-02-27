using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Utilities
{
    public static class FunctionUtility
    {
        public static Dictionary<string, LineObject> FindToCreate(Dictionary<string, LineObject> repository, Dictionary<string, LineObject> steam)
        {
            if (repository == null)
            {
                return steam;
            }

            if (repository.Count == 0)
            {
                return steam;
            }

            Dictionary<string, LineObject> toCreate = new Dictionary<string, LineObject>();
            foreach (KeyValuePair<string, LineObject> pair in steam)
            {
                if (repository.ContainsKey(pair.Key))
                {
                    continue;
                }
                toCreate.Add(pair.Key, pair.Value);
            }

            return toCreate;
        }
    }
}
