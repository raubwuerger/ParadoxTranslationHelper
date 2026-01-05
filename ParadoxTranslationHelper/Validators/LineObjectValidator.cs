using Serilog;

namespace ParadoxTranslationHelper.Validators
{
    public static class LineObjectValidator
    {
        public static bool IsValid( LineObject lineObject )
        {
            if (lineObject == null)
            {
                Log.Verbose("Parameter <LineObject> must not be null!");
                return false;
            }

            if( true == IsIgnoreLine(lineObject.OriginalLine) ) 
            {
                return true;
            }

            return true;
        }

        private static bool IsIgnoreLine( string originalLine )
        {
            if (true == string.IsNullOrWhiteSpace(originalLine))
            {
                return true;
            }

            if( true == originalLine.TrimStart().StartsWith('#') )
            {
                return true;
            }

            return false;
        }

        public static bool IsValid( LineObjectSubstitutionFile lineObject )
        {
            if( null == lineObject )
            {
                Log.Verbose("Parameter <LineObjectSubstitutionFile> must not be null!");
                return false;
            }

            if (true == string.IsNullOrWhiteSpace(lineObject.Substitute))
            {
                Log.Verbose("Parameter <LineObjectSubstitutionFile.Substitute> must not be null!");
                return false;
            }

            //TODO: 2026-01-05 - JHA - Eigentlich müsste auch auf Start/Ende Tags geprüft werden!
            if ( lineObject.Substitute.Length != 16 )
            {
                Log.Verbose($"Parameter <LineObjectSubstitutionFile.Substitute> length must be 16! Is {lineObject.Substitute.Length}!");
                return false;
            }

            //TODO: 2026-01-05 - JHA - Eigentlich müsste auch auf korrekte Substitute ($,[],§H,£,...) geprüft werden
            if ( true == string.IsNullOrWhiteSpace( lineObject.SubstitutedValue ) )
            {
                Log.Verbose("Parameter <LineObjectSubstitutionFile.KeySubstituted> must not be null!");
                return false;
            }

            if( true == string.IsNullOrWhiteSpace(lineObject.Key) )
            {
                Log.Verbose("Parameter <LineObjectSubstitutionFile.Key> must not be null!");
                return false;
            }

            if( lineObject.LineNumber < 0  )
            {
                Log.Verbose("Parameter <LineObjectSubstitutionFile.LineNumber> must not be lesser 0!");
                return false;
            }

            return true;
        }
    }
}
