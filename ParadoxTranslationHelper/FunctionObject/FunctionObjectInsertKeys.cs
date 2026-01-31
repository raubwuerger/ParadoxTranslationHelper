using ParadoxTranslationHelper.Utilities;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectInsertKeys : FunctionObjectBase
    {
        private string _localisationFileNameKeysToCreate;
        private string _localisationFilePathGerman;
        private string _localisationFilePathAnalyze;
        private string _localisationFilePathSteam;

        public string LocalisationFileNameKeysToCreate { get => _localisationFileNameKeysToCreate; set => _localisationFileNameKeysToCreate = value; }
        public string LocalisationFilePathGerman { get => _localisationFilePathGerman; set => _localisationFilePathGerman = value; }
        public string LocalisationFilePathAnalyze { get => _localisationFilePathAnalyze; set => _localisationFilePathAnalyze = value; }
        public string LocalisationFilePathSteam { get => _localisationFilePathSteam; set => _localisationFilePathSteam = value; }

        public FunctionObjectInsertKeys(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            if (true == string.IsNullOrEmpty(_localisationFileNameKeysToCreate))
            {
                Log.Verbose("Member <LocalisationFileNameKeysToCreate> must not be null!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_localisationFilePathGerman))
            {
                Log.Verbose("Member <LocalisationFilePathGerman> must not be null!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_localisationFilePathAnalyze))
            {
                Log.Verbose("Member <LocalisationFilePathAnalyze> must not be null!");
                return false;
            }

            if( false == Directory.Exists(_localisationFilePathGerman)) 
            {
                Log.Warning("File doesn't exist: " + _localisationFilePathGerman);
                return false;
            }

            List<TranslationFile> keysToInsert = FunctionUtility.LoadFileAndCreateKeys(_localisationFileNameKeysToCreate);
            if( keysToInsert == null )
            {
                return false;
            }

            if( keysToInsert.Count == 0 )
            { 
                Log.Warning("File doesn't contain keys to insert: " + _localisationFileNameKeysToCreate);
                return false; 
            }   

            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_localisationFilePathGerman);
            if( LocalisationFilesGerman == null )
            {
                Log.Warning($"Directory contains no translation files: {_localisationFilePathGerman}");
                return false;
            }

            List<TranslationFile> updatedFiles = CreateUpdateFiles(keysToInsert);
            foreach (TranslationFile file in updatedFiles) 
            {
                FileUtility.WriteLines(file.Lines.Values.ToList<LineObject>(), file.FileNameWithBasePath);
            }

            return true;
        }

        private List<TranslationFile> CreateUpdateFiles(List<TranslationFile> missingKeysToInsert)
        {
            List<TranslationFile> updatedFiles = new List<TranslationFile>();
            foreach (TranslationFile translationFile in missingKeysToInsert)
            {
                TranslationFile translationKeysToInsert = LocalisationFilesGerman.Find(x => x.FileNameWithoutLocalisation.Equals(translationFile.FileNameWithoutLocalisation));
                if (translationKeysToInsert == null)
                {
                    Log.Warning("File to insert not found! " + translationFile.FileNameWithBasePath);
                    translationKeysToInsert = CreateMissingTranslationFile(translationFile);
                    if (null == translationKeysToInsert)
                    {
                        continue;
                    }
                    AddMissingTranslationKeys(translationKeysToInsert, translationFile.Lines);
                }

                TranslationFile toInsert = InsertInto(translationKeysToInsert, translationFile);
                if (null == toInsert)
                {
                    continue;
                }

                updatedFiles.Add(toInsert);
            }

            return updatedFiles;
        }


        private TranslationFile InsertInto( TranslationFile original,  TranslationFile missing )
        {
            if( null == original )
            {
                return CreateMissingTranslationFile(missing);
            }

            if( null == missing )
            {
                return null;
            }

            if(missing.Lines == null ) 
            {
                Log.Warning("Dictionary toInsert is null!");
                return null;
            }

            if(original.Lines == null ) 
            {
                Log.Warning("Dictionary original is null!");
                return null;
            }

            string fileName = missing.FileNameWithBasePath;

            FileUtility.Write(original, Path.Combine(LocalisationFilePathAnalyze, original.FileNameWithoutLocalisation +Constants.LOCALISATION_GERMAN_FULL + Constants.EXTENSION_FILE_BACKUP));

            RemoveTranslationFileIdentifier(original.Lines);

            foreach ( KeyValuePair<int, LineObject> line in missing.Lines ) 
            {
                if ( line.Value.OriginalLine.Contains(Constants.TRANSLATION_FILE_IDENTIFIER) )
                {
                    continue;
                }
                int newLineNumber = original.Lines.Count + 1;
                original.Lines.Add( newLineNumber, new LineObject( newLineNumber, line.Value ) );
            }

            Log.Information($"Added {missing.Lines.Count} lines to file {original.FileName}");
            return original;
        }

        private TranslationFile CreateMissingTranslationFile( TranslationFile missing )
        {
            string fileNameOnly = Path.GetFileName(missing.FileNameWithBasePath);
            string filePath = Path.Combine(_localisationFilePathGerman, fileNameOnly.Replace(Constants.LOCALISATION_ENGLISH_FULL, Constants.LOCALISATION_GERMAN_FULL));
            TranslationFile toCreate = TranslationFileCreator.CreateEmpty(filePath);
            toCreate.Lines.Add( 0, LineObjectCreator.CreateLineObjectLanguageIdentifierGerman());
            if ( false == FileUtility.Write(toCreate) )
            {
                Log.Warning("Unable to create file! " + toCreate.FileNameWithBasePath);
                return null;
            }
            Log.Information("Created file: " + toCreate.FileNameWithBasePath);
            return toCreate;
        }

        private void AddMissingTranslationKeys(TranslationFile translationKeysToInsert, Dictionary<int, LineObject> lines )
        {
            if( null == translationKeysToInsert )
            {
                return;
            }
            
            if( lines == null )
            {
                return;
            }

            foreach( KeyValuePair<int,LineObject> keyValuePair in lines )
            {
                translationKeysToInsert.Lines.Add(keyValuePair.Key, new LineObject(keyValuePair.Value.LineNumber + 1, keyValuePair.Value));
            }
        }
        private bool RemoveTranslationFileIdentifier(Dictionary<int, LineObject> lines ) 
        {
            if(lines == null )
            {
                Log.Verbose("Parameter <lines> must not be null!");
                return false;
            }

            foreach( KeyValuePair<int, LineObject> lineObject in lines )
            {
                if( lineObject.Value.OriginalLine.Contains(Constants.TRANSLATION_FILE_IDENTIFIER) )
                {
                    lines.Remove(lineObject.Key);
                }
            }

            return true;
        }
    }
}
