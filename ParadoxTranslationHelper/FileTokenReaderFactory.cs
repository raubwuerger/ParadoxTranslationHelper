using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FileTokenReaderFactory
    {
        private static FileTokenReaderFactory instance;
        public static FileTokenReaderFactory Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new FileTokenReaderFactory();
                }
                return instance;
            }
        }

        private FileTokenReaderFactory() { }

        public FileTokenReader CreateReaderNamespaces()
        {
            FileTokenReader fileReader = new FileTokenReader();
            fileReader.PathReplace = "namespace";
            fileReader.StringParser = StringParserFactory.Instance.CreateParserNamespaces();
            return fileReader;
        }

        public FileTokenReader CreateReaderIcons()
        {
            FileTokenReader fileReader = new FileTokenReader();
            fileReader.PathReplace = "icons";
            fileReader.StringParser = StringParserFactory.Instance.CreateParserIcons();
            return fileReader;
        }

        public FileTokenReader CreateReaderVariables()
        {
            FileTokenReader fileReader = new FileTokenReader();
            fileReader.PathReplace = "variables";
            fileReader.StringParser = StringParserFactory.Instance.CreateParserNestingStrings();
            return fileReader;
        }

        public FileTokenReader CreateReaderColors()
        {
            FileTokenReader fileReader = new FileTokenReader();
            fileReader.PathReplace = "colors";
            fileReader.StringParser = StringParserFactory.Instance.CreateParserColorCodes();
            return fileReader;
        }

        public FileTokenReader CreateReaderInnerDoubleQuotes()
        {
            FileTokenReader fileReader = new FileTokenReader();
            fileReader.PathReplace = "innerDoubleQuotes";
            fileReader.StringParser = StringParserFactory.Instance.CreateParserInnerDoubleQuotes();
            return fileReader;
        }

        public FileTokenReader CreateReaderKeys()
        {
            FileTokenReader fileReader = new FileTokenReader();
            fileReader.PathReplace = "keys";
            fileReader.StringParser = StringParserFactory.Instance.CreateParserKey();
            return fileReader;
        }


    }
}
