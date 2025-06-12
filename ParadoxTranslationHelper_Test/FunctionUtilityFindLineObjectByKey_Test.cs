using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParadoxTranslationHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper.Helper;
using ParadoxTranslationHelper.Utilities;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class FunctionUtilityFindLineObjectByKey_Test
    {
        static string validKey1 = "validKey1";
        static string validKey2 = "validKey2";
        static string validKey3 = "validKey3";
        static string wrongKey = "wrongKey";

        [TestMethod]
        [DataRow(DisplayName = "FindLineObjectByKey: null,null --> null")]
        public void FindLineObjectByKey_001()
        {
            Assert.IsNull(FunctionUtility.FindLineObjectByKey(null,null));
        }

        [TestMethod]
        [DataRow(DisplayName = "FindLineObjectByKey: valid,null --> null")]
        public void FindLineObjectByKey_002()
        {
            Assert.IsNull(FunctionUtility.FindLineObjectByKey(validKey1, null));
        }

        [TestMethod]
        [DataRow(DisplayName = "FindLineObjectByKey: null,empty --> null")]
        public void FindLineObjectByKey_003()
        {
            Assert.IsNull(FunctionUtility.FindLineObjectByKey(null, new List<LineObject>()));
        }

        [TestMethod]
        [DataRow(DisplayName = "FindLineObjectByKey: valid,empty --> null")]
        public void FindLineObjectByKey_004()
        {
            Assert.IsNull(FunctionUtility.FindLineObjectByKey(validKey1, new List<LineObject>()));
        }

        [TestMethod]
        [DataRow(DisplayName = "FindLineObjectByKey: valid,no key --> null")]
        public void FindLineObjectByKey_005()
        {
            LineObject lineObject = new LineObject(1);
            Assert.IsNull(FunctionUtility.FindLineObjectByKey(validKey1, new List<LineObject> { lineObject } ));
        }

        [TestMethod]
        [DataRow(DisplayName = "FindLineObjectByKey: valid,wrong key --> null")]
        public void FindLineObjectByKey_006()
        {
            LineObject lineObject = new LineObject(1);
            lineObject.Key = wrongKey;
            Assert.IsNull(FunctionUtility.FindLineObjectByKey(validKey1, new List<LineObject> { lineObject }));
        }

        [TestMethod]
        [DataRow(DisplayName = "FindLineObjectByKey: valid, valid key --> not null")]
        public void FindLineObjectByKey_010()
        {
            LineObject lineObject1 = new LineObject(1);
            lineObject1.Key = validKey1;
            LineObject lineObject2 = new LineObject(1);
            lineObject2.Key = validKey2;
            LineObject lineObject3 = new LineObject(1);
            lineObject3.Key = validKey3;

            Assert.IsNotNull(FunctionUtility.FindLineObjectByKey(validKey1, new List<LineObject> { lineObject2, lineObject1, lineObject3 }));
        }
    }
}