using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using ParadoxTranslationHelper.FunctionObject;
using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class AnalyzeNestingStrings_Test
    {
        [TestMethod]
        [DataRow(DisplayName = "AnalyzeNestingStrings: null,null --> null")]
        public void TestMethod001()
        {
            string pwd = System.IO.Directory.GetCurrentDirectory();

            List<TranslationFile> steam = FileUtility.CreateTranslationFilesFromDirectory(@"..\..\..\testData\diffNestingStrings\steam");
            List<TranslationFile> german = FileUtility.CreateTranslationFilesFromDirectory(@"..\..\..\testData\diffNestingStrings\german");

            AnalyzerMissingFiles.GenerateMissingGermanTranslationFiles(german, steam);
            Assert.IsNull( AnalyzerMissingFiles.GenerateMissingGermanTranslationFiles(null,null) );
        }
    }
}
