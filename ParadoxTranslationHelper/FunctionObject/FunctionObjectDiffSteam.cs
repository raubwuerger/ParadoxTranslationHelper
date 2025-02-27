using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.FunctionObject
{
    public class FunctionObjectDiffSteam : FunctionObjectDiff
    {
        string _pathSteam;
        string _pathGerman;
        public FunctionObjectDiffSteam(string name) : base(name)
        {
        }

        public string PathSteam { get => _pathSteam; set => _pathSteam = value; }
        public string PathGerman { get => _pathGerman; set => _pathGerman = value; }

        public override bool DoWork()
        {
            if (true == string.IsNullOrEmpty(_pathGerman))
            {
                Console.WriteLine("Member <PathGerman> must not be null or empty!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_pathSteam))
            {
                Console.WriteLine("Member <PathSteam> must not be null or empty!");
                return false;
            }

            LocalisationEnglishSteam = FileUtility.CreateTranslationFilesFromDirectory(_pathSteam);
            if (null == LocalisationEnglishSteam)
            {
                Console.WriteLine("Steam path not set!");
                return false;
            }

            LocalisationEnglish = FileUtility.CreateTranslationFilesFromDirectory(_pathGerman);

            return CheckNewKeysUpdate();
        }

    }
}
