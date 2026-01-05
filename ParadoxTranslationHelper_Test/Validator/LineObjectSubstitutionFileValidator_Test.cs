using ParadoxTranslationHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParadoxTranslationHelper.Validators;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class LineObjectSubstitutionFileValidator_Test
    {
        private static LineObjectSubstitutionFile CreateEmpty(string empty = "")
        {
            return new LineObjectSubstitutionFile(empty);
        }

        private static LineObjectSubstitutionFile CreateValid()
        {
            LineObjectSubstitutionFile lineObject = new LineObjectSubstitutionFile(FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.NESTING_STRING_SUFFIX + Utility.PadLeft_6_0(7) + FileSubstitutionConstants.SUBSTITUTION_END);
            lineObject.SubstitutedValue = "$starfleet_ironclad$";
            lineObject.Key = "tech_starfleet_ironclad_unlock_title:";
            lineObject.LineNumber = 3;
            return lineObject;
        }

        [TestMethod]
        [DataRow(DisplayName = "Validate: null --> false")]
        public void LineObjectSubstitutionFile_Null()
        {
            Assert.IsFalse(LineObjectValidator.IsValid((LineObjectSubstitutionFile)null));
        }

        [TestMethod]
        [DataRow(DisplayName = "Validate: invalid substitute --> false")]
        public void LineObjectSubstitutionFile_Empty()
        {
            Assert.IsFalse(LineObjectValidator.IsValid(CreateEmpty()));
        }

        [TestMethod]
        [DataRow(DisplayName = "Validate: invalid substitute to short --> false")]
        public void LineObjectSubstitutionFile_008()
        {
            Assert.IsFalse(LineObjectValidator.IsValid(CreateEmpty("toShort")));
        }

        [TestMethod]
        [DataRow(DisplayName = "Validate: invalid substitute to long --> false")]
        public void LineObjectSubstitutionFile_009()
        {
            Assert.IsFalse(LineObjectValidator.IsValid(CreateEmpty("toLong_toLong_toLong")));
        }

        [TestMethod]
        [DataRow(DisplayName = "Validate: invalid substitute value --> false")]
        public void LineObjectSubstitutionFile_010()
        {
            LineObjectSubstitutionFile lineObject = CreateValid();
            lineObject.SubstitutedValue = "";
            Assert.IsFalse(LineObjectValidator.IsValid(lineObject));
        }

        [TestMethod]
        [DataRow(DisplayName = "Validate: invalid key --> false")]
        public void LineObjectSubstitutionFile_011()
        {
            LineObjectSubstitutionFile lineObject = CreateValid();
            lineObject.Key = "";
            Assert.IsFalse(LineObjectValidator.IsValid(lineObject));
        }

        [TestMethod]
        [DataRow(DisplayName = "Validate: invalid line number --> false")]
        public void LineObjectSubstitutionFile_012()
        {
            LineObjectSubstitutionFile lineObject = CreateValid();
            lineObject.LineNumber = -1;
            Assert.IsFalse(LineObjectValidator.IsValid(lineObject));
        }

        [TestMethod]
        [DataRow(DisplayName = "Validate: valid --> true")]
        public void LineObjectSubstitutionFile_100()
        {
            LineObjectSubstitutionFile lineObject = CreateValid();
            Assert.IsTrue(LineObjectValidator.IsValid(lineObject));
        }
    }
}

