using ParadoxTranslationHelper.SubResubstitution;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.LineObjects
{
    internal static class SubstituteFileHelper
    {
        public static string CreateSubKeyLineTripel(string sub, LineObject lineObject)
        {
            return $"{sub}{Constants.DEFAULT_SEPARATOR}{lineObject.Key}{Constants.DEFAULT_SEPARATOR}{lineObject.LineNumber}";
        }

        public static List<string>? SplitSubKeyLineToParts(string line)
        {
            if( true == string.IsNullOrWhiteSpace(line) )
            {
                Log.Verbose("Parameter <line> must not be null or empty!");
                return null;
            }

            string[] parts = line.Split(Constants.DEFAULT_SEPARATOR);
            if( parts.Length != 4 )
            {
                Log.Verbose($"Parameter <line> splitted by {Constants.DEFAULT_SEPARATOR} must have exactly 4 parts! Is={parts.Length}");
                return null;
            }

            return parts.ToList<string>();
        }

        public static LineObjectSubstitutionFile? Create( List<string> strings )
        {
            if( null == strings )
            {
                Log.Debug("Parameter <strings> must not be null!");
                return null;
            }

            if( strings.Count < 4 )
            {
                Log.Debug("Parameter <strings> must have at least 4 parts!");
                return null;
            }

            if (false == Int32.TryParse(strings[(int)SubstitutionEnums.LineNumber], out int lineNumber) )
            {
                Log.Debug("Parameter <strings[3]> (line number) is not a valid number!");
                return null;
            }

            LineObjectSubstitutionFile lineObject = new LineObjectSubstitutionFile(strings[(int)SubstitutionEnums.Substitute]);
            lineObject.SubstitutedValue = strings[(int)SubstitutionEnums.Original];
            lineObject.Key = strings[(int)SubstitutionEnums.Key];
            lineObject.LineNumber = lineNumber;

            return lineObject;
        }

        public static LineObjectSubstitutionFile? Create( string line )
        {
            return Create(SplitSubKeyLineToParts(line));
        }

    }
}
