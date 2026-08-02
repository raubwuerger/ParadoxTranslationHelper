using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class StringParserEmpty : StringParserBase
    {
        protected override List<string> GetToken(string source, List<string> tokens)
        {
            return new List<string>();
        }

    }
}
