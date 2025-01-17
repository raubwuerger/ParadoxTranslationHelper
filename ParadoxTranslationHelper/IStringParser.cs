using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public interface IStringParser
    {
        public List<string> GetToken(string source, List<string> tokens);

    }
}
