using ParadoxTranslationHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class LineObject_Test
    {
        LineObject _lineObject;

        [TestInitialize]
        public void TestInitialize()
        {
            _lineObject = new LineObject(1);
            _lineObject.TranslationFile = new TranslationFile("dummy");
            _lineObject.Key = "original_key";
            _lineObject.OriginalLine = "Original Line";
            _lineObject.OriginalLineSubstituted = "Original SUBSTITUTED";
            _lineObject.NameSpaces = new List<string>() { "Namespace 1", " Namespace 2"};
            _lineObject.NestingStrings = new List<string>() { };
            _lineObject.ColorCodes = new List<string>() { "CC01", "CC02", "CC03" };
            _lineObject.Icons = new List<string>() { "IC01" };
            _lineObject.NewLines = new List<string>() { };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Null object not allowed on copy constructor.")]
        public void TestMethodCopyConstructorNull()
        {
            LineObject newLineObject = new LineObject(null);
        }

        [TestMethod]
        public void TestMethodCopyConstructor()
        {
            LineObject newLineObject = new LineObject(_lineObject);
        }

        [TestMethod]
        public void TestMethodCopyConstructorChangeCopiedFile()
        {
            LineObject newLineObject = new LineObject(_lineObject);
            newLineObject.ColorCodes = new List<string> { "___CC01___" };
            Assert.AreNotEqual(newLineObject.ColorCodes, _lineObject.ColorCodes);
        }
    }
}

