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
    public class FunctionObjectRegistry_Test
    {
        [TestInitialize]
        public void TestInitialize()
        {
            FunctionObjectRegistry.Instance.Clear();
        }

        [TestMethod]
        [DataRow(DisplayName = "Register: null --> false")]
        public void TestMethod1()
        {
            Assert.IsFalse(FunctionObjectRegistry.Instance.Register(null));
        }

        [TestMethod]
        [DataRow(DisplayName = "Register: valid.name = null --> false")]
        public void TestMethod2()
        {
            Assert.IsFalse(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse(null)));
        }

        [TestMethod]
        [DataRow(DisplayName = "Register: valid.name.empty --> false")]
        public void TestMethod3()
        {
            Assert.IsFalse(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("")));
        }

        [TestMethod]
        [DataRow(DisplayName = "Register: valid.name --> true")]
        public void TestMethod4()
        {
            Assert.IsTrue(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("func")));
        }

        [TestMethod]
        [DataRow(DisplayName = "Register: valid.name x 2 --> false")]
        public void TestMethod5()
        {
            Assert.IsTrue(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("func")));
            Assert.IsFalse(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("func")));
        }

        [TestMethod]
        [DataRow(DisplayName = "Register: valid.names --> true")]
        public void TestMethod6()
        {
            Assert.IsTrue(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("func")));
            Assert.IsTrue(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("func_2")));
        }

        [TestMethod]
        [DataRow(DisplayName = "Clear: --> 0")]
        public void TestMethod7()
        {
            Assert.IsTrue(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("func")));
            Assert.AreEqual(1, FunctionObjectRegistry.Instance.Count());
            FunctionObjectRegistry.Instance.Clear();
            Assert.AreEqual(0, FunctionObjectRegistry.Instance.Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Count: empty --> 0")]
        public void TestMethod8()
        {
            Assert.AreEqual(0, FunctionObjectRegistry.Instance.Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Count: one --> 1")]
        public void TestMethod9()
        {
            Assert.IsTrue(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("func")));
            Assert.AreEqual(1, FunctionObjectRegistry.Instance.Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Count: one x 2 --> 1")]
        public void TestMethod10()
        {
            Assert.IsTrue(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("func")));
            Assert.IsFalse(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("func")));
            Assert.AreEqual(1, FunctionObjectRegistry.Instance.Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Count: two --> 2")]
        public void TestMethod11()
        {
            Assert.IsTrue(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("func")));
            Assert.IsTrue(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("func_2")));
            Assert.AreEqual(2, FunctionObjectRegistry.Instance.Count());
        }

        [TestMethod]
        [DataRow(DisplayName = "Remove: null --> null")]
        public void TestMethod12()
        {
            Assert.IsNull(FunctionObjectRegistry.Instance.RemoveFunctionObject(null));
        }

        [TestMethod]
        [DataRow(DisplayName = "Remove: empty --> null")]
        public void TestMethod13()
        {
            Assert.IsNull(FunctionObjectRegistry.Instance.RemoveFunctionObject(""));
        }

        [TestMethod]
        [DataRow(DisplayName = "Remove: one entry wrong name --> null")]
        public void TestMethod14()
        {
            Assert.IsTrue(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("func")));
            Assert.IsNull(FunctionObjectRegistry.Instance.RemoveFunctionObject("func_2"));
        }

        [TestMethod]
        [DataRow(DisplayName = "Remove: one entry valid name --> not null")]
        public void TestMethod15()
        {
            Assert.IsTrue(FunctionObjectRegistry.Instance.Register(new FunctionObjectAnalyse("func")));
            Assert.IsNotNull(FunctionObjectRegistry.Instance.RemoveFunctionObject("func"));
        }
    }
}
