using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using ParadoxTranslationHelper.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class Utility_TruncateOriginalLine_Test
    {
        string testFileName = "STNC_events_l_english.yml";// = @"D:\Privat\Projekte\ParadoxTranslationHelper\ParadoxTranslationHelper_Test\testData\stringParser\STNC_events_l_english.yml";
        TranslationFile translationFile;
        string testFilesLocation = @"testData\stringParser";

        [TestInitialize()]
        public void TestInitialize()
        {
            TranslationFileCreator translationFileCreator = new TranslationFileCreator();
            translationFile = translationFileCreator.Create(Path.Combine(Helper.GetTestLocation(),testFilesLocation,testFileName));
            Assert.IsNotNull(translationFile);
        }

        [TestMethod]
        public void TruncateOriginalLine_001()
        {
            foreach(KeyValuePair<int,LineObject> keyValuePair in translationFile.Lines )
            {

            }
        }
    }
}