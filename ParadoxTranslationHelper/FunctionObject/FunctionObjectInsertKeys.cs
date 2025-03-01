using ParadoxTranslationHelper.Utilities;
using System;
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
                Console.WriteLine("Member <LocalizationFileNameKeysToCreate> must not be null!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_localizationFilePathGerman))
            {
                Console.WriteLine("Member <LocalizationFilePathGerman> must not be null!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_localizationFilePathAnalyze))
            {
                Console.WriteLine("Member <LocalizationFilePathAnalyze> must not be null!");
                return false;
            }

            List<TranslationFile> keysToInsert = FunctionUtility.CreateKeys(_localizationFileNameKeysToCreate);

            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_localizationFilePathGerman);

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
                Console.WriteLine("Dictionary toInsert is null!");
                return null;
            }

            if(original.Lines == null ) 
            {
                Console.WriteLine("Dictionary original is null!");
                return null;
            }

            FileUtility.WriteTranslationFile(original, Path.Combine(LocalizationFilePathAnalyze, original.FileNameWithoutLocalisation +Constants.LOCALISATION_GERMAN_FULL + Constants.FILE_BACKUP_EXTENSION));

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
                updatedFiles.Add(InsertInto(LocalisationFilesGerman.Find(x => x.FileNameWithoutLocalisation.Equals(translationFile.FileNameWithoutLocalisation)), translationFile));
            }

            return updatedFiles;
        }

        private LineObject CreateLineObjectLanguageIdentifier(TranslationFile missing)
        {
            LineObject languageIdentifier = new LineObject(1);
            languageIdentifier.OriginalLine = Constants.LOCALISATION_GERMAN_FILE_IDENTIFIER;
            languageIdentifier.TranslationFile = missing;
            return languageIdentifier;
        }

        private TranslationFile CreateMissingTranslationFile( TranslationFile missing )
        {
            Dictionary<int, LineObject> includingLanguageIdentifier = new Dictionary<int, LineObject>
            {
                { 1, CreateLineObjectLanguageIdentifier(missing) }
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
                Console.WriteLine("Parameter <lines> must not be null!");
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
