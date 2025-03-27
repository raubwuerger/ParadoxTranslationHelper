using Serilog;

namespace ParadoxTranslationHelper.Validators
{
    public class TranslationFileValidator
    {
        public static bool IsValid( TranslationFile translationFile )
        {
            if ( translationFile == null )
            {
                Log.Verbose("Parameter <translationFile> must not be null!");
                return false;
            }

            if( true == string.IsNullOrEmpty( translationFile.FileName) )
            {
                Log.Verbose("Member <FileName> must not be null or empty!");
                return false;
            }

            if (true == string.IsNullOrEmpty(translationFile.FileNameWithoutLocalisation))
            {
                Log.Verbose("Member <FileNameWithoutLocalisation> must not be null or empty!");
                return false;
            }

            return false;
        }
    }
}
