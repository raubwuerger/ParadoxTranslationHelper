using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectSubstitute : FunctionObjectBase
    {
        string _pathToSubstitute;
        bool _substituteAgainstSteam = false;

        public FunctionObjectSubstitute(string name) : base(name)
        {
        }

        public string PathToSubstitute { get => _pathToSubstitute; set => _pathToSubstitute = value; }
        public bool SubstituteAgainstSteam { get => _substituteAgainstSteam; set => _substituteAgainstSteam = value; }

        public override bool DoWork()
        {
            if( _pathToSubstitute == null)
            {
                Console.WriteLine("Member <PathToSubstitute> not set!");
                return false;
            }

            LocalisationEnglish = Utility.CreateTranslationFilesFromDirectory(_pathToSubstitute);

            foreach (TranslationFile translationFile in LocalisationEnglish)
            {
                FileSubstitutor fileSubstitutor = new FileSubstitutor();
                if( true == fileSubstitutor.Substitute(translationFile) )
                {
                    Console.WriteLine("Substitution successfully!");
                    if( false == Utility.CreateEmptyFileUTF8_BOM( SubstitutionHelper.CreateFileNameResub(translationFile.FileName) ) )
                    {
                        Console.WriteLine("Unable to create file: " + SubstitutionHelper.CreateFileNameResub(translationFile.FileName) );
                    }
                }
            }

            return true;
        }
    }
}
