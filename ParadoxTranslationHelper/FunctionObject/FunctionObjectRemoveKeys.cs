using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectRemoveKeys : FunctionObjectBase
    {
        private string _localizationFileNameKeysToDelete;
        private string _localizationFilePathGerman;
        private string _localizationFilePathAnalyze;

        public string LocalizationFileNameKeysToDelete { get => _localizationFileNameKeysToDelete; set => _localizationFileNameKeysToDelete = value; }
        public string LocalizationFilePathGerman { get => _localizationFilePathGerman; set => _localizationFilePathGerman = value; }
        public string LocalizationFilePathAnalyze { get => _localizationFilePathAnalyze; set => _localizationFilePathAnalyze = value; }

        public FunctionObjectRemoveKeys(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            if (true == string.IsNullOrEmpty(_localizationFileNameKeysToDelete))
            {
                Log.Debug("Member <LocalizationFileNameKeysToDelete> must not be null!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_localizationFilePathGerman))
            {
                Log.Debug("Member <LocalizationFilePathGerman> must not be null!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_localizationFilePathAnalyze))
            {
                Log.Debug("Member <LocalizationFilePathAnalyze> must not be null!");
                return false;
            }

            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_localizationFilePathGerman);
            List<TranslationFile> translationFilesKeysToDelete = CreateTranslationFilesKeysToDelete(_localizationFileNameKeysToDelete);


            List<TranslationFile> updatedFiles = CreateUpdateFiles(translationFilesKeysToDelete);
            foreach (TranslationFile file in updatedFiles) 
            {
                BackupOriginalFile(FindFileByNameWithoutLocalosation(file.FileNameWithoutLocalisation));
                WriteOriginalWithoutKeysToDelete(file);
            }

            return true;
        }

        private List<TranslationFile>? CreateTranslationFilesKeysToDelete( string fileNamekeysToDelete ) 
        {
            if( string.IsNullOrWhiteSpace( fileNamekeysToDelete ) ) 
            {
                Log.Debug("Parameter <fileNamekeysToDelete> must not be null or empty!");
                return null; 
            }

            List<TranslationFile> translationFilesKeysToDelete = new List<TranslationFile>();
            TranslationFileCreator translationFileCreator = new TranslationFileCreator();

            List<string> lines = Utility.ConvertToList(File.ReadAllLines(fileNamekeysToDelete));
            List<string> foundFile = new List<string>();
            foreach ( string line in lines ) 
            {
                if (false == foundFile.Any() && true == FunctionUtility.ContainsFileName(line) ) 
                {
                    string fileName = FunctionUtility.CreateFileName( line );
                    if( true == string.IsNullOrEmpty(fileName) )
                    {
                        continue;
                    }

                    foundFile.Add(fileName);
                    continue;
                }

                if( true == foundFile.Any() && false == FunctionUtility.ContainsFileName(line) ) 
                {
                    foundFile.Add(line);
                    continue;
                }

                if ( true == FunctionUtility.ContainsFileName(line) )
                {
                    translationFilesKeysToDelete.Add(translationFileCreator.Create(foundFile));
                    foundFile = new List<string>();
                    string fileName = FunctionUtility.CreateFileName(line);
                    if( true == string.IsNullOrEmpty(fileName) )
                    {
                        continue;
                    }

                    foundFile.Add(fileName);
                }
            }

            if( true == foundFile.Any()) 
            {
                translationFilesKeysToDelete.Add(translationFileCreator.Create(foundFile));
            }

            return translationFilesKeysToDelete;
        }

        private List<TranslationFile> CreateUpdateFiles( List<TranslationFile> translationFilesKeysToDelete)
        {
            List<TranslationFile> updatedFiles = new List<TranslationFile>();
            foreach (TranslationFile translationFile in translationFilesKeysToDelete)
            {
                TranslationFile originalFile = FindFileByNameWithoutLocalosation(translationFile.FileNameWithoutLocalisation);
                if( originalFile == null ) 
                {
                    Log.Debug("Original file not found: " + translationFile.FileNameWithoutLocalisation);
                    continue;
                }

                TranslationFile translationWithRemovedKeys = RemoveKeysFromOriginal(originalFile, translationFile.Lines);
                if( null == translationWithRemovedKeys ) 
                {
                    continue;
                }

                updatedFiles.Add(translationWithRemovedKeys);
            }

            return updatedFiles;
        }

        private TranslationFile RemoveKeysFromOriginal(TranslationFile original, Dictionary<int, LineObject> keysToRemove)
        {
            if (null == original)
            {
                Log.Debug("Parameter <original> must not be null!");
                return null;
            }

            if (null == keysToRemove)
            {
                Log.Debug("Parameter <keysToRemove> must not be null!");
                return null;
            }

            if (keysToRemove.Count == 0)
            {
                Log.Debug("Parameter <keysToRemove> must not be empty!");
                return null;
            }

            TranslationFile originalWithRemovedKeys = new TranslationFile( original );

            foreach (KeyValuePair<int, LineObject> line in keysToRemove)
            {
                KeyValuePair<int, LineObject> found;
                try
                {
                    found = originalWithRemovedKeys.Lines.First(KeyValuePair => KeyValuePair.Value.Key.Equals(line.Value.Key));
                }
                catch (Exception ex)
                {
                    Log.Fatal("Key not found! " + line.Value.Key);
                    continue;
                }
                originalWithRemovedKeys.Lines.Remove(found.Key);
            }

            return originalWithRemovedKeys;
        }

        private TranslationFile FindFileByNameWithoutLocalosation(string fileNameWithoutLocalisation)
        {
            return LocalisationFilesGerman.Find(x => x.FileNameWithoutLocalisation.Equals(fileNameWithoutLocalisation));
        }

        private void WriteOriginalWithoutKeysToDelete(TranslationFile translationFile)
        {
            FileUtility.Write(translationFile);
        }
        
        private void BackupOriginalFile(TranslationFile translationFile)
        {
            FileUtility.Write(translationFile, translationFile.FileName + Constants.FILE_BACKUP_EXTENSION);
        }


    }
}
