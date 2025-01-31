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
    public class LineObjectCreator_Test
    {
        int LINE_NUMBER_ONE = 1;
        int LINE_NUMBER_TWO = 2;
        private void ParameterizeLineObject(LineObjectCreator lineObjectCreator)
        {
            string key = "key1";
            TranslationFile translationFile = new TranslationFile(key);
            List<string> nestingString = new List<string>();
            List<string> namespaces = new List<string>();
            List<string> colorCodes = new List<string>();
            List<string> icons = new List<string>();

            lineObjectCreator.TranslationFile = translationFile;
            lineObjectCreator.Key = key;
            lineObjectCreator.NameSpace = namespaces;
            lineObjectCreator.NestingStrings = nestingString;
            lineObjectCreator.ColorCodes = colorCodes;
            lineObjectCreator.Icons = icons;
        }

        [TestMethod]
        public void TestMethodCreate()
        {
            LineObjectCreator lineObjectCreator = new LineObjectCreator();
            LineObject lineObject = lineObjectCreator.Create(LINE_NUMBER_ONE);
            Assert.AreEqual(1UL, lineObject.LineNumber);
            Assert.IsNull(lineObject.TranslationFile);
            Assert.IsNull(lineObject.Key);
            Assert.IsNull(lineObject.NestingStrings);
            Assert.IsNull(lineObject.NameSpaces);
            Assert.IsNull(lineObject.ColorCodes);
            Assert.IsNull(lineObject.Icons);
        }

        [TestMethod]
        public void TestMethodCreateWithAdditionalData()
        {
            LineObjectCreator lineObjectCreator = new LineObjectCreator();
            ParameterizeLineObject(lineObjectCreator);
            LineObject lineObject = lineObjectCreator.Create(LINE_NUMBER_ONE);

            Assert.AreEqual(LINE_NUMBER_ONE, lineObject.LineNumber);
            Assert.IsNotNull(lineObject.TranslationFile);
            Assert.IsNotNull(lineObject.Key);
            Assert.IsNotNull(lineObject.NestingStrings);
            Assert.IsNotNull(lineObject.NameSpaces);
            Assert.IsNotNull(lineObject.ColorCodes);
            Assert.IsNotNull(lineObject.Icons);
        }

        [TestMethod]
        public void TestMethodCreateWithAdditionalDataTwice()
        {
            LineObjectCreator lineObjectCreator = new LineObjectCreator();
            ParameterizeLineObject(lineObjectCreator);
            LineObject lineObject = lineObjectCreator.Create(LINE_NUMBER_TWO);

            Assert.AreEqual(LINE_NUMBER_TWO, lineObject.LineNumber);
            Assert.IsNotNull(lineObject.TranslationFile);
            Assert.IsNotNull(lineObject.Key);
            Assert.IsNotNull(lineObject.NestingStrings);
            Assert.IsNotNull(lineObject.NameSpaces);
            Assert.IsNotNull(lineObject.ColorCodes);
            Assert.IsNotNull(lineObject.Icons);

            lineObject = lineObjectCreator.Create(2);
            Assert.AreEqual(LINE_NUMBER_TWO, lineObject.LineNumber);
            Assert.IsNotNull(lineObject.TranslationFile);
            Assert.IsNull(lineObject.Key);
            Assert.IsNull(lineObject.NestingStrings);
            Assert.IsNull(lineObject.NameSpaces);
            Assert.IsNull(lineObject.ColorCodes);
            Assert.IsNull(lineObject.Icons);

        }
    }
}

