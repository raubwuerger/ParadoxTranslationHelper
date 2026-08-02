using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper.LineCorrector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper_Test.LineCorrectors
{
    [TestClass]
    public class LineCorrectorSplitByKeys_Test
    {
        //TODO: 2025-12-30 - JHA - To implement ...
        LineCorrectorFactory factory = new LineCorrectorFactory();

        [TestMethod]
        [DataRow(DisplayName = "Lines null: --> 0")]
        public void SplitNull()
        {
            List<string> _null = null;
            ILineCorrector lineCorrector = factory.CreateCorrectorSplitByKeys();
            lineCorrector.Correct(_null);
            Assert.AreEqual(0, lineCorrector.GetCorrected().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines empty: --> 0")]
        public void SplitEmpty()
        {
            List<string> _null = null;
            ILineCorrector lineCorrector = factory.CreateCorrectorSplitByKeys();
            lineCorrector.Correct(new List<string>());
            Assert.AreEqual(0, lineCorrector.GetCorrected().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines contain no keys: --> 1")]
        public void SplitNoKey()
        {
            List<string> _null = null;
            ILineCorrector lineCorrector = factory.CreateCorrectorSplitByKeys();
            lineCorrector.Correct(new List<string> { "No Key" } );
            Assert.AreEqual(1, lineCorrector.GetCorrected().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines contain one keys: --> 1")]
        public void SplitOneKey()
        {
            List<string> _null = null;
            ILineCorrector lineCorrector = factory.CreateCorrectorSplitByKeys();
            lineCorrector.Correct(new List<string> { "|___KY129551___|„EPS-Verteiler Mk IV“" });
            Assert.AreEqual(1, lineCorrector.GetCorrected().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines contain one key per line: --> 2")]
        public void SplitTwoKeyLines()
        {
            List<string> _null = null;
            ILineCorrector lineCorrector = factory.CreateCorrectorSplitByKeys();
            lineCorrector.Correct(new List<string> { "|___KY129551___|„EPS-Verteiler Mk IV“", "|___KY128017___|„Befehls-Subroutinen“" });
            Assert.AreEqual(2, lineCorrector.GetCorrected().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines contain two keys in one line: --> 2")]
        public void SplitTwoKeysInOneLine()
        {
            List<string> _null = null;
            ILineCorrector lineCorrector = factory.CreateCorrectorSplitByKeys();
            lineCorrector.Correct(new List<string> { "|___KY129551___|„EPS-Verteiler Mk IV“|___KY128017___|„Befehls-Subroutinen“" });
            Assert.AreEqual(2, lineCorrector.GetCorrected().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines contain three keys in one line and another key: --> 4")]
        public void SplitTwoThreeKeysInOneLine()
        {
            List<string> _null = null;
            ILineCorrector lineCorrector = factory.CreateCorrectorSplitByKeys();
            lineCorrector.Correct(new List<string> { "|___KY129551___|„EPS-Verteiler Mk IV“|___KY128017___|„Befehls-Subroutinen“|___KY129372___|„Schildgenerator“" , "|___KY128430___|„Ao'Holvhe”" });
            Assert.AreEqual(4, lineCorrector.GetCorrected().Count());
        }
    }
}
