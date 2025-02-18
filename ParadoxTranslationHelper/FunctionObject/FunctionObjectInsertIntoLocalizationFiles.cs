using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectInsertIntoLocalizationFiles : FunctionObjectBase
    {
        private const string _translationFileIdentifier = "##### ";
        public FunctionObjectInsertIntoLocalizationFiles(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            const string pathMissingKeys = "MissingTranslationKeysSteam.yml.sub.resub";
            List<TranslationFile> missingKeysToInsert = CreateMissingKeysToInsert( Path.Combine(ParadoxTranslationHelperConfig.PathResult, pathMissingKeys));
            LocalisationGerman = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathGerman);

            //TODO: 2025-02-17 - JHA - Anhand FileNameWithoutLocalisation die deutsche Übersetzung suchen und fehlende Schlüssel einsetzen, und in andere Datei speichern

            List<TranslationFile> updatedFiles = CreateUpdateFiles(missingKeysToInsert);
            foreach (TranslationFile file in updatedFiles) 
            {
                Utility.WriteLines(file.Lines.Values.ToList<LineObject>(), CreateFileNameResultPathGerman(file.FileNameWithoutLocalisation));
            }

            return true;
        }

        private string? CreateFileNameResultPathGerman(string fileNameWithoutLocalisation )
        {
            if (string.IsNullOrEmpty(fileNameWithoutLocalisation))
            {
                return null;
            }

            return Path.Combine(ParadoxTranslationHelperConfig.PathResult, fileNameWithoutLocalisation + Constants.LOCALISATION_GERMAN_FULL + ".updated.yml");
        }

        private List<TranslationFile>? CreateMissingKeysToInsert( string pathMissingKeys ) 
        {
            if( string.IsNullOrWhiteSpace( pathMissingKeys ) ) 
            {
                Console.WriteLine("Parameter <pathMissingKeys> must not be null or empty!");
                return null; 
            }

            List<TranslationFile> missingKeys = new List<TranslationFile>();
            TranslationFileCreator translationFileCreator = new TranslationFileCreator();

            List<string> lines = CreateLines(File.ReadAllLines(pathMissingKeys));
            List<string> foundFile = new List<string>();
            foreach ( string line in lines ) 
            {
                if (false == foundFile.Any() && true == ContainsFileName(line) ) 
                {
                    string fileName = CreateFileName( line );
                    if( true == string.IsNullOrEmpty(fileName) )
                    {
                        continue;
                    }

                    foundFile.Add(fileName);
                    continue;
                }

                if( true == foundFile.Any() && false == ContainsFileName(line) ) 
                {
                    foundFile.Add(line);
                    continue;
                }

                if ( true == ContainsFileName(line) )
                {
                    missingKeys.Add(translationFileCreator.Create(foundFile));
                    foundFile = new List<string>();
                    string fileName = CreateFileName(line);
                    if( true == string.IsNullOrEmpty(fileName) )
                    {
                        continue;
                    }

                    foundFile.Add(fileName);
                }
            }

            if( true == foundFile.Any()) 
            {
                missingKeys.Add(translationFileCreator.Create(foundFile));
            }

            return missingKeys;
        }

        private List<string> CreateLines(string[] lines )
        {
            if( lines.Length == 0 ) 
            {
                Console.WriteLine("File has no content!");
                return null;
            }

            return new List<string>( lines );
        }

        private bool ContainsFileName( string fileName ) 
        {
            return fileName.Contains( _translationFileIdentifier );
        }

        private string? CreateFileName( string line )
        {
            string fileName = ExtractFileNameFromString(line);
            if (true == string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            return fileName;
        }

        private string? ExtractFileNameFromString( string containsFileName ) 
        {
            if( string.IsNullOrEmpty( containsFileName ) ) 
            {
                return null;
            }

            int indexFileNameStart = containsFileName.IndexOf( _translationFileIdentifier );
            if( indexFileNameStart == -1 ) 
            {
                Console.WriteLine("_translationFileIdentifier not found!");
                return null;
            }

            string fileName = Path.GetFileName(containsFileName.Substring(indexFileNameStart) );
            int startFileExtension = fileName.IndexOf( Constants.LOCALISATION_EXTENSION );

            if( startFileExtension == -1 ) 
            {
                Console.WriteLine("Not a valid localisation file: LOCALISATION_EXTENSION not found!");
                return null;
            }

            return fileName.Remove(startFileExtension + Constants.LOCALISATION_EXTENSION.Length);
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

            foreach( KeyValuePair<int, LineObject> line in missing.Lines ) 
            {
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
                updatedFiles.Add(InsertInto(LocalisationGerman.Find(x => x.FileNameWithoutLocalisation.Equals(translationFile.FileNameWithoutLocalisation)), translationFile));
            }

            return updatedFiles;
        }

        private LineObject CreateLanguageIdentifierLineObject(TranslationFile missing)
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
                { 1, CreateLanguageIdentifierLineObject(missing) }
            };

            foreach ( KeyValuePair<int, LineObject> keyValuePair in missing.Lines )
            {
                int newLineNumber = includingLanguageIdentifier.Count + 1;
                includingLanguageIdentifier.Add( newLineNumber, new LineObject( newLineNumber, keyValuePair.Value ) );
            }

            missing.Lines = includingLanguageIdentifier;

            TranslationFileCreator translationFileCreator = new TranslationFileCreator();
            return translationFileCreator.CopyExceptFileName( Path.Combine(ParadoxTranslationHelperConfig.PathGerman, Utility.ConvertLocalisationToGerman(missing.FileName)), missing );
        }
    }
}
