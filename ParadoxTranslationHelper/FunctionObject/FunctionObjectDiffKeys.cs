using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using Serilog;
using System.Linq;

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
                Log.Debug("Member <LocalizationFilePathSteam> must not be null!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_localizationFilePathGerman))
            {
                Log.Debug("Member <LocalizationFilePathGerman> must not be null!");
                return false;
            }

/*            if (true == string.IsNullOrEmpty(_localizationFilePathAnalyze))
            {
                Log.Debug("Member <LocalizationFilePathAnalyze> must not be null!");
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
                Log.Debug("Path contains no files:" + _localizationFilePathGerman);
                return false;
            }

            LocalisationFilesSteam = FileUtility.CreateTranslationFilesFromDirectory(_localizationFilePathSteam);
            if (LocalisationFilesSteam == null)
            {
                return false;
            }

            if (LocalisationFilesSteam.Count == 0)
            {
                Log.Debug("Path contains no files:" + _localizationFilePathSteam);
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
                Log.Debug("Parameter <org> must not be null!");
                return;
            }

            if (org == null)
            {
                Log.Debug("Parameter <toVerify> must not be null!");
                return;
            }

            foreach( LineObject line in org.Lines.Values.ToList() ) 
            {
                if( false == DiffKeys(line, FunctionUtility.FindCorrespondingLineObject(toVerify.Lines.Values.ToList(), line)) )
                {
                    Log.Information("Key not found: " + line.Key);
                }
            }
        }

        private bool DiffKeys( LineObject org, LineObject toVerify )
        {
            if (org == null)
            {
                Log.Debug("Parameter <org> must not be null!");
                return false;
            }

            if (toVerify == null)
            {
                Log.Debug("Parameter <toVerify> must not be null!");
                return false;
            }

            List<string> orgCopy = org.ColorCodes.ConvertAll( x => String.Copy(x) );
            List<string> toVerifyCopy = toVerify.ColorCodes.ConvertAll( x => String.Copy(x) );

            foreach (string item in toVerify.ColorCodes)
            {
                if( false == orgCopy.Contains(item) )
                {
                    continue;
                }
                orgCopy.Remove(item);
                toVerifyCopy.Remove(item);
            }

            Log.Information("ColorCodes not found in toVerify: " +orgCopy.ToArray().ToString() );
            Log.Information("ColorCodes wrong in toVerify: " + toVerifyCopy.ToArray().ToString());

            return true;
        }

    }
}
