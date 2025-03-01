using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Validators
{
    public class TranslationFileValidator
    {
        public static bool IsValid( TranslationFile translationFile )
        {
            if ( translationFile == null )
            {
                Console.WriteLine("Parameter <translationFile> must not be null!");
                return false;
            }

            if( true == string.IsNullOrEmpty( translationFile.FileName) )
            {
                Console.WriteLine("Member <FileName> must not be null or empty!");
                return false;
            }

            if (true == string.IsNullOrEmpty(translationFile.FileNameWithoutLocalisation))
            {
                Console.WriteLine("Member <FileNameWithoutLocalisation> must not be null or empty!");
                return false;
            }

            return false;
        }
    }
}
