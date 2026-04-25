using Serilog;
using System.Linq;

namespace ParadoxTranslationHelper.Helper
{
    internal class SubstitutionTokenCorrector
    {
        public static readonly int TOKEN_LENGTH_CORRECT = 16;
        public static readonly int TOKEN_LENGTH_CORRUPT = 17;

        private int globalEndIndex = 0;
        private int foundToken = 0;
        private bool containsNoToken = true;
        private bool containsAtLeastOneCorruptToken = false;

        public bool HasNoTokenAtAll
        {
            get
            {
                if (foundToken > 0)
                {
                    return false;
                }
                if (containsAtLeastOneCorruptToken == true)
                {
                    return false;
                }
                return true;
            }
        }
        public bool ContainsNoToken { get => containsNoToken; }
        public int FoundToken { get => foundToken; }
        public bool ContainsAtLeastOneCorruptToken { get => containsAtLeastOneCorruptToken; set => containsAtLeastOneCorruptToken = value; }

        public string? CorrectLine( string line, int globalStartIndex = 0 )
        {
            if( globalEndIndex == 0 )
            {
                containsNoToken = true;
                foundToken = 0;
                containsAtLeastOneCorruptToken = false;
            }

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

            if ( token.Length < TOKEN_LENGTH_CORRECT )
            {
                foundToken++;
                Log.Debug($"Found token start and end but token is to short: {token.Length};{token}");
                if (globalStartIndex == 0)
                {
                    return null;
                }
                return line;
            }

            if( false == token.Contains(" ") )
            {
                foundToken++;
                Log.Verbose($"Found token without whitespace: {token.Length};{token}");
                if (globalStartIndex == 0 && line.Length < TOKEN_LENGTH_CORRECT + TOKEN_LENGTH_CORRUPT)
                {
                    return line;
                }

                return CorrectLine(line,globalEndIndex);
            }

            if ( token.Length == TOKEN_LENGTH_CORRECT )
            {
                foundToken++;
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

            foundToken++;
            Log.Debug($"Corrected token: {joined}");
            return CorrectLine(line.Replace(token,joined),globalEndIndex);   //TODO: 2026-03-19 - JHA - Stimmt der lastIndex wenn das korrigierte Token eingefügt wurde?
        }

        string? FindStringTokenStartEnd( string line, int globalStartIndex )
        {
            int localStartIndex = line.IndexOf(FileSubstitutionConstants.SUBSTITUTION_START_SIGN, globalStartIndex);
            if( localStartIndex == -1 )
            {
                return null;
            }

            if( localStartIndex + 1 >= line.Length )
            {
                return null;
            }

            int localEndIndex = line.IndexOf(FileSubstitutionConstants.SUBSTITUTION_END_SIGN, localStartIndex + 1);
            if( localEndIndex == -1 )
            {
                containsAtLeastOneCorruptToken = true;
                return null;
            }

            containsNoToken = false;
            globalEndIndex = localEndIndex + 1;
            return line.Substring(localStartIndex ,localEndIndex - localStartIndex + 1);
        }
    }
}
