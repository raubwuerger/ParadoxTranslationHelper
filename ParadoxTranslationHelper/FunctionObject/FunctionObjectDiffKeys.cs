using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using Serilog;
using System.Linq;
using ParadoxTranslationHelper.Comparator;
using System.IO;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectDiffKeys : FunctionObjectBase
    {
        private string _localisationFilePathSteam;
        private string _localisationFilePathGerman;
        private string _localisationFilePathAnalyze;
        string _fileNameMissingKeys;

        public string LocalisationFilePathGerman { get => _localisationFilePathGerman; set => _localisationFilePathGerman = value; }
        public string LocalisationFilePathAnalyze { get => _localisationFilePathAnalyze; set => _localisationFilePathAnalyze = value; }
        public string LocalisationFilePathSteam { get => _localisationFilePathSteam; set => _localisationFilePathSteam = value; }
        public string FileNameMissingKeys { get => _fileNameMissingKeys; set => _fileNameMissingKeys = value; }


        public FunctionObjectDiffKeys(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            if( true == String.IsNullOrWhiteSpace(_localisationFilePathSteam) )
            {
                Log.Verbose("Member <LocalisationFilePathSteam> must not be null!");
                return false;
            }

            if (false == Directory.Exists(_localisationFilePathSteam))
            {
                Log.Error($"Directory {_localisationFilePathSteam} dosen't exist!");
                return false;
            }

            if (true == String.IsNullOrWhiteSpace(_localisationFilePathGerman))
            {
                Log.Verbose("Member <_localisationFilePathGerman> must not be null!");
                return false;
            }

            if (false == Directory.Exists(_localisationFilePathGerman))
            {
                Log.Error($"Directory {_localisationFilePathGerman} dosen't exist!");
                return false;
            }

            if (false == Directory.Exists(LocalisationFilePathAnalyze))
            {
                Log.Information($"Member <LocalisationFilePathAnalyze> must not be null! --> Creating directory {LocalisationFilePathAnalyze} ...");
                Directory.CreateDirectory(LocalisationFilePathAnalyze);
            }

            LocalisationFilesSteam = FileUtility.CreateTranslationFilesFromDirectory(_localisationFilePathSteam);
            if (LocalisationFilesSteam == null)
            {
                Log.Error($"Unable to read directory {_localisationFilePathSteam}!");
                return false;
            }

            if (LocalisationFilesSteam.Count == 0)
            {
                Log.Error("Path contains no files:" + _localisationFilePathSteam);
                return false;
            }

            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_localisationFilePathGerman);
            if (LocalisationFilesGerman == null)
            {
                Log.Error($"Unable to read directory {_localisationFilePathSteam}!");
                return false;
            }

            if (LocalisationFilesGerman.Count == 0)
            {
                Log.Verbose("Path contains no files:" + _localisationFilePathGerman);
                return false;
            }

            DiffKeys();

            return true;
        }
        protected bool DiffKeys()
        {
            RemoveFilesNoLongerInSteamExisting(CreateFilesNoLongerInSteamExistent());

            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_localisationFilePathGerman);

            Dictionary<string, LineObject> steam = Utility.ExtractKeys(LocalisationFilesSteam);
            Dictionary<string, LineObject> repository = Utility.ExtractKeys(LocalisationFilesGerman);

            CreateFileKeys(FunctionUtility.FindToCreate(repository, steam), _fileNameMissingKeys);

            return true;
        }

        private bool CreateFileKeys(Dictionary<string, LineObject> keys, string fileName)
        {
            if (keys.Values.Count > 0)
            {
                FileUtility.WriteLinesPushFrontTranslationIdentifier(keys.Values.ToList(), Path.Combine(_localisationFilePathAnalyze, fileName));
            }
            else
            {
                FileUtility.WriteEmptyFileUTF8_BOM(Path.Combine(_localisationFilePathAnalyze, fileName));
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
        private void RemoveFilesNoLongerInSteamExisting(List<TranslationFile>? filesToRemove)
        {
            if (filesToRemove == null)
            {
                return;
            }

            foreach (TranslationFile file in filesToRemove)
            {
                string fileNameToRemove = file.FileNameWithBasePath + Constants.EXTENSION_KEYS_TO_REMOVE;
                if (File.Exists(fileNameToRemove))
                {
                    File.Delete(fileNameToRemove);
                }
                File.Move(file.FileNameWithBasePath, fileNameToRemove);
            }
        }

    }
}
