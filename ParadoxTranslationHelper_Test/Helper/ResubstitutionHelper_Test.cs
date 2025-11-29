using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using ParadoxTranslationHelper.Helper;
using System.Collections.Generic;

namespace ParadoxTranslationHelper_Test;

[TestClass]
public class ResubstitutionHelper_Test
{
    [TestMethod]
    [DataRow(DisplayName = "Test for NE")]
    public void TestMethod_001()
    {
        string lineToTest = "bunker_desc:0\t„Dieses Gebäude verursacht im Kampf für jede Festungsstufe einen Angriffsmalus von $VALUE|+%0$ für den Angreifer. Angriffe aus mehreren Richtungen verringern die Wirkung der Festungen.|___NE16___|“";
        LineObject lineObject = new LineObject(0);
        lineObject.OriginalLine = lineToTest;
        lineObject.OriginalLineSubstituted = lineToTest;
        List<LineObject> lineObjects = new List<LineObject> { lineObject };

        Dictionary<string, string> substituteTokens = new Dictionary<string, string>();
        substituteTokens.Add("|___NE16___|", "$bunker_effect$");

        ResubstitutionHelper resubstitutionHelper = new ResubstitutionHelper();
        resubstitutionHelper.ReSubstituteLines(lineObjects, ref substituteTokens);

        Assert.IsFalse(lineObjects[0].OriginalLineSubstituted.Contains(FileSubstitutionConstants.SUBSTITUTION_START));
        Assert.IsFalse(lineObjects[0].OriginalLineSubstituted.Contains(FileSubstitutionConstants.SUBSTITUTION_END));
    }
}
