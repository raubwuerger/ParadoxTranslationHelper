using ParadoxTranslationHelper.Helper;
using ParadoxTranslationHelper.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectValidate : FunctionObjectBase
    {
        string _pathGerman;
        string _pathSteam;

        public string PathGerman { get => _pathGerman; set => _pathGerman = value; }
        public string PathSteam { get => _pathSteam; set => _pathSteam = value; }

        public FunctionObjectValidate(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            if (true == string.IsNullOrEmpty(_pathGerman))
            {
                Log.Verbose("Member <PathGerman> must not be null or empty!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_pathSteam))
            {
                Log.Verbose("Member <PathSteam> must not be null or empty!");
                return false;
            }

            LocalisationFilesSteam = FileUtility.CreateTranslationFilesFromDirectory(_pathSteam);
            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_pathGerman);

            Sort();

            return true;
        }

        private void Sort()
        {
            foreach( TranslationFile translationFile in LocalisationFilesSteam )
            {
                TranslationFile? found = FunctionUtility.FindCorrespondingTranslationFile(LocalisationFilesGerman, translationFile);
                if( null == found )
                {
                    Log.Warning($"Corresponding german translation file not found! {translationFile.FileNameWithBasePath}");
                    continue;
                }

                MultipleKeyRemover multipleKeyRemover = new MultipleKeyRemover();
                Dictionary<int, LineObject> withoutMultipleKeys = multipleKeyRemover.RemoveMultipleKeys(translationFile.Lines);


                KeySorter keySorter = new KeySorter();
                keySorter.OriginalKeys = translationFile.Lines;
                keySorter.TranslatedKeys = found.Lines;
                if( false == keySorter.Sort() )
                {
                    Log.Warning("Keys mismatch");
                }

                found.Lines = keySorter.SortedKeys;
                BackupOriginalFile(found);
                SaveSortedFile(found);
            }

        }

        private bool SaveSortedFile( TranslationFile translationFile )
        {
            return FileUtility.Write(translationFile, translationFile.FileNameWithBasePath);
        }

        private bool BackupOriginalFile( TranslationFile translationFile )
        {
            return FileUtility.BackupFile(translationFile.FileNameWithBasePath);
        }
    }
}
