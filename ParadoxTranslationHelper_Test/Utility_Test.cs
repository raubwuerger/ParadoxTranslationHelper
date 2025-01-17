using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParadoxTranslationHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HoI4_TranslationHelper_Test
{
    [TestClass]
    public class Utility_Test
    {
        [TestMethod]
        public void ConvertToDictionaryNull()
        {
            Assert.IsFalse(Utility.ConvertToDictionary(null).Any());
        }

        [TestMethod]
        public void ConvertToDictionaryEmpty()
        {
            List<string> empty = new List<string>();
            Assert.IsFalse(Utility.ConvertToDictionary(empty).Any());
        }

        [TestMethod]
        public void ConvertToDictionaryOnlyKey()
        {
            List<string> onlyKey = new List<string>();
            onlyKey.Add("onlyKey");
            Assert.IsFalse(Utility.ConvertToDictionary(onlyKey).Any());
        }

        [TestMethod]
        public void ConvertToDictionaryOnlyValue()
        {
            List<string> onlyValue = new List<string>();
            onlyValue.Add(";onlyValue");
            Assert.IsFalse(Utility.ConvertToDictionary(onlyValue).Any());
        }

        [TestMethod]
        public void ConvertToDictionaryDoubleKey()
        {
            List<string> doubleKey = new List<string>();
            doubleKey.Add("entryOne;valueOne");
            doubleKey.Add("entryOne;valueOne");
            Assert.AreEqual(1, Utility.ConvertToDictionary(doubleKey).Count);
        }

        [TestMethod]
        public void ConvertToDictionaryTwoEntries()
        {
            List<string> twoEntries = new List<string>();
            twoEntries.Add("entryOne;valueOne");
            twoEntries.Add("entryTwo;valueTwo");
            Assert.AreEqual(2, Utility.ConvertToDictionary(twoEntries).Count);
        }
    }
}