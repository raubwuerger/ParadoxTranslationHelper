using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.FunctionObject
{
    public class FunctionObjectRegistryInitialiser
    {
        static public void Init()
        {
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateSubstitute());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateReSubstitute());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateAnalyse());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateDiff());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateDiffSteam());
        }
    }
}

