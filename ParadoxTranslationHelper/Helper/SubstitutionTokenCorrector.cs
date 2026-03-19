using Serilog;
using System.Linq;

namespace ParadoxTranslationHelper.Helper
{
    internal class SubstitutionTokenCorrector
    {
        public static readonly int TOKEN_LENGTH_CORRECT = 16;
        public static readonly int TOKEN_LENGTH_CORRUPT = 17;
        public string? CorrectLine( string line, int startIndex = 0 )
        {
            if( null == line )
            {
                return null;
            }

            if( line.Length < TOKEN_LENGTH_CORRECT )
            {
                return null;
            }

            string token = FindStringTokenStartEnd(line, startIndex);
            if( token == null )
            {
                return null;
            }

            if( token.Length < TOKEN_LENGTH_CORRECT )
            {
                Log.Debug($"Found token start and end but token is to short: {token.Length};{token}");
                return null;
            }

            if( false == token.Contains(" ") )
            {
                Log.Debug($"Found token without whitespace: {token.Length};{token}");
                return null;
            }

            if( token.Length == TOKEN_LENGTH_CORRECT )
            {
                Log.Debug($"Found correct token: {token}");
                return line;
            }

            string[] splitted = token.Split(" ");
            string joined = string.Join("", splitted);

            if( joined.Length != TOKEN_LENGTH_CORRECT )
            {
                Log.Debug($"Corrected token is to short: {joined.Length};{joined}");
                return null;
            }

            Log.Debug($"Corrected token: {joined}");
            return line.Replace(token,joined);
        }

        string? FindStringTokenStartEnd( string line, int startIndex )
        {
            int indexStart = line.IndexOf(FileSubstitutionConstants.SUBSTITUTION_START_END_SIGN, startIndex);
            if( indexStart == -1 )
            {
                return null;
            }

            if( indexStart + 1 >= line.Length )
            {
                return null;
            }

            int indexEnd = line.IndexOf(FileSubstitutionConstants.SUBSTITUTION_START_END_SIGN, startIndex + indexStart + 1);
            if( indexEnd == -1 )
            {
                return null;
            }

            return line.Substring(indexStart + startIndex ,indexEnd - indexStart + 1 + startIndex);
        }
    }
}
