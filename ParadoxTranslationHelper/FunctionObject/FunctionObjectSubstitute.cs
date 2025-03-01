using ParadoxTranslationHelper.Utilities;
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
        string _translationFileToIgnore;

        public FunctionObjectSubstitute(string name) : base(name)
        {
        }

        public string PathToSubstitute { get => _pathToSubstitute; set => _pathToSubstitute = value; }
        public bool SubstituteAgainstSteam { get => _substituteAgainstSteam; set => _substituteAgainstSteam = value; }
        public string TranslationFileToIgnore { get => _translationFileToIgnore; set => _translationFileToIgnore = value; }

        public override bool DoWork()
        {
            if( _pathToSubstitute == null)
            {
                Console.WriteLine("Member <PathToSubstitute> not set!");
                return false;
            }

            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_pathToSubstitute);
            RemoveFileOnIgnoreList();

            foreach (TranslationFile translationFile in LocalisationFilesGerman)
            {
                FileSubstitutor fileSubstitutor = new FileSubstitutor();
                if( true == fileSubstitutor.Substitute(translationFile) )
                {
                    Console.WriteLine("Substitution successfully!");
                    if( false == FileUtility.WriteEmptyFileUTF8_BOM( SubstitutionHelper.CreateFileNameResub(translationFile.FileName) ) )
                    {
                        Console.WriteLine("Unable to create file: " + SubstitutionHelper.CreateFileNameResub(translationFile.FileName) );
                    }
                }
            }

            return true;
        }

        private void RemoveFileOnIgnoreList()
        {
            if( null == _translationFileToIgnore )
            {
                return;
            }

            LocalisationFilesGerman.Remove( LocalisationFilesGerman.First( x => x.FileNameWithoutLocalisation.Equals(_translationFileToIgnore)));
        }
    }
}
