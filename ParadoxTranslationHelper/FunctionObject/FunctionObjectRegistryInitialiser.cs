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
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateDiffFiles());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateSteamSubstitute());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateLineCorrector());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateLineCorrectorSubstitute());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateInsertKeys());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateRemoveKeys()); 
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateCheckForDoubleKeys());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateCheckForDoubleKeysAllFiles());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateCheckForDoubleKeysAllFilesFix());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateKeysDiff());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateAnalyse());
        }
    }
}

