using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public interface IFunctionObject
    {
        string Name { get; }

        string Description { get; set; }
        bool DoWork();
    }
}
