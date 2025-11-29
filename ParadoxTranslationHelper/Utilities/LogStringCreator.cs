using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Utilities
{
    internal class LogStringCreator
    {
        public static string? Create(LineObject? lineObject)
        {
            if( null == lineObject )
            {
                return null;
            }

            return $"key={lineObject.Key}, lineNumber={lineObject.LineNumber}, fileName={lineObject.TranslationFile.FileName}";
        }
    }
}
