using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParadoxTranslationHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class FileTokenReader_Test
    {
        String fileEmpty = new string(".\\testData\\fileReader\\fileEmpty.yml");
        String fileTokenNone = new string(".\\testData\\fileReader\\fileTokenNone.yml");
        String fileTokenOne = new string(".\\testData\\fileReader\\fileTokenOne.yml");

        [TestMethod]
        public void TestMethodReadNull()
        {
            FileTokenReader fileReader = new FileTokenReader();
            Assert.IsNull(fileReader.Read(null));
        }

        [TestMethod]
        public void TestMethodReadEmpty()
        {
            FileTokenReader fileReader = new FileTokenReader();
            Assert.IsNull(fileReader.Read(fileEmpty));
        }

        [TestMethod]
        public void TestMethodReadTokenNone()
        {
            FileTokenReader fileReader = new FileTokenReader();
            Assert.IsNull(fileReader.Read(fileTokenNone));
        }

        [TestMethod]
        public void TestMethodReadTokenOne()
        {
            FileTokenReader fileReader = new FileTokenReader();
            FileWithToken result = fileReader.Read(fileTokenOne);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
    }
}