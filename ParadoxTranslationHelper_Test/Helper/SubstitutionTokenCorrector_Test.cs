using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using ParadoxTranslationHelper.Helper;
using ParadoxTranslationHelper.Utilities;
using System.IO;

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
    }

    [TestMethod]
    [DataRow(DisplayName = "line empty -> null")]
    public void TestMethod002()
    {
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        Assert.IsNull(corrector.CorrectLine(""));
    }

    [TestMethod]
    [DataRow(DisplayName = "line to short -> null")]
    public void TestMethod003()
    {
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        Assert.IsNull(corrector.CorrectLine("LineToShort"));
    }

    [TestMethod]
    [DataRow(DisplayName = "corrupt token to short -> null")]
    public void TestMethod004()
    {
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        Assert.IsNull(corrector.CorrectLine("asds|___KY0000 ___|klljlkjs"));
    }

    [TestMethod]
    [DataRow(DisplayName = "corrupt token to short End -> null")]
    public void TestMethod005()
    {
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        Assert.IsNull(corrector.CorrectLine("asdklljlkjss|___KY0000 ___|"));
    }

    [TestMethod]
    [DataRow(DisplayName = "corrupt token without end tag -> null")]
    public void TestMethod006()
    {
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        Assert.IsNull(corrector.CorrectLine("asdklljlkjss|___KY000012___"));
    }

    [TestMethod]
    [DataRow(DisplayName = "corrupt token -> null")]
    public void TestMethod010()
    {
        string toCorrect = "asdklljl|___NL00 0072___|asas";
        SubstitutionTokenCorrector corrector = new SubstitutionTokenCorrector();
        string corrected = corrector.CorrectLine(toCorrect);
        Assert.IsNotNull(corrected);
        Assert.AreEqual(toCorrect.Length, corrected.Length + 1);
    }
    
}

