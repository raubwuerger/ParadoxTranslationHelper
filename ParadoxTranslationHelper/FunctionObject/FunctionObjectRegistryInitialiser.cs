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
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateAnalyse());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateSteamDiff());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateSteamSubstitute());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateSteamResubstitute());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateInsertKeys());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateRemoveKeys()); 
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateCheckForDoubleKeys());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateCheckForDoubleKeysAllFiles());
            FunctionObjectRegistry.Instance.Register(FunctionObjectFactory.CreateCheckForDoubleKeysAllFilesFix());
        }
    }
}

