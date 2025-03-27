using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ParadoxTranslationHelper_Test
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
        public void ReadFileWithDifferentKeys()
        {
            const string testFileName = @"C:\Projects\ParadoxTranslationHelper\ParadoxTranslationHelper_Test\testData\stringParser\TestStrings.yml";
            IStringParser stringParser = StringParserFactory.Instance.CreateParserKey();
            TranslationFileCreator translationFileCreator = new TranslationFileCreator();
            TranslationFile translationFile = translationFileCreator.Create(testFileName);

            Assert.IsNotNull(translationFile);

            List<string> lines = Utility.ConvertToList(File.ReadAllLines(testFileName));
            Assert.IsTrue(lines.Count > 0);

            List<string> keys = new List<string>();
            foreach (string line in lines) 
            {
                List<string> token = new List<string>();
                token = stringParser.GetToken(line, token);
                if (token.Count > 0)
                {
                    keys.Add(token[0]);
                }
            }

            Console.WriteLine(keys.ToArray());
        }

        //        static string AIRWING_MISSION_DAY_NIGHT = "AIRWING_MISSION_DAY_NIGHT:0 \"Missions are executed §Hday§! and §Hnight§!.\"";

        [TestMethod]
        [DataRow(DisplayName = "Extract ColorCode: 0 --> 0")]
        public void ExtractColorCodes_000()
        {
            string AIRWING_MISSION_DAY_NIGHT = "";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserColorCodes();
            List<string> token = new List<string>();
            List<string> colorCodes = stringParser.GetToken(AIRWING_MISSION_DAY_NIGHT, token);
            Assert.AreEqual(0, colorCodes.Count);
        }

        [TestMethod]
        [DataRow(DisplayName = "Extract ColorCode: 1 --> 1")]
        public void ExtractColorCodes_001()
        {
            string AIRWING_MISSION_DAY_NIGHT = "§Hday§! and night.";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserColorCodes();
            List<string> token = new List<string>();
            List<string> colorCodes = stringParser.GetToken(AIRWING_MISSION_DAY_NIGHT, token);
            Assert.AreEqual(1, colorCodes.Count);
            Assert.AreEqual("§H",colorCodes[0]);
        }

        [TestMethod]
        [DataRow(DisplayName = "Extract ColorCode: 2 --> 2")]
        public void ExtractColorCodes_002()
        {
            string AIRWING_MISSION_DAY_NIGHT = "§Hday§! and §Anight§!.";
            
            IStringParser stringParser = StringParserFactory.Instance.CreateParserColorCodes();
            List<string> token = new List<string>();
            List<string> colorCodes = stringParser.GetToken(AIRWING_MISSION_DAY_NIGHT, token);
            Assert.AreEqual(2,colorCodes.Count);
            Assert.AreEqual("§H", colorCodes[0]);
            Assert.AreEqual("§A", colorCodes[1]);
        }

        [TestMethod]
        [DataRow(DisplayName = "Extract ColorCode: 3 --> 3")]
        public void ExtractColorCodes_003()
        {
            string AIRWING_MISSION_DAY_NIGHT = "§Hday§! and §Anight§! or §Wyet§!.";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserColorCodes();
            List<string> token = new List<string>();
            List<string> colorCodes = stringParser.GetToken(AIRWING_MISSION_DAY_NIGHT, token);
            Assert.AreEqual(3, colorCodes.Count);
            Assert.AreEqual("§H", colorCodes[0]);
            Assert.AreEqual("§A", colorCodes[1]);
            Assert.AreEqual("§W", colorCodes[2]);
        }

        [TestMethod]
        [DataRow(DisplayName = "Extract NestingStrings: 2 --> 2")]
        public void ExtractNestingStrings_001()
        {
            string ABILITY_TOOLTIP_DETAILED_COST = "    - Grundkosten: $VALUE|H2$ (jeweils für $UNITS|H0$ Bataillone)";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserNestingStrings();
            List<string> token = new List<string>();
            List<string> nestingStrings = stringParser.GetToken(ABILITY_TOOLTIP_DETAILED_COST, token);
            Assert.AreEqual(2, nestingStrings.Count);
            Assert.AreEqual("VALUE|H2", nestingStrings[0]);
            Assert.AreEqual("UNITS|H0", nestingStrings[1]);
        }
    }
}
