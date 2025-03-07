using ParadoxTranslationHelper.Helper;
using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.FunctionObject
{
    public class FunctionObjectDiffSteam : FunctionObjectBase
    {
        string _pathSteam;
        string _pathGerman;
        string _fileNameMissingKeys;
        string _fileNameNoMissingKeysFound = "SteamDiff_NoMissingKeysFound.txt";
        string _fileNameKeysToDelete;
        string _fileNameNoKeysToDeleteFound = "SteamDiff_NoKeysToDeleteFound.txt";
        public FunctionObjectDiffSteam(string name) : base(name)
        {
        }

        public string PathSteam { get => _pathSteam; set => _pathSteam = value; }
        public string PathGerman { get => _pathGerman; set => _pathGerman = value; }
        public string FileNameMissingKeys { get => _fileNameMissingKeys; set => _fileNameMissingKeys = value; }
        public string FileNameKeysToDelete { get => _fileNameKeysToDelete; set => _fileNameKeysToDelete = value; }

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

            if (true == string.IsNullOrEmpty(_fileNameMissingKeys))
            {
                Console.WriteLine("Member <FileNameMissingKeys> must not be null or empty!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_fileNameKeysToDelete))
            {
                Console.WriteLine("Member <FileNameKeysToDelete> must not be null or empty!");
                return false;
            }

            LocalisationFilesSteam = FileUtility.CreateTranslationFilesFromDirectory(_pathSteam);
            if (null == LocalisationFilesSteam)
            {
                Console.WriteLine("Steam path not set!");
                return false;
            }

            if( LocalisationFilesSteam.Count == 0 )
            {
                Console.WriteLine("Steam path containes no files: " + _pathSteam);
                return false;
            }

            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_pathGerman);
            if( null == LocalisationFilesGerman )
            {
                return false;
            }

            if (LocalisationFilesGerman.Count == 0)
            {
                return false;
            }

            return AnalyzeKeys();
        }

        protected bool AnalyzeKeys()
        {
            RemoveFilesNoLongerInSteamExistent(CreateFilesNoLongerInSteamExistent());

            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_pathGerman);
            List<TranslationFile> translationFilesToRemove = LocalisationFilesGerman
                .ExceptBy(
                    LocalisationFilesSteam.Select(locFilesSteam => locFilesSteam.FileNameWithoutLocalisation.ToUpper()),
                    locFilesGerman => locFilesGerman.FileNameWithoutLocalisation.ToUpper())
                .ToList();

            Dictionary<string, LineObject> steam = Utility.ExtractKeys(LocalisationFilesSteam);
            Dictionary<string, LineObject> repository = Utility.ExtractKeys(LocalisationFilesGerman);

            CreateFileKeys(FunctionUtility.FindToCreate(steam, repository), _fileNameKeysToDelete);
            CreateFileKeys(FunctionUtility.FindToCreate(repository, steam), _fileNameMissingKeys);

            return true;
        }

        private bool CreateFileKeys(Dictionary<string, LineObject> keys, string fileName )
        {
            string directory = FileUtility.CreateDirectoryAnalysis();
            if (null == directory)
            {
                Console.WriteLine("Unable to create directory! " + directory);
                return false;
            }

            if (keys.Values.Count > 0)
            {
                FileUtility.WriteLinesPushFrontTranslationIdentifier(keys.Values.ToList(), Path.Combine(directory, fileName));
            }
            else
            {
                FileUtility.WriteEmptyFileUTF8_BOM(Path.Combine(directory, fileName));
            }
            return true;
        }

        private List<TranslationFile>? CreateFilesNoLongerInSteamExistent()
        {
            List<TranslationFile> translationFilesToRemove;
            try
            {
                translationFilesToRemove = LocalisationFilesGerman
                    .ExceptBy(
                    LocalisationFilesSteam.Select(locFilesSteam => locFilesSteam.FileNameWithoutLocalisation.ToUpper()),
                    locFilesGerman => locFilesGerman.FileNameWithoutLocalisation.ToUpper())
                    .ToList();
                return translationFilesToRemove;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private void RemoveFilesNoLongerInSteamExistent( List<TranslationFile>? filesToRemove )
        {
            if( filesToRemove == null ) 
            {
                return;
            }

            const string fileToRemove = ".toRemove";

            foreach(TranslationFile file in filesToRemove ) 
            {
                string fileNameToRemove = file.FileName + fileToRemove;
                if ( File.Exists(fileNameToRemove) )
                {
                    File.Delete(fileNameToRemove);
                }
                File.Move(file.FileName, fileNameToRemove);
            }
        }

    }
}
