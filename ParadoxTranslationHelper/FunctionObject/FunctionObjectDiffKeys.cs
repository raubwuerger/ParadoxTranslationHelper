using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectDiffKeys : FunctionObjectBase
    {
        private string _localizationFilePathSteam;
        private string _localizationFilePathGerman;
        private string _localizationFilePathAnalyze;

        public string LocalizationFilePathGerman { get => _localizationFilePathGerman; set => _localizationFilePathGerman = value; }
        public string LocalizationFilePathAnalyze { get => _localizationFilePathAnalyze; set => _localizationFilePathAnalyze = value; }
        public string LocalizationFilePathSteam { get => _localizationFilePathSteam; set => _localizationFilePathSteam = value; }

        public FunctionObjectDiffKeys(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            if (true == string.IsNullOrEmpty(_localizationFilePathSteam))
            {
                Console.WriteLine("Member <LocalizationFilePathSteam> must not be null!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_localizationFilePathGerman))
            {
                Console.WriteLine("Member <LocalizationFilePathGerman> must not be null!");
                return false;
            }

/*            if (true == string.IsNullOrEmpty(_localizationFilePathAnalyze))
            {
                Console.WriteLine("Member <LocalizationFilePathAnalyze> must not be null!");
                return false;
            }
*/
            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_localizationFilePathGerman);
            if( LocalisationFilesGerman == null )
            {
                return false;
            }

            if( LocalisationFilesGerman.Count == 0 )
            {
                Console.WriteLine("Path contains no files:" + _localizationFilePathGerman);
                return false;
            }

            LocalisationFilesSteam = FileUtility.CreateTranslationFilesFromDirectory(_localizationFilePathSteam);
            if (LocalisationFilesSteam == null)
            {
                return false;
            }

            if (LocalisationFilesSteam.Count == 0)
            {
                Console.WriteLine("Path contains no files:" + _localizationFilePathSteam);
                return false;
            }

            DiffKeys();

            return true;
        }

        private void DiffKeys() 
        {
           foreach( TranslationFile translationFile in LocalisationFilesSteam ) 
           {
                DiffKeys(translationFile, FunctionUtility.FindCorrespondingTranslationFile(LocalisationFilesGerman, translationFile));
           }
        }

        private void DiffKeys( TranslationFile org, TranslationFile toVerify )
        {
            if( org == null ) 
            {
                Console.WriteLine("Parameter <org> must not be null!");
                return;
            }

            if (org == null)
            {
                Console.WriteLine("Parameter <toVerify> must not be null!");
                return;
            }

            foreach( LineObject line in org.Lines.Values.ToList() ) 
            {
                if( false == DiffKeys(line, FunctionUtility.FindCorrespondingLineObject(toVerify.Lines.Values.ToList(), line)) )
                {
                    Console.WriteLine("Key not found: " + line.Key);
                }
            }
        }

        private bool DiffKeys( LineObject org, LineObject toVerify )
        {
            if (org == null)
            {
                Console.WriteLine("Parameter <org> must not be null!");
                return false;
            }

            if (toVerify == null)
            {
                Console.WriteLine("Parameter <toVerify> must not be null!");
                return false;
            }

            int orgColorCodes = org.ColorCodes.Count;
            int toVerifyColorCodes = toVerify.ColorCodes.Count;

            return true;
        }

    }
}
