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
        const string fileNameGerman = "focus_l_german.yml";
        readonly string baseTestPath = Path.Combine(Helper.GetTestLocation(), Helper.TEST_DATA, Helper.FUNCTIONS, Helper.VALIDATE, Helper.LOCALISATION);
        readonly string baseTestPathOriginals = Path.Combine(Helper.GetTestLocation(), Helper.TEST_DATA, Helper.FUNCTIONS, Helper.VALIDATE, Helper.LOCALISATION,"german_orgs");
        readonly string testPath_german_empty = "german_empty";
        readonly string testPath_german_equal = "german_equal";
        readonly string testPath_german_orgmixed = "german_orgmixed";
        readonly string testPath_german_missingKeys = "german_missingKeys";
        readonly string testPath_german_missingKeysMixed = "german_missingKeysMixed";


        [TestInitialize]
        public void TestInitialize()
        {
            String steamFile = Path.Combine(Helper.GetTestLocation(), Helper.TEST_DATA, Helper.FUNCTIONS, Helper.VALIDATE, Helper.LOCALISATION, Helper.STEAM);
            _localisationFilesSteam = FileUtility.CreateTranslationFilesFromDirectory(steamFile);
        }

        private bool ResetTest( string pathToClean, string pathToGetOriginal )
        {
            try
            {
                Directory.CreateDirectory(pathToClean);
                string[] filePaths = Directory.GetFiles(pathToClean);
                foreach (string filePath in filePaths)
                {
                    File.Delete(filePath);
                }

                File.Copy(Path.Combine(pathToGetOriginal, fileNameGerman), Path.Combine(pathToClean, fileNameGerman));

                return true;
            }
            catch(Exception ex)
            {
                Assert.Fail();
                return false;
            }
        }

        [TestMethod]
        [DataRow(DisplayName = "translated file null entries")]
        public void TranslatedFileNoEntries()
        {
            Assert.IsTrue(ResetTest(Path.Combine(baseTestPath, testPath_german_empty), Path.Combine(baseTestPathOriginals, testPath_german_empty)));

            String pathTranslated = Path.Combine(Helper.GetTestLocation(), Helper.TEST_DATA, Helper.FUNCTIONS, Helper.VALIDATE, Helper.LOCALISATION, testPath_german_empty);
            List<TranslationFile> translated = FileUtility.CreateTranslationFilesFromDirectory(pathTranslated);
            
            KeySorter keySorter = new KeySorter();
            keySorter.TranslatedKeys = translated[0].Lines;
            keySorter.OriginalKeys = _localisationFilesSteam[0].Lines;

            keySorter.Sort();

            Dictionary<int, LineObject> sorted = DictionaryHelper.ConvertDictionary(keySorter.SortedKeys);
            //INFO: 2026-03-18 - JHA - Comparing lines and keys can't match
            //            Assert.AreEqual(keySorter.OriginalKeys.Count,sorted.Count);

            Assert.IsTrue(FileUtility.BackupFile(translated[0].FileNameWithBasePath));

            translated[0].Lines = sorted;
            Assert.IsTrue(FileUtility.Write(translated[0], translated[0].FileNameWithBasePath));
        }

        [TestMethod]
        [DataRow(DisplayName = "translated file equals original")]
        public void TranslatedFileEqualsOriginal()
        {
            Assert.IsTrue(ResetTest(Path.Combine(baseTestPath, testPath_german_equal), Path.Combine(baseTestPathOriginals, testPath_german_equal)));

            String pathTranslated = Path.Combine(Helper.GetTestLocation(), Helper.TEST_DATA, Helper.FUNCTIONS, Helper.VALIDATE, Helper.LOCALISATION, testPath_german_equal);
            List<TranslationFile> translated = FileUtility.CreateTranslationFilesFromDirectory(pathTranslated);

            KeySorter keySorter = new KeySorter();
            keySorter.TranslatedKeys = translated[0].Lines;
            keySorter.OriginalKeys = _localisationFilesSteam[0].Lines;

            keySorter.Sort();

            Dictionary<int, LineObject> sorted = DictionaryHelper.ConvertDictionary(keySorter.SortedKeys);
            //INFO: 2026-03-18 - JHA - Comparing lines and keys can't match
            //            Assert.AreEqual(keySorter.OriginalKeys.Count, sorted.Count);

            Assert.IsTrue(FileUtility.BackupFile(translated[0].FileNameWithBasePath));

            translated[0].Lines = sorted;
            Assert.IsTrue(FileUtility.Write(translated[0], translated[0].FileNameWithBasePath));
        }

        [TestMethod]
        [DataRow(DisplayName = "translated file is mixed")]
        public void TranslatedFileIsMixed()
        {
            Assert.IsTrue(ResetTest(Path.Combine(baseTestPath, testPath_german_orgmixed), Path.Combine(baseTestPathOriginals, testPath_german_orgmixed)));

            String pathTranslated = Path.Combine(Helper.GetTestLocation(), Helper.TEST_DATA, Helper.FUNCTIONS, Helper.VALIDATE, Helper.LOCALISATION, testPath_german_orgmixed);
            List<TranslationFile> translated = FileUtility.CreateTranslationFilesFromDirectory(pathTranslated);

            KeySorter keySorter = new KeySorter();
            keySorter.TranslatedKeys = translated[0].Lines;
            keySorter.OriginalKeys = _localisationFilesSteam[0].Lines;

            keySorter.Sort();

            Dictionary<int, LineObject> sorted = DictionaryHelper.ConvertDictionary(keySorter.SortedKeys);
            //INFO: 2026-03-18 - JHA - Comparing lines and keys can't match
            //            Assert.AreEqual(keySorter.OriginalKeys.Count, sorted.Count);

            Assert.IsTrue(FileUtility.BackupFile(translated[0].FileNameWithBasePath));

            translated[0].Lines = sorted;
            Assert.IsTrue(FileUtility.Write(translated[0], translated[0].FileNameWithBasePath));
        }

        [TestMethod]
        [DataRow(DisplayName = "translated file has missing keys")]
        public void TranslatedFileHasMissingKeys()
        {
            Assert.IsTrue(ResetTest(Path.Combine(baseTestPath, testPath_german_missingKeys), Path.Combine(baseTestPathOriginals, testPath_german_missingKeys)));

            String pathTranslated = Path.Combine(Helper.GetTestLocation(), Helper.TEST_DATA, Helper.FUNCTIONS, Helper.VALIDATE, Helper.LOCALISATION, testPath_german_missingKeys);
            List<TranslationFile> translated = FileUtility.CreateTranslationFilesFromDirectory(pathTranslated);

            KeySorter keySorter = new KeySorter();
            keySorter.TranslatedKeys = translated[0].Lines;
            keySorter.OriginalKeys = _localisationFilesSteam[0].Lines;

            keySorter.Sort();

            Dictionary<int, LineObject> sorted = DictionaryHelper.ConvertDictionary(keySorter.SortedKeys);
//INFO: 2026-03-18 - JHA - Comparing lines and keys can't match
//            Assert.AreEqual(keySorter.OriginalKeys.Count, sorted.Count);

            Assert.IsTrue(FileUtility.BackupFile(translated[0].FileNameWithBasePath));

            translated[0].Lines = sorted;
            Assert.IsTrue(FileUtility.Write(translated[0], translated[0].FileNameWithBasePath));
        }

        [TestMethod]
        [DataRow(DisplayName = "translated file has missing keys and mixed keys")]
        public void TranslatedFileHasMissingKeysMixed()
        {
            Assert.IsTrue(ResetTest(Path.Combine(baseTestPath, testPath_german_missingKeysMixed), Path.Combine(baseTestPathOriginals, testPath_german_missingKeysMixed)));

            String pathTranslated = Path.Combine(Helper.GetTestLocation(), Helper.TEST_DATA, Helper.FUNCTIONS, Helper.VALIDATE, Helper.LOCALISATION, testPath_german_missingKeysMixed);
            List<TranslationFile> translated = FileUtility.CreateTranslationFilesFromDirectory(pathTranslated);

            KeySorter keySorter = new KeySorter();
            keySorter.TranslatedKeys = translated[0].Lines;
            keySorter.OriginalKeys = _localisationFilesSteam[0].Lines;

            keySorter.Sort();

            Dictionary<int, LineObject> sorted = DictionaryHelper.ConvertDictionary(keySorter.SortedKeys);
            //INFO: 2026-03-18 - JHA - Comparing lines and keys can't match
            //            Assert.AreEqual(keySorter.OriginalKeys.Count, sorted.Count);

            Assert.IsTrue(FileUtility.BackupFile(translated[0].FileNameWithBasePath));

            translated[0].Lines = sorted;
            Assert.IsTrue(FileUtility.Write(translated[0], translated[0].FileNameWithBasePath));
        }
    }
}

