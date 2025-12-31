using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using ParadoxTranslationHelper.Comparator;
using ParadoxTranslationHelper.LineCorrector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper_Test.LineCorrectors
{
    [TestClass]
    public class LineCorrectorFirstLast_Test
    {
        LineCorrectorFactory factory = new LineCorrectorFactory();

        [TestMethod]
        [DataRow(DisplayName = "Lines null: --> 0")]
        public void CorrectNull()
        {
            List<string> _null = null;
            ILineCorrector lineCorrector = factory.CreateCorrectorFirstLast();
            lineCorrector.Correct(_null);
            Assert.AreEqual(0, lineCorrector.GetIncorrect().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines empty: --> 0")]
        public void CorrectEmpty()
        {
            List<string> empty = new List<string>();
            ILineCorrector lineCorrector = factory.CreateCorrectorFirstLast();
            lineCorrector.Correct(empty);
            Assert.AreEqual(0, lineCorrector.GetIncorrect().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines no matching signs: --> 0")]
        public void CorrectNoMatchinSigns()
        {
            List<string> notEmpty = new List<string>();
            notEmpty.Add("no matching sign!");
            ILineCorrector lineCorrector = factory.CreateCorrectorFirstLast();
            lineCorrector.Correct(notEmpty);
            Assert.AreEqual(0, lineCorrector.GetIncorrect().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines matching signs: --> 0")]
        public void CorrectMatchinSigns()
        {
            List<string> notEmpty = new List<string>();
            notEmpty.Add($"matching sign! {LineCorrectorFirstLast._incorrectSign1}{LineCorrectorFirstLast._incorrectSign2}");
            ILineCorrector lineCorrector = factory.CreateCorrectorFirstLast();
            lineCorrector.Correct(notEmpty);
            Assert.AreEqual(0, lineCorrector.GetIncorrect().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines matching signs: --> 0")]
        public void CorrectMatchinSigns2()
        {
            List<string> notEmpty = new List<string>();
            notEmpty.Add($"matching sign! {LineCorrectorFirstLast._incorrectSign2}{LineCorrectorFirstLast._incorrectSign1}");
            ILineCorrector lineCorrector = factory.CreateCorrectorFirstLast();
            lineCorrector.Correct(notEmpty);
            Assert.AreEqual(0, lineCorrector.GetIncorrect().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines matching signs identical: --> 0")]
        public void CorrectMatchinSigns3()
        {
            List<string> notEmpty = new List<string>();
            notEmpty.Add($"matching signs identical! {LineCorrectorFirstLast._incorrectSign1} some text in the middle {LineCorrectorFirstLast._incorrectSign1}");
            ILineCorrector lineCorrector = factory.CreateCorrectorFirstLast();
            lineCorrector.Correct(notEmpty);
            Assert.AreEqual(0, lineCorrector.GetIncorrect().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines only one matching: --> 1")]
        public void CorrectOnlyOneMatchinSign()
        {
            List<string> notEmpty = new List<string>();
            notEmpty.Add($"only one matching! {LineCorrectorFirstLast._incorrectSign1}");
            ILineCorrector lineCorrector = factory.CreateCorrectorFirstLast();
            lineCorrector.Correct(notEmpty);
            Assert.AreEqual(1, lineCorrector.GetIncorrect().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines with correct quataion marks include one wrong! --> 0")]
        public void CorrectQuotationMarksIncludeOneWrong()
        {
            List<string> notEmpty = new List<string>();
            notEmpty.Add($"matching signs identical! {Constants.QUOTATION_MARKS} some text in the middle {LineCorrectorFirstLast._incorrectSign1}{Constants.QUOTATION_MARKS}");
            ILineCorrector lineCorrector = factory.CreateCorrectorFirstLast();
            lineCorrector.Correct(notEmpty);
            Assert.AreEqual(0, lineCorrector.GetIncorrect().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Lines with correct quataion marks include two wrong! --> 0")]
        public void CorrectQuotationMarksIncludeTwoWrong()
        {
            List<string> notEmpty = new List<string>();
            notEmpty.Add($"matching signs identical! {Constants.QUOTATION_MARKS} {LineCorrectorFirstLast._incorrectSign1} some text in the middle {LineCorrectorFirstLast._incorrectSign1}{Constants.QUOTATION_MARKS}");
            ILineCorrector lineCorrector = factory.CreateCorrectorFirstLast();
            lineCorrector.Correct(notEmpty);
            Assert.AreEqual(0, lineCorrector.GetIncorrect().Count());
        }
        
        [TestMethod]
        [DataRow(DisplayName = "Line with correct quataion mark at the end! --> 0")]
        public void CorrectQuotationMarkAtTheEnd()
        {
            List<string> notEmpty = new List<string>();
            notEmpty.Add($"matching signs identical! {LineCorrectorFirstLast._incorrectSign1} some text in the middle {Constants.QUOTATION_MARKS}");
            ILineCorrector lineCorrector = factory.CreateCorrectorFirstLast();
            lineCorrector.Correct(notEmpty);
            Assert.AreEqual(0, lineCorrector.GetIncorrect().Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Line with correct quataion mark at the start! --> 0")]
        public void CorrectQuotationMarkAtTheStart()
        {
            List<string> notEmpty = new List<string>();
            notEmpty.Add($"matching signs identical! {Constants.QUOTATION_MARKS} some text in the middle {LineCorrectorFirstLast._incorrectSign1}");
            ILineCorrector lineCorrector = factory.CreateCorrectorFirstLast();
            lineCorrector.Correct(notEmpty);
            Assert.AreEqual(0, lineCorrector.GetIncorrect().Count());
        }

    }
}
