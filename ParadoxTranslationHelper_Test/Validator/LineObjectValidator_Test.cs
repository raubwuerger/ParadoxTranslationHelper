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
    public class LineObjectValidator_Test
    {
        LineObject _lineObject;

        [TestInitialize]
        public void TestInitialize()
        {
            _lineObject = new LineObject(1);
        }

        private static LineObject CreateEmpty()
        {
            return new LineObject(1);
        }

        [TestMethod]
        [DataRow(DisplayName = "Validate: null --> false")]
        public void IsValidNull()
        {
            Assert.IsFalse(LineObjectValidator.IsValid((LineObject)null));
        }

        [TestMethod]
        [DataRow(DisplayName = "Validate: empty --> true")]
        public void TestMethodCopyConstructor()
        {
            Assert.IsTrue(LineObjectValidator.IsValid(CreateEmpty()));
        }

        [TestMethod]
        [DataRow(DisplayName = "Validate: starts with # --> true")]
        public void TestMethodCopyConstructorChangeCopiedFile()
        {
            LineObject newLineObject = CreateEmpty();
            newLineObject.OriginalLine = "#";
            Assert.IsTrue(LineObjectValidator.IsValid(newLineObject));
        }
    }
}

