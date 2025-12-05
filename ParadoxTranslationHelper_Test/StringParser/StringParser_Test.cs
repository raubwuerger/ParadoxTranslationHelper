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
        public void IconTest2()
        {
            Assert.IsTrue("synthetic_refinery_resource:0\t\"£resources_strip|$FRAME$\"".Contains(StringParserFactory.ICON_START));
            Assert.IsTrue("synthetic_refinery_resource:0\t\"£resources_strip|$FRAME$\"".Contains(icon));
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
            const string testFileName = @"..\..\..\testData\stringParser\TestStrings.yml";
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
        [DataRow(DisplayName = "Extract ColorCode: 1 --> 2")]
        public void ExtractColorCodes_001()
        {
            string AIRWING_MISSION_DAY_NIGHT = "§Hday§! and night.";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserColorCodes();
            List<string> token = new List<string>();
            List<string> colorCodes = stringParser.GetToken(AIRWING_MISSION_DAY_NIGHT, token);
            Assert.AreEqual(2, colorCodes.Count);
            Assert.AreEqual("§H",colorCodes[0]);
            Assert.AreEqual("§!", colorCodes[1]);
        }

        [TestMethod]
        [DataRow(DisplayName = "Extract ColorCode: 2 --> 4")]
        public void ExtractColorCodes_002()
        {
            string AIRWING_MISSION_DAY_NIGHT = "§Hday§! and §Anight§!.";
            
            IStringParser stringParser = StringParserFactory.Instance.CreateParserColorCodes();
            List<string> token = new List<string>();
            List<string> colorCodes = stringParser.GetToken(AIRWING_MISSION_DAY_NIGHT, token);
            Assert.AreEqual(4,colorCodes.Count);
            Assert.AreEqual("§H", colorCodes[0]);
            Assert.AreEqual("§!", colorCodes[1]);
            Assert.AreEqual("§A", colorCodes[2]);
            Assert.AreEqual("§!", colorCodes[3]);
        }

        [TestMethod]
        [DataRow(DisplayName = "Extract ColorCode: 3 --> 6")]
        public void ExtractColorCodes_003()
        {
            string AIRWING_MISSION_DAY_NIGHT = "§Hday§! and §Anight§! or §Wyet§!.";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserColorCodes();
            List<string> token = new List<string>();
            List<string> colorCodes = stringParser.GetToken(AIRWING_MISSION_DAY_NIGHT, token);
            Assert.AreEqual(6, colorCodes.Count);
            Assert.AreEqual("§H", colorCodes[0]);
            Assert.AreEqual("§!", colorCodes[1]);
            Assert.AreEqual("§A", colorCodes[2]);
            Assert.AreEqual("§!", colorCodes[3]);
            Assert.AreEqual("§W", colorCodes[4]);
            Assert.AreEqual("§!", colorCodes[5]);
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

        [TestMethod]
        [DataRow(DisplayName = "Extract NameSpaces: 0 --> 0")]
        public void ExtractNameSpaces_001()
        {
            string AFG_the_getyear_general_elections = "AFG_the_getyear_general_elections:0 \"The GetYear General Elections\"";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserNamespaces();
            List<string> token = new List<string>();
            List<string> namespaces = stringParser.GetToken(AFG_the_getyear_general_elections, token);
            Assert.AreEqual(0, namespaces.Count);
        }

        [TestMethod]
        [DataRow(DisplayName = "Extract NameSpaces: (AFG.GetAdjective) 1 --> 1")]
        public void ExtractNameSpaces_002()
        {
            string AFG_the_getyear_general_elections = "AFG_communist_influence_r56:0\"[AFG.GetAdjective] Communist Influence\"";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserNamespaces();
            List<string> token = new List<string>();
            List<string> namespaces = stringParser.GetToken(AFG_the_getyear_general_elections, token);
            Assert.AreEqual(1, namespaces.Count);
            Assert.AreEqual("AFG.GetAdjective", namespaces[0]);
        }

        [TestMethod]
        [DataRow(DisplayName = "Extract NameSpaces: (GetYear) 1 --> 1")]
        public void ExtractNameSpaces_003()
        {
            string AFG_the_getyear_general_elections = "[GetYear]";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserNamespaces();
            List<string> token = new List<string>();
            List<string> namespaces = stringParser.GetToken(AFG_the_getyear_general_elections, token);
            Assert.AreEqual(1, namespaces.Count);
            Assert.AreEqual("GetYear", namespaces[0]);
        }

        [TestMethod]
        [DataRow(DisplayName = "Extract NameSpaces: (GetYear) 1 --> 0")]
        public void ExtractNameSpaces_004()
        {
            string AFG_the_getyear_general_elections = "AFG_the_getyear_general_elections:0 \"The[GetYear] General Elections\"";
            IStringParser stringParser = StringParserFactory.Instance.CreateParserNamespaces();
            List<string> token = new List<string>();
            List<string> namespaces = stringParser.GetToken(AFG_the_getyear_general_elections, token);
            Assert.AreEqual(1, namespaces.Count);
            Assert.AreEqual("GetYear", namespaces[0]);
        }

        [TestMethod]
        [DataRow(DisplayName = "ReplaceFirst null, null, null: --> null")]
        public void ReplaceFirst_001()
        {
            string invalid = null;
            Assert.IsNull(invalid.ReplaceFirst(null, null));
        }
        
        [TestMethod]
        [DataRow(DisplayName = "ReplaceFirst empty, null, null: --> null")]
        public void ReplaceFirst_002()
        {
            string valid = "";
            Assert.IsNull(valid.ReplaceFirst(null, null));
        }


        [TestMethod]
        [DataRow(DisplayName = "ReplaceFirst empty, empty, null: --> null")]
        public void ReplaceFirst_003()
        {
            string valid = "";
            Assert.IsNull(valid.ReplaceFirst("", null));
        }

        [TestMethod]
        [DataRow(DisplayName = "ReplaceFirst empty, empty, empty: --> not null")]
        public void ReplaceFirst_004()
        {
            string valid = "";
            Assert.IsNotNull(valid.ReplaceFirst("", ""));
        }

        [TestMethod]
        [DataRow(DisplayName = "ReplaceFirst valid, noMatch, toInsert: --> not null")]
        public void ReplaceFirst_010()
        {
            string valid = "valid";
            string result = valid.ReplaceFirst("noMatch", "_toInsert_");
            Assert.AreEqual(valid,result);
        }

        [TestMethod]
        [DataRow(DisplayName = "ReplaceFirst valid, noMatch, toInsert: --> not null")]
        public void ReplaceFirst_011()
        {
            string valid = "valid";
            string result = valid.ReplaceFirst("ali", "_toInsert_");
            Assert.AreEqual(result, "v_toInsert_d");
        }

        [TestMethod]
        [DataRow(DisplayName = "ReplaceFirst valid, noMatch, toInsert: --> not null")]
        public void ReplaceFirst_012()
        {
            string valid = "valid";
            string result = valid.ReplaceFirst("valid", "_toInsert_");
            Assert.AreEqual(result, "_toInsert_");
        }

        [TestMethod]
        [DataRow(DisplayName = "Contains no NewLine: --> count == 0")]
        public void FindNewLine_000()
        {
            string newLine = " accession_country_integration_in_progress: \"§HIntegrating New Member Worlds§! §R--§!:\"";
            IStringParser stringParserNewLine = StringParserFactory.Instance.CreateParserNewLine();

            List<string> tokens = new List<string>();
            List<string> found = stringParserNewLine.GetToken(newLine, tokens);
            Assert.AreEqual(0, found.Count);
        }

        [TestMethod]
        [DataRow(DisplayName = "Contains NewLine at the end: --> count == 1")]
        public void FindNewLine_001()
        {
            string newLine = " accession_country_integration_in_progress: \"§HIntegrating New Member Worlds§! §R--§!:\\n\"";
            IStringParser stringParserNewLine = StringParserFactory.Instance.CreateParserNewLine();

            List<string> tokens = new List<string>();
            List<string> found = stringParserNewLine.GetToken(newLine, tokens);
            Assert.AreEqual(1, found.Count);
        }

        [TestMethod]
        [DataRow(DisplayName = "Contains two NewLines: --> count == 2")]
        public void FindNewLine_002()
        {
            string newLine = " accession_country_integration_in_progress: \"§HIntegrating New Member Worlds§!\\n §R--§!:\\n\"";
            IStringParser stringParserNewLine = StringParserFactory.Instance.CreateParserNewLine();

            List<string> tokens = new List<string>();
            List<string> found = stringParserNewLine.GetToken(newLine, tokens);
            Assert.AreEqual(2, found.Count);
        }

        [TestMethod]
        [DataRow(DisplayName = "Contains five NewLines: --> count == 5")]
        public void FindNewLine_003()
        {
            string newLine = " accession_country_integration_in_progress: \"§H\\n\\nIntegrating \\nNew Member Worlds§!\\n §R--§!:\\n\"";
            IStringParser stringParserNewLine = StringParserFactory.Instance.CreateParserNewLine();

            List<string> tokens = new List<string>();
            List<string> found = stringParserNewLine.GetToken(newLine, tokens);
            Assert.AreEqual(5, found.Count);
        }

        [TestMethod]
        [DataRow(DisplayName = "Contains color code at line start: --> count == 1")]
        public void FindColorCode_001()
        {
            string newLine = " accession_country_integration_in_progress: \"§H\\n\\nIntegrating \\nNew Member Worlds§!\\n §R--§!:\\n\"";
            IStringParser stringParserNewLine = StringParserFactory.Instance.CreateParserNewLine();

            List<string> tokens = new List<string>();
            List<string> found = stringParserNewLine.GetToken(newLine, tokens);
            Assert.AreEqual(5, found.Count);
        }

        [TestMethod]
        [DataRow(DisplayName = "Contains icon with dircted attached other sign: --> count == 1")]
        public void FindIcon_001()
        {
            string newLine = "synthetic_refinery_resource:0\t\"£resources_strip|$FRAME$\"";
            StringParser stringParserIcon = StringParserFactory.Instance.CreateParserIcons();

            List<string> tokens = new List<string>();
            List<string> found = stringParserIcon.GetToken(newLine, tokens);
            Assert.AreEqual(1, found.Count);
        }

        [TestMethod]
        [DataRow(DisplayName = "Contains icon with attached : --> count == 1")]
        public void FindIcon_002()
        {
            string newLine = "synthetic_refinery_resource:0\t\"£resources_strip|$FRAME$ another icon £resources_strip\"";
            StringParser stringParserIcon = StringParserFactory.Instance.CreateParserIcons();

            List<string> tokens = new List<string>();
            List<string> found = stringParserIcon.GetToken(newLine, tokens);
            Assert.AreEqual(2, found.Count);
        }
    }
}
//___KY8___ "§YWenn wir den Code knacken, hat das folgende Auswirkungen: ___CC___ ___NL___ "
