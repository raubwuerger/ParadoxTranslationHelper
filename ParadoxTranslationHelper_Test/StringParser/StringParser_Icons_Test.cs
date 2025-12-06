using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class StringParser_Icons_Test
    {
        [TestMethod]
        [DataRow(DisplayName = "Contains icon with dircted attached other sign: --> count == 1")]
        public void FindIcon_000()
        {
            string _testStringWithIcon = "alfacite_ore_icon: \"£alfacite_ore£\"";

            StringParser stringParserIcon = StringParserFactory.Instance.CreateParserIcons();
            List<string> found = stringParserIcon.GetToken(_testStringWithIcon);

            Assert.AreEqual(1, found.Count);
            //TODO: 2025-12-06 - JHA - Ist das so richtig???
            Assert.AreEqual("£alfacite_ore£", found[0]);
        }

        [TestMethod]
        [DataRow(DisplayName = "Contains icon with dircted attached other sign: --> count == 1")]
        public void FindIcon_001()
        {
            string newLine = "synthetic_refinery_resource:0\t\"£resources_strip|$FRAME$\"";
            StringParser stringParserIcon = StringParserFactory.Instance.CreateParserIcons();

            List<string> found = stringParserIcon.GetToken(newLine);

            Assert.AreEqual(1, found.Count);
            Assert.AreEqual("£resources_strip", found[0]);
        }

        [TestMethod]
        [DataRow(DisplayName = "Contains 2 icons: --> count == 2")]
        public void FindIcon_002()
        {
            string newLine = "synthetic_refinery_resource:0\t\"£resources_strip|$FRAME$ another icon £astrometric_probes\"";

            StringParser stringParserIcon = StringParserFactory.Instance.CreateParserIcons();
            List<string> found = stringParserIcon.GetToken(newLine);

            Assert.AreEqual(2, found.Count);
            Assert.AreEqual("£resources_strip", found[0]);
            Assert.AreEqual("£astrometric_probes", found[1]);
        }
    }
}
