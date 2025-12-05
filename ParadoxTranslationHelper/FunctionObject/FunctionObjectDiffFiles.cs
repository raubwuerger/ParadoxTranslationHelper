using ParadoxTranslationHelper.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;

namespace ParadoxTranslationHelper.FunctionObject
{
    public class FunctionObjectDiffFiles : FunctionObjectBase
    {
        string _pathSteam;
        string _pathGerman;
        string _fileNameMissingKeys;
        string _fileNameNoMissingKeysFound = "Diff_NoMissingKeysFound.txt";
        string _fileNameKeysToDelete;
        string _fileNameNoKeysToDeleteFound = "Diff_NoKeysToDeleteFound.txt";
        public FunctionObjectDiffFiles(string name) : base(name)
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
                Log.Verbose("Member <PathGerman> must not be null or empty!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_pathSteam))
            {
                Log.Verbose("Member <PathSteam> must not be null or empty!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_fileNameMissingKeys))
            {
                Log.Verbose("Member <FileNameMissingKeys> must not be null or empty!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_fileNameKeysToDelete))
            {
                Log.Verbose("Member <FileNameKeysToDelete> must not be null or empty!");
                return false;
            }

            LocalisationFilesSteam = FileUtility.CreateTranslationFilesFromDirectory(_pathSteam);
            if (null == LocalisationFilesSteam)
            {
                Log.Warning("Steam path not set! " + _pathSteam);
                return false;
            }

            Log.Information("Parsing directory: " + _pathSteam);
            if ( LocalisationFilesSteam.Count == 0 )
            {
                Log.Warning("Steam path contains no files: " + _pathSteam);
                return false;
            }

            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_pathGerman);
            if( null == LocalisationFilesGerman )
            {
                return false;
            }

            return DiffFiles();
        }

        protected bool DiffFiles()
        {
            RemoveFilesNoLongerInSteamExisting(CreateFilesNoLongerInSteamExistent());

            CreateFilesMissingLocally(CreateFilesMissingLocally());

            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_pathGerman);

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
                Log.Warning("Unable to create directory! " + directory);
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
            try
            {
                return LocalisationFilesGerman.ExceptBy(
                    LocalisationFilesSteam.Select(locFilesSteam => locFilesSteam.FileNameWithoutLocalisation.ToUpper()),
                    locFilesGerman => locFilesGerman.FileNameWithoutLocalisation.ToUpper())
                    .ToList();
            }
            catch (Exception ex) 
            {
                Log.Fatal(ex.Message);
                return null;
            }
        }
        private void RemoveFilesNoLongerInSteamExisting( List<TranslationFile>? filesToRemove )
        {
            if( filesToRemove == null ) 
            {
                return;
            }

            const string fileToRemove = ".toRemove";

            foreach(TranslationFile file in filesToRemove ) 
            {
                string fileNameToRemove = file.FileNameWithBasePath + fileToRemove;
                if ( File.Exists(fileNameToRemove) )
                {
                    File.Delete(fileNameToRemove);
                }
                File.Move(file.FileNameWithBasePath, fileNameToRemove);
            }
        }

        private List<TranslationFile>? CreateFilesMissingLocally()
        {
            try 
            {
                return LocalisationFilesSteam.ExceptBy(
                        LocalisationFilesGerman.Select(LocalisationFilesGerman => LocalisationFilesGerman.FileNameWithoutLocalisation.ToUpper()),
                        LocalisationFilesSteam => LocalisationFilesSteam.FileNameWithoutLocalisation.ToUpper())
                    .ToList();
            }
            catch(Exception ex) 
            {
                Log.Fatal(ex.Message);
                return null;    
            }
        }

        private void CreateFilesMissingLocally(List<TranslationFile>? translationFiles)
        {
            if (translationFiles == null)
            {
                return;
            }

            foreach (TranslationFile translationFile in translationFiles )
            {
                TranslationFile translationCreated = TranslationFileCreator.CreateEmpty(CreateFilePath(translationFile));

                if ( translationCreated == null )
                {
                    Log.Warning("Unable to create TranslationFile!");
                    continue;
                }
                
                LineObject lineObject = LineObjectCreator.CreateLineObjectLanguageIdentifierGerman();
                if( lineObject == null )
                {
                    Log.Warning("Unable to create LineObject!");
                    continue;
                }

                translationCreated.Lines.Add(lineObject.LineNumber, lineObject);

                if ( false == FileUtility.Write(translationCreated) )
                {
                    Log.Warning("Unable to create file:" +translationCreated.FileNameWithBasePath);
                }
            }
        }

        private string CreateFilePath(TranslationFile translationFile)
        {
            string subPathRelative = Path.GetRelativePath(ParadoxTranslationHelperConfig.PathSteam, translationFile.BasePath);
            if( true == subPathRelative.Equals(".") )
            {
                return Path.Combine(ParadoxTranslationHelperConfig.PathGerman, $"{translationFile.FileNameWithoutLocalisation}{Constants.LOCALISATION_GERMAN_FULL}{Constants.LOCALISATION_EXTENSION}");
            }
            else
            {
                return Path.Combine(ParadoxTranslationHelperConfig.PathGerman, subPathRelative, $"{translationFile.FileNameWithoutLocalisation}{Constants.LOCALISATION_GERMAN_FULL}{Constants.LOCALISATION_EXTENSION}");
            }
        }
    }
}
