using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using ParadoxTranslationHelper.Helper;
using ParadoxTranslationHelper.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParadoxTranslationHelper_Test;

[TestClass]
public class SubstitutionTokenCorrector_Test
{
    TranslationFile translationFile;

    [TestInitialize]
    public void TestInitialize()
    {
        translationFile = FileUtility.CreateTranslationFileFromFile(@"C:\Projects\ParadoxTranslationHelper\ParadoxTranslationHelper_Test\testData\SubstitutionTokenCorrector\translationFileWithCorruptTokens.yml");
        Assert.IsNotNull(translationFile);
    }


    [TestMethod]
    [DataRow(DisplayName = "line null -> null")]
    public void TestMethod001()
    {
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        Assert.IsNull(corrector.CorrectLine(null));
        Assert.AreEqual(0, corrector.FoundToken);
        Assert.IsTrue(corrector.HasNoTokenAtAll);
        Assert.IsFalse(corrector.ContainsAtLeastOneCorruptToken);
    }

    [TestMethod]
    [DataRow(DisplayName = "line empty -> null")]
    public void TestMethod002()
    {
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        Assert.IsNull(corrector.CorrectLine(""));
        Assert.AreEqual(0, corrector.FoundToken);
        Assert.IsTrue(corrector.HasNoTokenAtAll);
        Assert.IsFalse(corrector.ContainsAtLeastOneCorruptToken);
    }

    [TestMethod]
    [DataRow(DisplayName = "line to short -> null")]
    public void TestMethod003()
    {
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        Assert.IsNull(corrector.CorrectLine("LineToShort"));
        Assert.AreEqual(0, corrector.FoundToken);
        Assert.IsTrue(corrector.HasNoTokenAtAll);
        Assert.IsFalse(corrector.ContainsAtLeastOneCorruptToken);
    }

    [TestMethod]
    [DataRow(DisplayName = "corrupt token to short -> null")]
    public void TestMethod004()
    {
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        Assert.IsNull(corrector.CorrectLine("asds|___KY0000 ___|klljlkjs"));
        Assert.AreEqual(1, corrector.FoundToken);
        Assert.IsFalse(corrector.HasNoTokenAtAll);
        Assert.IsFalse(corrector.ContainsAtLeastOneCorruptToken);
    }

    [TestMethod]
    [DataRow(DisplayName = "corrupt token to short End -> null")]
    public void TestMethod005()
    {
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        Assert.IsNull(corrector.CorrectLine("asdklljlkjss|___KY0000 ___|"));
        Assert.AreEqual(1, corrector.FoundToken);
        Assert.IsFalse(corrector.HasNoTokenAtAll);
        Assert.IsFalse(corrector.ContainsAtLeastOneCorruptToken);
    }

    [TestMethod]
    [DataRow(DisplayName = "corrupt token without end tag -> null")]
    public void TestMethod006()
    {
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        Assert.IsNull(corrector.CorrectLine("asdklljlkjss|___KY000012___"));
        Assert.AreEqual(0, corrector.FoundToken);
        Assert.IsFalse(corrector.HasNoTokenAtAll);
        Assert.IsTrue(corrector.ContainsAtLeastOneCorruptToken);
    }

    [TestMethod]
    [DataRow(DisplayName = "corrupt token -> not null")]
    public void TestMethod010()
    {
        string toCorrect = "asdklljl|___NL00 0072___|asas";
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        string corrected = corrector.CorrectLine(toCorrect);
        Assert.IsNotNull(corrected);
        Assert.AreEqual(toCorrect.Length, corrected.Length + 1);
        Assert.AreEqual(1, corrector.FoundToken);
        Assert.IsFalse(corrector.HasNoTokenAtAll);
        Assert.IsFalse(corrector.ContainsAtLeastOneCorruptToken);

    }

    [TestMethod]
    [DataRow(DisplayName = "corrupt token x 2 -> not null")]
    public void TestMethod011()
    {
        string toCorrect = "asdklljl|___NL00 0072___|asas lljl|___NL00 0073___|dsdgf";
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        string corrected = corrector.CorrectLine(toCorrect);
        Assert.IsNotNull(corrected);
        Assert.AreEqual(toCorrect.Length, corrected.Length + 2);
        Assert.AreEqual(2, corrector.FoundToken);
        Assert.IsFalse(corrector.HasNoTokenAtAll);
        Assert.IsFalse(corrector.ContainsAtLeastOneCorruptToken);
    }

    [TestMethod]
    [DataRow(DisplayName = "corrupt token, correct token -> not null")]
    public void TestMethod012()
    {
        string toCorrect = "asdklljl|___NL00 0072___|asas lljl|___NL000073___|dsdgf";
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        string corrected = corrector.CorrectLine(toCorrect);
        Assert.IsNotNull(corrected);
        Assert.AreEqual(toCorrect.Length, corrected.Length + 1);
        Assert.AreEqual(2, corrector.FoundToken);
        Assert.IsFalse(corrector.HasNoTokenAtAll);
        Assert.IsFalse(corrector.ContainsAtLeastOneCorruptToken);
    }

    [TestMethod]
    [DataRow(DisplayName = "corrupt token, corrupt token, correct token -> not null")]
    public void TestMethod013()
    {
        string toCorrect = "asdklljl|___NL00 0072___|asaljl|___NL00 0073___|ass lljl|___NL000075___|dsdgf";
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        string corrected = corrector.CorrectLine(toCorrect);
        Assert.IsNotNull(corrected);
        Assert.AreEqual(toCorrect.Length, corrected.Length + 2);
        Assert.AreEqual(3, corrector.FoundToken);
        Assert.IsFalse(corrector.HasNoTokenAtAll);
        Assert.IsFalse(corrector.ContainsAtLeastOneCorruptToken);
    }

    [TestMethod]
    [DataRow(DisplayName = "corrupt token, corrupt token, correct token, invalid token -> null")]
    public void TestMethod014()
    {
        string toCorrect = "asdklljl|___NL00 0072___|asaljl|___NL00 0073___|ass lljl|___NL000075___|dsdgf|___NL00 ";
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        string corrected = corrector.CorrectLine(toCorrect);
        Assert.IsNotNull(corrected);
        Assert.AreEqual(toCorrect.Length, corrected.Length + 2);
        Assert.AreEqual(3, corrector.FoundToken);
        Assert.IsFalse(corrector.HasNoTokenAtAll);
        Assert.IsTrue(corrector.ContainsAtLeastOneCorruptToken);
    }

    [TestMethod]
    [DataRow(DisplayName = "9 valid token, corrupt token -> not null")]
    public void TestMethod015()
    {
        string toCorrect = "decision_cost_Manpower1000_Guns_1000_cp_25: |___IC000000___|  |___CC000208___|1000|___CC000209___| |___IC000001___|  |___CC000210___|1000|___CC000211___||___NL000031___|| ___IC000002___|  |___CC000212___|25|___CC000213___|";
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        string corrected = corrector.CorrectLine(toCorrect);
        Assert.IsNotNull(corrected);
        Assert.AreEqual(toCorrect.Length, corrected.Length + 1);
        Assert.AreEqual(10, corrector.FoundToken);
        Assert.IsFalse(corrector.HasNoTokenAtAll);
        Assert.IsFalse(corrector.ContainsAtLeastOneCorruptToken);
    }

    [TestMethod]
    [DataRow(DisplayName = "no token at all -> null")]
    public void TestMethod016()
    {
        string toCorrect = "decision_cost_Manpower1000_Guns_1000_cp_25: No Token at all!";
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        string corrected = corrector.CorrectLine(toCorrect);
        Assert.IsNull(corrected);
        Assert.AreEqual(0, corrector.FoundToken);
        Assert.IsTrue(corrector.HasNoTokenAtAll);
        Assert.IsFalse(corrector.ContainsAtLeastOneCorruptToken);
    }

    [TestMethod]
    [DataRow(DisplayName = "ono incorrect -> null")]
    public void TestMethod017()
    {
        string toCorrect = "decision_cost_Manpower1000_Guns_1000_cp_25: £manpower_texticon  §Y1000§! £infantry_equipment_text_icon  §Y1000§!\\n| ___IC000002___|  §Y25§!";
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        string corrected = corrector.CorrectLine(toCorrect);
        Assert.IsNotNull(corrected);
        Assert.AreEqual(1, corrector.FoundToken);
        Assert.IsFalse(corrector.HasNoTokenAtAll);
        Assert.IsFalse(corrector.ContainsAtLeastOneCorruptToken);
    }

    [TestMethod]
    [DataRow(DisplayName = "Real test")]
    public void TestMethod999()
    {
        List<LineObject> lineObjects = translationFile.Lines.Values.ToList<LineObject>();
        foreach (LineObject lineObject in lineObjects )
        {
            SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
            lineObject.OriginalLineSubstituted = corrector.CorrectLine(lineObject.OriginalLine);
        }
    }
}

