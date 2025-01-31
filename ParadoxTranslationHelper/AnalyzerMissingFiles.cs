using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class AnalyzerMissingFiles
    {
        public static List<TranslationFile> GenerateMissingGermanTranslationFiles( List<TranslationFile> filesGerman, List<TranslationFile> filesEnglish )
        {
            if(filesGerman == null )
            {
                Console.WriteLine("Parameter <filesGerman> must not be null!");
                return null;
            }

            if(filesEnglish == null)
            {
                Console.WriteLine("Parameter <filesEnglish> must not be null!");
                return null;
            }

            List<string> localisationFileNamesEnglish = filesEnglish.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> localisationFileNamesGerman = filesGerman.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> localisationFileNamesGermanMissing = localisationFileNamesEnglish.Except(localisationFileNamesGerman).ToList<string>();

            List<TranslationFile> missingTranslationFilesEnglishOriginal = new List<TranslationFile>();
            List<TranslationFile> missingTranslationFilesGerman = new();
            foreach (string file in localisationFileNamesGermanMissing)
            {
                TranslationFile translation = filesEnglish.Find(x => x.FileNameWithoutLocalisation == file);
                if (translation == null)
                {
                    continue;
                }

                missingTranslationFilesGerman.Add(Utility.ConvertToGerman(translation));
                missingTranslationFilesEnglishOriginal.Add(translation);
            }

            return CreateTranslationFileList(localisationFileNamesGermanMissing,filesGerman);
        }

        private static List<TranslationFile> CreateTranslationFileList( List<string> missingGermanTranslationFile, List<TranslationFile> filesGerman )
        {
            List<TranslationFile> translationFiles = new List<TranslationFile>();

            foreach( string file in missingGermanTranslationFile )
            {
                continue;
            }

            return translationFiles;
        }
    }
}
