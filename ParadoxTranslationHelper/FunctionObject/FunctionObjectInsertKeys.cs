using ParadoxTranslationHelper.Utilities;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectInsertKeys : FunctionObjectBase
    {
        private string _localizationFileNameKeysToCreate;
        private string _localizationFilePathGerman;
        private string _localizationFilePathAnalyze;

        public string LocalizationFileNameKeysToCreate { get => _localizationFileNameKeysToCreate; set => _localizationFileNameKeysToCreate = value; }
        public string LocalizationFilePathGerman { get => _localizationFilePathGerman; set => _localizationFilePathGerman = value; }
        public string LocalizationFilePathAnalyze { get => _localizationFilePathAnalyze; set => _localizationFilePathAnalyze = value; }

        public FunctionObjectInsertKeys(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            if (true == string.IsNullOrEmpty(_localizationFileNameKeysToCreate))
            {
                Log.Debug("Member <LocalizationFileNameKeysToCreate> must not be null!");
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

            if( false == File.Exists(_localizationFilePathGerman)) 
            {
                Log.Warning("File doesn't exist: " + _localizationFilePathGerman);
                return false;
            }

            List<TranslationFile> keysToInsert = FunctionUtility.LoadFileAndCreateKeys(_localizationFileNameKeysToCreate);
            if( keysToInsert == null )
            {
                return false;
            }

            if( keysToInsert.Count == 0 )
            { 
                Log.Warning("File doesn't contain keys to insert: " + _localizationFileNameKeysToCreate);
                return false; 
            }   

            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_localizationFilePathGerman);
            if( LocalisationFilesGerman == null )
            {
                return false;
            }

            if( LocalisationFilesGerman.Count == 0 )
            {
                Log.Warning("Path contains no files:" + _localizationFilePathGerman);
                return false;
            }

            List<TranslationFile> updatedFiles = CreateUpdateFiles(keysToInsert);
            foreach (TranslationFile file in updatedFiles) 
            {
                FileUtility.WriteLines(file.Lines.Values.ToList<LineObject>(), file.FileName);
            }

            return true;
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

            string fileName = missing.FileName;

            FileUtility.Write(original, Path.Combine(LocalizationFilePathAnalyze, original.FileNameWithoutLocalisation +Constants.LOCALISATION_GERMAN_FULL + Constants.FILE_BACKUP_EXTENSION));

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

            return original;
        }

        private List<TranslationFile> CreateUpdateFiles( List<TranslationFile> missingKeysToInsert)
        {
            List<TranslationFile> updatedFiles = new List<TranslationFile>();
            foreach (TranslationFile translationFile in missingKeysToInsert)
            {
                TranslationFile translationKeysToInsert = LocalisationFilesGerman.Find(x => x.FileNameWithoutLocalisation.Equals(translationFile.FileNameWithoutLocalisation));
                if (translationKeysToInsert == null)
                {
                    Log.Warning("File to insert not found! " + translationFile.FileName);
                }
                updatedFiles.Add(InsertInto(translationKeysToInsert, translationFile));
            }

            return updatedFiles;
        }

        private TranslationFile CreateMissingTranslationFile( TranslationFile missing )
        {
            Dictionary<int, LineObject> includingLanguageIdentifier = new Dictionary<int, LineObject>
            {
                { 1, FunctionUtility.CreateLineObjectLanguageIdentifier(missing) }
            };

            foreach ( KeyValuePair<int, LineObject> keyValuePair in missing.Lines )
            {
                int newLineNumber = includingLanguageIdentifier.Count + 1;
                includingLanguageIdentifier.Add( newLineNumber, new LineObject( newLineNumber, keyValuePair.Value ) );
            }

            missing.Lines = includingLanguageIdentifier;

            TranslationFileCreator translationFileCreator = new TranslationFileCreator();
            return translationFileCreator.CopyExceptFileName( Path.Combine(_localizationFilePathGerman, Utility.ConvertLocalisationToGerman(missing.FileName)), missing );
        }

        private bool RemoveTranslationFileIdentifier(Dictionary<int, LineObject> lines ) 
        {
            if(lines == null )
            {
                Log.Debug("Parameter <lines> must not be null!");
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
