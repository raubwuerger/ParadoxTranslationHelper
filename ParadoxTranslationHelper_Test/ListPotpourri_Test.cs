using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class ListPotpourri_Test
    {
        List<string> listItemsNone = new List<string>();

        List<string> listItemsA = new List<string>();
        List<string> listItemsAB = new List<string>();
        List<string> listItemsABC = new List<string>();
        List<string> listItemsB = new List<string>();
        List<string> listItemsAC = new List<string>();
        List<string> listItemsBC = new List<string>();
        List<string> listItemsC = new List<string>();

        List<string> listItemsX = new List<string>();
        List<string> listItemsXY = new List<string>();
        List<string> listItemsXYZ = new List<string>();


        string a = "a";
        string b = "b";
        string c = "c";
        string d = "d";

        string x = "x";
        string y = "y";
        string z = "z"; 

        [TestInitialize]
        public void Initialize()
        {
            listItemsA.Add(a);

            listItemsAB.Add(a);
            listItemsAB.Add(b);

            listItemsABC.Add(a);
            listItemsABC.Add(b);
            listItemsABC.Add(c);
            
            listItemsAC.Add(a);
            listItemsAC.Add(c);

            listItemsB.Add(b);

            listItemsBC.Add(b);
            listItemsBC.Add(c);

            listItemsC.Add(c);

            listItemsX.Add(x);

            listItemsXY.Add(x);
            listItemsXY.Add(y);

            listItemsXYZ.Add(x);
            listItemsXYZ.Add(y);
            listItemsXYZ.Add(z);
        }

        [TestMethod]
        public void TestMethod1()
        {
            List<string> list_A_except_None = listItemsA.Except(listItemsNone).ToList<string>();
            List<string> list_A_except_A = listItemsA.Except(listItemsA).ToList<string>();
            List<string> list_A_except_B = listItemsA.Except(listItemsB).ToList<string>();
            List<string> list_A_except_C = listItemsA.Except(listItemsC).ToList<string>();
            List<string> list_A_except_AB = listItemsA.Except(listItemsAB).ToList<string>();
            List<string> list_A_except_ABC = listItemsA.Except(listItemsABC).ToList<string>();

            List<string> list_B_except_None = listItemsB.Except(listItemsNone).ToList<string>();
            List<string> list_B_except_B = listItemsB.Except(listItemsB).ToList<string>();
            List<string> list_B_except_A = listItemsB.Except(listItemsA).ToList<string>();
            List<string> list_B_except_C = listItemsB.Except(listItemsC).ToList<string>();
            List<string> list_B_except_AB = listItemsB.Except(listItemsAB).ToList<string>();
            List<string> list_B_except_ABC = listItemsB.Except(listItemsABC).ToList<string>();


            List<string> list_AB_except_None = listItemsAB.Except(listItemsNone).ToList<string>();
            List<string> list_AB_except_A = listItemsAB.Except(listItemsA).ToList<string>();
            List<string> list_AB_except_B = listItemsAB.Except(listItemsB).ToList<string>();
            List<string> list_AB_except_C = listItemsAB.Except(listItemsC).ToList<string>();
            List<string> list_AB_except_AC = listItemsAB.Except(listItemsAC).ToList<string>();
            List<string> list_AB_except_ABC = listItemsAB.Except(listItemsABC).ToList<string>();

            List<string> list_AC_except_None = listItemsAC.Except(listItemsNone).ToList<string>();
            List<string> list_AC_except_A = listItemsAC.Except(listItemsA).ToList<string>();
            List<string> list_AC_except_B = listItemsAC.Except(listItemsB).ToList<string>();
            List<string> list_AC_except_C = listItemsAC.Except(listItemsC).ToList<string>();
            List<string> list_AC_except_AB = listItemsAC.Except(listItemsAB).ToList<string>();
            List<string> list_AC_except_ABC = listItemsAC.Except(listItemsABC).ToList<string>();

            List<string> list_ABC_except_None = listItemsABC.Except(listItemsNone).ToList<string>();
            List<string> list_ABC_except_A = listItemsABC.Except(listItemsA).ToList<string>();
            List<string> list_ABC_except_AB = listItemsABC.Except(listItemsAB).ToList<string>();


            List<string> inSecondOnly = listItemsAB.Except(listItemsA).ToList<string>();
            Assert.AreEqual(list_A_except_AB, inSecondOnly);
        }
    }
}
