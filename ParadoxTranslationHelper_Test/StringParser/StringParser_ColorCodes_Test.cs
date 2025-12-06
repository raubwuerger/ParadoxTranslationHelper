using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class StringParser_ColorCodes_Test
    {
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
    }
}
