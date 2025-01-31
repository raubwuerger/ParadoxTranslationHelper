using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using ParadoxTranslationHelper.FunctionObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class AnalyzerMissingFiles_Test
    {
        [TestMethod]
        [DataRow(DisplayName = "GenerateMissingGermanTranslationFiles: null,null --> null")]
        public void TestMethod001()
        {
            List<TranslationFile> english = Utility.CreateTranslationFilesFromDirectory(@"C:\Projects\ParadoxTranslationHelper\ParadoxTranslationHelper_Test\testData\localization\missingFiles\GermanThree_EnglishThree_different_OneIdentical\english");
            List<TranslationFile> german = Utility.CreateTranslationFilesFromDirectory(@"C:\Projects\ParadoxTranslationHelper\ParadoxTranslationHelper_Test\testData\localization\missingFiles\GermanThree_EnglishThree_different_OneIdentical\german");

            AnalyzerMissingFiles.GenerateMissingGermanTranslationFiles(german, english);
            AnalyzerMissingFiles.GenerateMissingGermanTranslationFiles(null,null);
        }
    }
}
