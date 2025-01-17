using ParadoxTranslationHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace HoI4_TranslationHelper_Test
{
    [TestClass]
    public class StringParser_Test
    {
        private string testTokenStart = "[";
        private string testTokenEnd = "]";
        private string icon = "£";
        private StringParser _stringParser;

        private string _testStringInnerDoubleQuotes = "news.3.d:0 \"Äthiopien ist italienisch\", sagt Mussolini, als seine Truppen Addis Abeba besetzen\n Äthiopiens Ära der \\\"Unabhängigkeit\\\", die seit biblischen Zeiten andauerte, endete heute Nachmittag um 4 Uhr, nachdem Badoglio 30.000 Mann in die \"Hauptstadt\" geführt hatte, während am Himmel Flugzeuge schwirrten. In Rom sprach Mussolini zu einer riesigen Menschenmenge auf der Piazza Venezia, die sich auf jedem Dorfplatz Italiens vor den Lautsprechern versammelte, und die Botschaft war klar: Die Welt hat ihre erste Kostprobe italienischer Militärmacht bekommen - und es wird nicht die letzte sein!\"\"";
        private string _testStringInnerDoubleQuotesReal = "news.3.d:0 \"Äthiopien ist italienisch\", sagt Mussolini, als seine Truppen Addis Abeba besetzen\\n Äthiopiens Ära der Unabhängigkeit, die seit biblischen Zeiten andauerte, endete heute Nachmittag um 4 Uhr, nachdem Badoglio 30.000 Mann in die Hauptstadt geführt hatte, während am Himmel Flugzeuge schwirrten. In Rom sprach Mussolini zu einer riesigen Menschenmenge auf der Piazza Venezia, die sich auf jedem Dorfplatz Italiens vor den Lautsprechern versammelte, und die Botschaft war klar: Die Welt hat ihre erste Kostprobe italienischer Militärmacht bekommen - und es wird nicht die letzte sein!\"";
        private string _testStringInnerDoubleQuotesRealShort = "news.3.d:0 \"Äthiopien ist italienisch\", sagt Mussolini, Kostprobe italienischer Militärmacht bekommen sein!\"";
        private string _testStringInnerDoubleQuotesRealEmpty = "news.3.d:0 \"\"";
        private string _testStringLineToIgnore = "# \"";
        private string _testStringLineToIgnore1 = " #\"";
        private string _testStringLineToIgnore2 = "  #       \"";
        private string _testStringWithIcon = "alfacite_ore_icon: \"£alfacite_ore£\"";

        [TestInitialize]
        public void Initialize()
        {
            _stringParser = new StringParser();
        }

        [TestMethod]
        public void TestMethod1()
        {
            List<string> tokens = new List<string>();
            string toTest = "sdfsdfsf[asds]";

            List<string> tokenExpected = new List<string>() { "asds" };
            List<string> tokensFound = _stringParser.GetToken(toTest, tokens);
            Assert.AreEqual(tokenExpected[0], tokensFound[0]);
        }

        [TestMethod]
        public void TestMethod2()
        {
            List<string> tokens = new List<string>();
            string toTest = "sdfsdfsf£asds";

            _stringParser.StartTag = "£";
            _stringParser.EndTags.Add(" ");

            List<string> tokenExpected = new List<string>() { "asds" };
            List<string> tokensFound = _stringParser.GetToken(toTest, tokens);
            Assert.AreEqual(tokenExpected[0], tokensFound[0]);
        }

        [TestMethod]
        public void TestMethod3()
        {
            List<string> tokens = new List<string>();
            string toTest = "DISBAND_PRIDE_OF_THE_FLEET_COST:1	\"Disbanding your §HPride of the Fleet§! §H($NAME$)§!will cost $COST | R$ £pol_power\"";

            _stringParser.StartTag = "£";
            _stringParser.EndTags.Add(" ");
            _stringParser.EndTags.Add("\n");
            _stringParser.EndTags.Add("\"");

            List<string> tokenExpected = new List<string>() { "pol_power" };
            List<string> tokensFound = _stringParser.GetToken(toTest, tokens);
            Assert.AreEqual(tokenExpected[0], tokensFound[0]);
        }

        [TestMethod]
        public void InnerDoubleQuotes_Test()
        {
            StringParserBase stringParserBase = new StringParserFirstLast();

            List<string> tokens = new List<string>();
            stringParserBase.StartTag = "\"";
            stringParserBase.EndTags.Add("\"");

            List<string> tokensFound = stringParserBase.GetToken(_testStringInnerDoubleQuotesRealShort, tokens);
            Assert.AreEqual(tokensFound[0], tokensFound[0]);
        }

        [TestMethod]
        public void InnerDoubleQuotes_Test02()
        {
            StringParserBase stringParserBase = new StringParserFirstLast();

            List<string> tokens = new List<string>();
            stringParserBase.StartTag = "\"";
            stringParserBase.EndTags.Add("\"");

            List<string> tokensFound = stringParserBase.GetToken(_testStringInnerDoubleQuotesRealEmpty, tokens);
            Assert.AreEqual(tokensFound[0], tokensFound[0]);
        }

        [TestMethod]
        public void Key_Test01()
        {
            StringParserBase stringParserBase = new StringParserKey();

            List<string> tokens = new List<string>();
            stringParserBase.StartTag = "\"";

            List<string> tokensFound = stringParserBase.GetToken(_testStringInnerDoubleQuotesRealShort, tokens);
            Assert.AreEqual(tokensFound[0], tokensFound[0]);
        }

        [TestMethod]
        public void Key_Test02()
        {
            StringParserBase stringParserBase = new StringParserKey();

            List<string> tokens = new List<string>();
            stringParserBase.StartTag = "\"";

            List<string> tokensFound = stringParserBase.GetToken(_testStringInnerDoubleQuotesRealEmpty, tokens);
            Assert.AreEqual(tokensFound[0], tokensFound[0]);
        }

        [TestMethod]
        public void Key_Test03()
        {
            StringParserBase stringParserBase = new StringParserKey();

            List<string> tokens = new List<string>();
            stringParserBase.StartTag = "\"";
            stringParserBase.LineIgnores.Add("#");
            stringParserBase.LineIgnores.Add(" #");

            List<string> tokensFound = stringParserBase.GetToken(_testStringLineToIgnore, tokens);
            
            Assert.IsTrue(stringParserBase.GetToken(_testStringLineToIgnore, tokens).Count == 0);
        }

        [TestMethod]
        public void Key_Test04()
        {
            StringParserBase stringParserBase = new StringParserKey();

            List<string> tokens = new List<string>();
            stringParserBase.StartTag = "\"";
            stringParserBase.LineIgnores.Add("#");
            stringParserBase.LineIgnores.Add(" #");

            Assert.IsTrue(stringParserBase.GetToken(_testStringLineToIgnore1, tokens).Count == 0);
        }

        [TestMethod]
        public void Key_Test05()
        {
            StringParserBase stringParserBase = new StringParserKey();

            List<string> tokens = new List<string>();
            stringParserBase.StartTag = "\"";
            stringParserBase.LineIgnores.Add("#");
            stringParserBase.LineIgnores.Add(" #");
            stringParserBase.LineIgnores.Add("  #");

            Assert.IsTrue(stringParserBase.GetToken(_testStringLineToIgnore2, tokens).Count == 0);
        }

        [TestMethod]
        public void IconTest()
        {
            Assert.IsTrue(_testStringWithIcon.Contains(StringParserFactory.ICON_START));
            Assert.IsTrue(_testStringWithIcon.Contains(icon));
            Assert.AreEqual(icon, StringParserFactory.ICON_START);
        }

        [TestMethod]
        public void NewLineNull()
        {
            IStringParser stringParser = StringParserFactory.Instance.CreateParserNewLine();
            List<string> tokens = new List<string>();
            Assert.AreEqual(0,stringParser.GetToken(null, tokens).Count);
        }

        [TestMethod]
        public void NewLineEmpty()
        {
            IStringParser stringParser = StringParserFactory.Instance.CreateParserNewLine();
            List<string> tokens = new List<string>();
            Assert.AreEqual(0, stringParser.GetToken("", tokens).Count);
        }

        [TestMethod]
        public void NewLineNoToken()
        {
            string noToken = "no Token";
            IStringParser stringParser = StringParserFactory.Instance.CreateParserNewLine();
            List<string> tokens = new List<string>();
            Assert.AreEqual(0, stringParser.GetToken(noToken, tokens).Count);
        }

        [TestMethod]
        public void NewLineCorruptedToken()
        {
            string corruptedToken = @"\no Token";
            IStringParser stringParser = StringParserFactory.Instance.CreateParserNewLine();
            List<string> tokens = new List<string>();
            Assert.AreEqual(0, stringParser.GetToken(corruptedToken, tokens).Count);
        }

        [TestMethod]
        public void NewLineOneToken()
        {
            char backSlash = (char)92;
            char nn = (char)110;

            string backSlashnn = backSlash.ToString() + nn.ToString();
            string corruptedToken = @"\nno \Toke\n";
            string corruptedToken2 = @"\n o Token";
            string corruptedToken3 = @"\ no Token";
            int index = corruptedToken.IndexOf(@"\n");
            int index2 = corruptedToken.IndexOf('\n');
            IStringParser stringParser = StringParserFactory.Instance.CreateParserNewLine();
            List<string> tokens = new List<string>();
            Assert.AreEqual(1, stringParser.GetToken(corruptedToken, tokens).Count);
            stringParser = StringParserFactory.Instance.CreateParserNewLine();
            Assert.AreEqual(2, stringParser.GetToken(corruptedToken2, tokens).Count);
            stringParser = StringParserFactory.Instance.CreateParserNewLine();
            Assert.AreEqual(0, stringParser.GetToken(corruptedToken3, null).Count);
        }
    }
}
