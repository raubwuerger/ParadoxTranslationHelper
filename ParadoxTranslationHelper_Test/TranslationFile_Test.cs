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
    public class TranslationFile_Test
    {
        TranslationFile _translationFile;

        [TestInitialize]
        public void TestInitialize()
        {
            _translationFile = new TranslationFile("Original_l_english.yml");
            _translationFile.FileNameWithoutLocalisation = "Original.yml";
            _translationFile.Lines = new Dictionary<int, LineObject> { { 1, new LineObject(1) }, { 2, new LineObject(2) } };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Null object not allowed on copy constructor.")]
        public void TestMethodCopyConstructorNull()
        {
            TranslationFile translationFileNull = null;
            TranslationFile newTranslationFile = new TranslationFile(translationFileNull);
        }

        [TestMethod]
        public void TestMethodCopyConstructor()
        {
            TranslationFile newTranslationFile = new TranslationFile(_translationFile);
            Assert.AreEqual(_translationFile.Lines.Count, newTranslationFile.Lines.Count);
        }

        [TestMethod]
        public void TestMethodCopyConstructorChangeCopiedFile()
        {
            TranslationFile newTranslationFile = new TranslationFile(_translationFile);
            newTranslationFile.Lines = new Dictionary<int, LineObject> { };
            Assert.AreNotEqual(_translationFile.Lines.Count, newTranslationFile.Lines.Count);
        }
    }
}

