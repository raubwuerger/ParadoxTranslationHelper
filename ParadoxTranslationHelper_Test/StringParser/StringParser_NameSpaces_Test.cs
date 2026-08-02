using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class StringParser_NameSpaces_Test
    {
        [TestMethod]
        [DataRow(DisplayName = "Extract NameSpaces: 0 --> 0")]
        public void ExtractNameSpaces_001()
        {
            string AFG_the_getyear_general_elections = "AFG_the_getyear_general_elections:0 \"The GetYear General Elections\"";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserNamespaces();
            List<string> namespaces = stringParser.GetToken(AFG_the_getyear_general_elections);

            Assert.AreEqual(0, namespaces.Count);
        }

        [TestMethod]
        [DataRow(DisplayName = "Extract NameSpaces: [AFG.GetAdjective] 1 --> 1")]
        public void ExtractNameSpaces_002()
        {
            string AFG_the_getyear_general_elections = "AFG_communist_influence_r56:0\"[AFG.GetAdjective] Communist Influence\"";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserNamespaces();
            List<string> namespaces = stringParser.GetToken(AFG_the_getyear_general_elections);

            Assert.AreEqual(1, namespaces.Count);
            Assert.AreEqual("AFG.GetAdjective", namespaces[0]);
        }

        [TestMethod]
        [DataRow(DisplayName = "Extract NameSpaces: [GetYear] 1 --> 1")]
        public void ExtractNameSpaces_003()
        {
            string AFG_the_getyear_general_elections = "[GetYear]";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserNamespaces();
            List<string> namespaces = stringParser.GetToken(AFG_the_getyear_general_elections);

            Assert.AreEqual(1, namespaces.Count);
            Assert.AreEqual("GetYear", namespaces[0]);
        }

        [TestMethod]
        [DataRow(DisplayName = "Extract NameSpaces: [GetYear] 1 --> 1")]
        public void ExtractNameSpaces_004()
        {
            string AFG_the_getyear_general_elections = "AFG_the_getyear_general_elections:0 \"The[GetYear] General Elections\"";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserNamespaces();
            List<string> namespaces = stringParser.GetToken(AFG_the_getyear_general_elections);

            Assert.AreEqual(1, namespaces.Count);
            Assert.AreEqual("GetYear", namespaces[0]);
        }

        [TestMethod]
        [DataRow(DisplayName = "Extract NameSpaces: [Root.GetSpeciesName},[contact_empire.GetName] 2 --> 2")]
        public void ExtractNameSpaces_005()
        {
            string Root_GetSpeciesName = "Root.GetSpeciesName";
            string contact_empire_GetName = "contact_empire.GetName";
            string action_1_bol = @$"action.1.bol:	""The name of your contact vessel bears a close resemblance to an impolite term in our language, [{Root_GetSpeciesName}]. Nevertheless, we are the [{contact_empire_GetName}], and we are pleased to make your acquaintance.""";

            IStringParser stringParser = StringParserFactory.Instance.CreateParserNamespaces();
            List<string> namespaces = stringParser.GetToken(action_1_bol);

            Assert.AreEqual(2, namespaces.Count);
            Assert.AreEqual(Root_GetSpeciesName, namespaces[0]);
            Assert.AreEqual(contact_empire_GetName, namespaces[1]);
        }
    }
}
