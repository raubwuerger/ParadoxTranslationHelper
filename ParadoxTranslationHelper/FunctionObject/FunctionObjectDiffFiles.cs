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
        string _fileExtensionToDelete;
        public FunctionObjectDiffFiles(string name) : base(name)
        {
        }

        public string PathSteam { get => _pathSteam; set => _pathSteam = value; }
        public string PathGerman { get => _pathGerman; set => _pathGerman = value; }
        public string FileExtensionToDelete { get => _fileExtensionToDelete; set => _fileExtensionToDelete = value; }

        public override bool DoWork()
        {
            if (false == Directory.Exists(_pathGerman))
            {
                Log.Verbose($"Directory <{_pathGerman}> doesn't exist!");
                return false;
            }

            if (false == Directory.Exists(_pathSteam))
            {
                Log.Verbose($"Directory <{_pathSteam}> doesn't exist!");
                return false;
            }

            string[] allfilesSteam = Directory.GetFiles(_pathSteam, "*" + Constants.LOCALISATION_EXTENSION, SearchOption.AllDirectories);
            string[] allfilesGerman = Directory.GetFiles(_pathGerman, "*" + Constants.LOCALISATION_EXTENSION, SearchOption.AllDirectories);

            Log.Debug($"Red count files <{allfilesSteam.Length}> from path {_pathSteam}.");
            Log.Debug($"Red count files <{allfilesGerman.Length}> from path {_pathGerman}.");

            List<string> filesToDelete = DetermineFilesToDelete(allfilesSteam.ToList<string>(), allfilesGerman.ToList<string>());
            List<string> filesToCreate = DetermineFilesToCreate(allfilesSteam.ToList<string>(), allfilesGerman.ToList<string>());

            RenameFilesToDelete(filesToDelete);
            CreateFiles(filesToCreate);

            return true;
        }

        List<string> DetermineFilesToDelete( List<string> filesSteam, List<string> filesGerman)
        {
            List<string> filesToDelete = new List<string>();
            foreach( string fileGerman in filesGerman )
            {
                if (true == filesSteam.Any(sub => sub.Contains(CreateFileNameWithoutLocalisation(fileGerman, Constants.LOCALISATION_GERMAN), StringComparison.OrdinalIgnoreCase)) )
                {
                    continue;
                }
                filesToDelete.Add(fileGerman);
            }
            return filesToDelete;
        }

        string CreateFileNameWithoutLocalisation( string fileName, string localisation )
        {
            return Path.GetFileName(fileName).Replace( localisation + Constants.LOCALISATION_EXTENSION, "");
        }

        List<string> DetermineFilesToCreate(List<string> filesSteam, List<string> filesGerman)
        {
            List<string> filesToCreate = new List<string>();
            foreach (string fileSteam in filesSteam)
            {
                if (true == filesGerman.Any(sub => sub.Contains(CreateFileNameWithoutLocalisation(fileSteam, Constants.LOCALISATION_ENGLISH), StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }
                filesToCreate.Add(fileSteam);
            }
            return filesToCreate;
        }

        bool RenameFilesToDelete( List<string> fileNames )
        {
            try
            {
                foreach (string fileName in fileNames)
                {
                    File.Move(fileName, fileName + Constants.EXTENSION_FILE_TO_DELETE, true);
                    Log.Debug($"Renamed file from {fileName} to {fileName + Constants.EXTENSION_FILE_TO_DELETE}");
                }
                return true;
            }
            catch( Exception ex )
            {
                Log.Warning($"Exception occured: {ex.Message}");
                return false;
            }
        }

        bool CreateFiles( List<string> fileNames )
        {
            try
            {
                foreach (string fileName in fileNames)
                {
                    string fileNameCorrected = CorrectFilePathAndName(fileName);

                    string pathGerman = ParadoxTranslationHelperConfig.PathGerman;

                    string finalPath = Path.Combine(pathGerman, GetSubPath(Path.GetDirectoryName(fileName)));
                    Directory.CreateDirectory(finalPath);
                    string fullFileName = Path.Combine(finalPath, fileNameCorrected);
                    FileUtility.WriteFileUTF8_BOM(fullFileName, Constants.LOCALISATION_GERMAN_FILE_IDENTIFIER );
                    Log.Information($"Created file {fullFileName}");
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Warning($"Exception occured: {ex.Message}");
                return false;
            }
        }

        string GetSubPath( string path )
        {
            int directoryCountPathSteam = ParadoxTranslationHelperConfig.PathSteam.Split(Path.DirectorySeparatorChar).Count();
            int directoryCountPathFile = path.Split(Path.DirectorySeparatorChar).Count();

            if( directoryCountPathSteam == directoryCountPathFile )
            {
                return "";
            }

            return path.Split(Path.DirectorySeparatorChar).Last();
        }

        string CorrectFilePathAndName( string fileName )
        {
            string fileNameOnly = Path.GetFileName(fileName);
            fileNameOnly = fileNameOnly.Replace(Constants.LOCALISATION_ENGLISH, Constants.LOCALISATION_GERMAN);

            return fileNameOnly;
        }

    }
}
