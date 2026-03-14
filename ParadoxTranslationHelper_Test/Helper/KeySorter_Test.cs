using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using ParadoxTranslationHelper.Helper;
using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class KeySorter_Test
    {
        List<TranslationFile> _localisationFilesSteam;

        const string fileNameSteam = "focus_l_english.yml";

        [TestInitialize]
        public void TestInitialize()
        {
            String steamFile = Path.Combine(Helper.GetTestLocation(), Helper.TEST_DATA, Helper.FUNCTIONS, Helper.VALIDATE, Helper.LOCALISATION, Helper.STEAM);
            _localisationFilesSteam = FileUtility.CreateTranslationFilesFromDirectory(steamFile);
        }

        [TestMethod]
        [DataRow(DisplayName = "translated file null entries")]
        public void TranslatedFileNoEntries()
        {
            String pathTranslated = Path.Combine(Helper.GetTestLocation(), Helper.TEST_DATA, Helper.FUNCTIONS, Helper.VALIDATE, Helper.LOCALISATION, "german_empty");
            List<TranslationFile> translated = FileUtility.CreateTranslationFilesFromDirectory(pathTranslated);
            
            KeySorter keySorter = new KeySorter();
            keySorter.TranslatedKeys = translated[0].Lines;
            keySorter.OriginalKeys = _localisationFilesSteam[0].Lines;

            keySorter.Sort();

            Dictionary<int, LineObject> sorted = keySorter.SortedKeys;
            Assert.AreEqual(keySorter.OriginalKeys.Count,sorted.Count);
        }

        [TestMethod]
        [DataRow(DisplayName = "translated file equals original")]
        public void TranslatedFileEqualsOriginal()
        {
        }

    }
}

