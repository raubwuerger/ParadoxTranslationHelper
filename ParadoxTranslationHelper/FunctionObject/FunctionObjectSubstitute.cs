using ParadoxTranslationHelper.Utilities;
using System;
using Serilog;
using System.Linq;
using System.IO;

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
                Log.Warning("Member <PathToSubstitute> must not be null!");
                return false;
            }

            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_pathToSubstitute);
            if( null == LocalisationFilesGerman )
            {
                Log.Warning($"No files found in path {_pathToSubstitute}");
                return false;
            }
            
            RemoveFileOnIgnoreList();

            foreach (TranslationFile translationFile in LocalisationFilesGerman)
            {
                FileSubstitutor fileSubstitutor = new FileSubstitutor();
                if( true == fileSubstitutor.Substitute(translationFile) )
                {
                    Log.Verbose("Substitution successfully!");
                    if( false == FileUtility.WriteEmptyFileUTF8_BOM( SubstitutionHelper.CreateFileNameResub(translationFile.FileNameWithBasePath) ) )
                    {
                        Log.Warning("Unable to create file: " + SubstitutionHelper.CreateFileNameResub(translationFile.FileNameWithBasePath) );
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

            if( null == LocalisationFilesGerman )
            {
                return;
            }

            if( false == File.Exists(_translationFileToIgnore) )
            {
                return;
            }

            TranslationFile toRemove;
            try
            {
                toRemove = LocalisationFilesGerman.First(x => x.FileNameWithoutLocalisation.Equals(_translationFileToIgnore, StringComparison.CurrentCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString());
                return;
            }

            LocalisationFilesGerman.Remove(toRemove);
        }
    }
}
