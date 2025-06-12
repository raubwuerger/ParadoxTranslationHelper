using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParadoxTranslationHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper.Helper;
using ParadoxTranslationHelper.Utilities;
using ParadoxTranslationHelper.Comparator;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class ComparatorNestedString_Test
    {
        ComparatorNestedString comparatorNestedString = new ComparatorNestedString();
        List<string> _empty = new List<string>();
        List<string> _oneA = new List<string>();
        List<string> _oneB = new List<string>();
        List<string> _twoAB = new List<string>();
        List<string> _twoBA = new List<string>();

        [TestInitialize()]
        public void TestInitialize()
        {
            _oneA.Add("A");
            _oneB.Add("B");
            
            _twoAB.Add("A");
            _twoAB.Add("B");

            _twoBA.Add("B");
            _twoBA.Add("A");
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: null,null --> false")]
        public void DiffNestingStrings_001()
        {
            comparatorNestedString.Compare(null, null);
            Assert.IsFalse(comparatorNestedString.Ok());
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: empty,null --> false")]
        public void DiffNestingStrings_002()
        {
            comparatorNestedString.Compare(_empty, null);
            Assert.IsFalse(comparatorNestedString.Ok());
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: valid,null --> false")]
        public void DiffNestingStrings_003()
        {
            comparatorNestedString.Compare(_oneA, null);
            Assert.IsFalse(comparatorNestedString.Ok());
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: null,empty --> false")]
        public void DiffNestingStrings_004()
        {
            comparatorNestedString.Compare(null, _empty);
            Assert.IsFalse(comparatorNestedString.Ok());
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: null,valid --> false")]
        public void DiffNestingStrings_005()
        {
            comparatorNestedString.Compare(null, _oneA);
            Assert.IsFalse(comparatorNestedString.Ok());
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: empty,empty --> true")]
        public void DiffNestingStrings_006()
        {
            comparatorNestedString.Compare(_empty, _empty);
            Assert.IsTrue(comparatorNestedString.Ok());
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: a,b --> false")]
        public void DiffNestingStrings_007()
        {
            comparatorNestedString.Compare(_oneA, _oneB);
            Assert.IsFalse(comparatorNestedString.Ok());
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: ab,b --> false")]
        public void DiffNestingStrings_008()
        {
            comparatorNestedString.Compare(_twoAB, _oneB);
            Assert.IsFalse(comparatorNestedString.Ok());
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: ab,ab --> true")]
        public void DiffNestingStrings_010()
        {
            comparatorNestedString.Compare(_twoAB, _twoAB);
            Assert.IsTrue(comparatorNestedString.Ok());
        }

        [TestMethod]
        [DataRow(DisplayName = "DiffNestingStrings: ab,ba --> true")]
        public void DiffNestingStrings_011()
        {
            comparatorNestedString.Compare(_twoAB, _twoBA);
            Assert.IsTrue(comparatorNestedString.Ok());
        }
    }
}