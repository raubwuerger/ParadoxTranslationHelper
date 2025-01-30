using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FileTokenReader
    {
        private IStringParser _stringParser;
        public IStringParser StringParser { get => _stringParser; set => _stringParser = value; }

        private string _pathReplace;
        public string PathReplace { get => _pathReplace; set => _pathReplace = value; }

        public FileWithToken Read(string textFileName)
        {
            if( null == _stringParser )
            {
                return null;
            }

            if (null == _pathReplace)
            {
                return null;
            }

            if( null == textFileName)
            {
                return null;
            }

            string[] lines = File.ReadAllLines(textFileName);
            if( false == lines.Any() )
            {
                return null;
            }

            List<LineTextTupel> parsedFile = ParseStrings(lines);
            if(false == parsedFile.Any())
            {
                return null;
            }

            FileWithToken fileWithToken = new FileWithToken(parsedFile);
            fileWithToken.PathName = Path.GetDirectoryName(textFileName);
            fileWithToken.FileName = Path.GetFileName(textFileName);
            fileWithToken.PathNameToSave = Path.Combine(fileWithToken.PathName, _pathReplace, fileWithToken.FileName);

            return fileWithToken;
        }

        private List<LineTextTupel> ParseStrings(string[] lines )
        {
            List<LineTextTupel> lineTextTupels = new List<LineTextTupel>();
            int lineNumber = 0;
            foreach( string line in lines )
            {
                lineNumber++;
                List<string> tokens = new List<string>();
                tokens = StringParser.GetToken(line, tokens);
                if( null == tokens )
                {
                    continue;
                }

                foreach( string token in tokens )
                {
                    LineTextTupel lineTextTupel = new LineTextTupel(lineNumber, token.Trim());
                    lineTextTupel.LineText = line;
                    lineTextTupels.Add(lineTextTupel);
                }
            }

            return lineTextTupels;
        }
    }
}
