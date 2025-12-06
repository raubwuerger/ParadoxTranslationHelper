using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class StringParser_Keys_Test
    {
        [TestMethod]
        public void Key_Test01()
        {
            string _testStringInnerDoubleQuotesRealShort = "news.3.d:0 \"Äthiopien ist italienisch\", sagt Mussolini, Kostprobe italienischer Militärmacht bekommen sein!\"";
            
            IStringParser stringParser = StringParserFactory.Instance.CreateParserKey();
            List<string> tokensFound = stringParser.GetToken(_testStringInnerDoubleQuotesRealShort);

            Assert.AreEqual(tokensFound[0], tokensFound[0]);
        }

        [TestMethod]
        public void Key_Test02()
        {
            string _testStringInnerDoubleQuotesRealEmpty = "news.3.d:0 \"\"";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserKey();
            List<string> tokensFound = stringParser.GetToken(_testStringInnerDoubleQuotesRealEmpty);

            Assert.AreEqual(tokensFound[0], tokensFound[0]);
        }

        [TestMethod]
        public void Key_Test03()
        {
            string _testStringLineToIgnore = "# \"";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserKey();
            List<string> tokensFound = stringParser.GetToken(_testStringLineToIgnore);
            
            Assert.IsTrue(stringParser.GetToken(_testStringLineToIgnore).Count == 0);
        }

        [TestMethod]
        public void Key_Test04()
        {
            string _testStringLineToIgnore1 = " #\"";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserKey();

            Assert.IsTrue(stringParser.GetToken(_testStringLineToIgnore1).Count == 0);
        }

        [TestMethod]
        public void Key_Test05()
        {
            string _testStringLineToIgnore2 = "  #       \"";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserKey();

            Assert.IsTrue(stringParser.GetToken(_testStringLineToIgnore2).Count == 0);
        }
    }
}
