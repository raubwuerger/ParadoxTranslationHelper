using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParadoxTranslationHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper.Helper;
using ParadoxTranslationHelper.Utilities;

//TODO: 2025-06-12 - JHA - Wird anscheinend nicht mehr benötigt
namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class FunctionUtilityDiffNestingStringsTranslationFile_Test
    {
        static List<TranslationFile> emptyTranslationFiles;
        static List<TranslationFile> steamTranslationFiles;
        static List<TranslationFile> germanTranslationFiles;

        [TestInitialize()]
        public void TestInitialize()
        {
            emptyTranslationFiles = new List<TranslationFile>();
            steamTranslationFiles = FileUtility.CreateTranslationFilesFromDirectory(@"..\..\..\testData\diffNestingStrings\steam");
            germanTranslationFiles = FileUtility.CreateTranslationFilesFromDirectory(@"..\..\..\testData\diffNestingStrings\german");
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: null,null --> null")]
        public void DiffNestingStrings_001()
        {
            Assert.IsFalse(FunctionUtility.DiffNestingStrings(null,null));
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: valid,null --> null")]
        public void DiffNestingStrings_002()
        {
//            Assert.IsFalse(FunctionUtility.DiffNestingStringsTranslationFile(steamTranslationFiles[0], null));
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: null,valid --> null")]
        public void DiffNestingStrings_003()
        {
//            Assert.IsFalse(FunctionUtility.DiffNestingStringsTranslationFile(null, germanTranslationFiles[0]));
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: valid,valid --> null")]
        public void DiffNestingStrings_004()
        {
            //            Assert.IsFalse(FunctionUtility.DiffNestingStrings(validKey1, new List<LineObject>()));
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: valid,no key --> null")]
        public void DiffNestingStrings_005()
        {
            //            LineObject lineObject = new LineObject(1);
            //            Assert.IsFalse(FunctionUtility.DiffNestingStrings(validKey1, new List<LineObject> { lineObject } ));
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: valid,wrong key --> null")]
        public void DiffNestingStrings_006()
        {
            //            LineObject lineObject = new LineObject(1);
            //            lineObject.Key = wrongKey;
            //            Assert.IsFalse(FunctionUtility.DiffNestingStrings(validKey1, new List<LineObject> { lineObject }));
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: valid, valid --> true")]
        public void DiffNestingStrings_010()
        {
//            Assert.IsTrue(FunctionUtility.DiffNestingStringsTranslationFile(steamTranslationFiles[0], germanTranslationFiles[0]));
        }
    }
}