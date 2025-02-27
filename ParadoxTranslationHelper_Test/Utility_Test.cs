using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParadoxTranslationHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper.Helper;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class Utility_Test
    {
        [TestMethod]
        public void ConvertToDictionaryNull()
        {
            Assert.IsFalse(DictionaryHelper.ConvertToDictionary(null).Any());
        }

        [TestMethod]
        public void ConvertToDictionaryEmpty()
        {
            List<string> empty = new List<string>();
            Assert.IsFalse(DictionaryHelper.ConvertToDictionary(empty).Any());
        }

        [TestMethod]
        public void ConvertToDictionaryOnlyKey()
        {
            List<string> onlyKey = new List<string>();
            onlyKey.Add("onlyKey");
            Assert.IsFalse(DictionaryHelper.ConvertToDictionary(onlyKey).Any());
        }

        [TestMethod]
        public void ConvertToDictionaryOnlyValue()
        {
            List<string> onlyValue = new List<string>();
            onlyValue.Add(";onlyValue");
            Assert.IsFalse(DictionaryHelper.ConvertToDictionary(onlyValue).Any());
        }

        [TestMethod]
        public void ConvertToDictionaryDoubleKey()
        {
            List<string> doubleKey = new List<string>();
            doubleKey.Add("entryOne;valueOne");
            doubleKey.Add("entryOne;valueOne");
            Assert.AreEqual(1, DictionaryHelper.ConvertToDictionary(doubleKey).Count);
        }

        [TestMethod]
        public void ConvertToDictionaryTwoEntries()
        {
            List<string> twoEntries = new List<string>();
            twoEntries.Add("entryOne;valueOne");
            twoEntries.Add("entryTwo;valueTwo");
            Assert.AreEqual(2, DictionaryHelper.ConvertToDictionary(twoEntries).Count);
        }

        [TestMethod]
        [DataRow(DisplayName = "RemoveAllFileExtensions: null --> null")]
        public void RemoveAllFileExtensions001()
        {
            Assert.IsNull(Utility.RemoveAllFileExtensions(null));
        }

        [TestMethod]
        [DataRow(DisplayName = "RemoveAllFileExtensions: fileName --> fileName")]
        public void RemoveAllFileExtensions002()
        {
            string fileName = "fileName";
            Assert.AreEqual(fileName, Utility.RemoveAllFileExtensions(fileName));
        }

        [TestMethod]
        [DataRow(DisplayName = "RemoveAllFileExtensions: fileName.ext1 --> fileName")]
        public void RemoveAllFileExtensions003()
        {
            string fileName = "fileName";
            string fileNameWithExtension = fileName + ".ext1";
            Assert.AreEqual(fileName, Utility.RemoveAllFileExtensions(fileNameWithExtension));
        }

        [TestMethod]
        [DataRow(DisplayName = "RemoveAllFileExtensions: fileName.ext1.ext2 --> fileName")]
        public void RemoveAllFileExtensions004()
        {
            string fileName = "fileName";
            string fileNameWithExtension = fileName + ".ext1" +".ext2";
            Assert.AreEqual(fileName, Utility.RemoveAllFileExtensions(fileNameWithExtension));
        }

        [TestMethod]
        [DataRow(DisplayName = "RemoveAllFileExtensions: fileName.ext1.ext2.ext3 --> fileName")]
        public void RemoveAllFileExtensions005()
        {
            string fileName = "fileName";
            string fileNameWithExtension = fileName + ".ext1" + ".ext2" +".ext3";
            Assert.AreEqual(fileName, Utility.RemoveAllFileExtensions(fileNameWithExtension));
        }
    }
}