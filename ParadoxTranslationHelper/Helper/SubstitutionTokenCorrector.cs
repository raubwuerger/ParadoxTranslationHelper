using Serilog;
using System.Linq;

namespace ParadoxTranslationHelper.Helper
{
    internal class SubstitutionTokenCorrector
    {
        public static readonly int TOKEN_LENGTH_CORRECT = 16;
        public static readonly int TOKEN_LENGTH_CORRUPT = 17;

        private int globalEndIndex = 0;
        public string? CorrectLine( string line, int globalStartIndex = 0 )
        {
            if( null == line )
            {
                return null;
            }

            if( line.Length < TOKEN_LENGTH_CORRECT )
            {
                if (globalStartIndex == 0)
                {
                    return null;
                }
                return line;
            }

            string token = FindStringTokenStartEnd(line, globalStartIndex);
            if( token == null )
            {
                if( globalStartIndex == 0 )
                {
                    return null;
                }
                return line;
            }

            if( token.Length < TOKEN_LENGTH_CORRECT )
            {
                Log.Debug($"Found token start and end but token is to short: {token.Length};{token}");
                if (globalStartIndex == 0)
                {
                    return null;
                }
                return line;
            }

            if( false == token.Contains(" ") )
            {
                Log.Debug($"Found token without whitespace: {token.Length};{token}");
                if (globalStartIndex == 0)
                {
                    return line;
                }
                return CorrectLine(line,globalEndIndex);
            }

            if ( token.Length == TOKEN_LENGTH_CORRECT )
            {
                Log.Debug($"Found correct token: {token}");
                return CorrectLine(line, globalEndIndex);
            }

            string[] splitted = token.Split(" ");
            string joined = string.Join("", splitted);

            if( joined.Length != TOKEN_LENGTH_CORRECT )
            {
                Log.Debug($"Corrected token is to short: {joined.Length};{joined}");
                if (globalStartIndex == 0)
                {
                    return null;
                }
                return CorrectLine(line, globalEndIndex);
            }

            Log.Debug($"Corrected token: {joined}");
            return CorrectLine(line.Replace(token,joined),globalEndIndex);   //TODO: 2026-03-19 - JHA - Stimmt der lastIndex wenn das korrigierte Token eingefügt wurde?
        }

        string? FindStringTokenStartEnd( string line, int globalStartIndex )
        {
            int localStartIndex = line.IndexOf(FileSubstitutionConstants.SUBSTITUTION_START_END_SIGN, globalStartIndex);
            if( localStartIndex == -1 )
            {
                return null;
            }

            if( localStartIndex + 1 >= line.Length )
            {
                return null;
            }

            int localEndIndex = line.IndexOf(FileSubstitutionConstants.SUBSTITUTION_START_END_SIGN, localStartIndex + 1);
            if( localEndIndex == -1 )
            {
                return null;
            }

            globalEndIndex = localEndIndex;
            return line.Substring(localStartIndex ,localEndIndex - localStartIndex + 1);
        }
    }
}
