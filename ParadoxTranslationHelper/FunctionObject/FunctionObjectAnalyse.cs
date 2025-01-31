using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectAnalyse : FunctionObjectBase
    {
        public FunctionObjectAnalyse(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            LocalisationEnglish = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathEnglish);
            LocalisationEnglishUpdated = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathEnglishUpdated);
            LocalisationGerman = Utility.CreateTranslationFilesFromDirectory(ParadoxTranslationHelperConfig.PathGerman);

            CheckTranslationFilesMissingUpdate();
            CheckTranslationFilesDeletedUpdate();

            CheckMissingTranslationFile();
            CheckMissingKeys();


            return false;
        }

        private void CheckTranslationFilesMissingUpdate()
        {
            if (false == LocalisationEnglishUpdated.Any() || false == LocalisationEnglish.Any())
            {
                Console.WriteLine("No translation files found!");
                return;
            }

            Console.WriteLine("Following transtlation files are no more existant in update: " + Path.GetFullPath(LocalisationEnglishUpdated[0].FileName) + Environment.NewLine);
            List<string> localisationFileNamesEnglish = LocalisationEnglish.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> localisationFileNamesEnglishUpdated = LocalisationEnglishUpdated.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> missingTranslationFiles = localisationFileNamesEnglish.Except(localisationFileNamesEnglishUpdated).ToList<string>();

            if (missingTranslationFiles.Count > 0)
            {
                foreach (string translationFile in missingTranslationFiles)
                {
                    Console.WriteLine(translationFile + Environment.NewLine);
                }
            }
        }

        private void CheckTranslationFilesDeletedUpdate()
        {
            if (false == LocalisationEnglishUpdated.Any() || false == LocalisationEnglish.Any())
            {
                Console.WriteLine("No translation files found!");
                return;
            }

            Console.WriteLine("Following transtlation files are new in update: " + Path.GetFullPath(LocalisationEnglishUpdated[0].FileName) + Environment.NewLine);
            List<string> localisationFileNamesEnglish = LocalisationEnglish.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> localisationFileNamesEnglishUpdated = LocalisationEnglishUpdated.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> translationFilesToDelete = localisationFileNamesEnglishUpdated.Except(localisationFileNamesEnglish).ToList<string>();

            foreach (string translationFile in translationFilesToDelete)
            {
                Console.WriteLine(translationFile + Environment.NewLine);
            }
        }

        private void CheckMissingTranslationFile()
        {
            List<string> localisationFileNamesEnglish = LocalisationEnglish.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> localisationFileNamesGerman = LocalisationGerman.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> missingTranslationFiles = localisationFileNamesEnglish.Except(localisationFileNamesGerman).ToList<string>();

            if (missingTranslationFiles.Count > 0)
            {
                Console.WriteLine("Following transtlation files are missing:" + Environment.NewLine);
                foreach (string translationFile in missingTranslationFiles)
                {
                    Console.WriteLine(translationFile + Environment.NewLine);
                }
            }
        }

        private void CheckMissingKeys()
        {
            DataSetLineObjectCompare dataSetLineObjectCompareEnglish = CreateDataSetLineObjectCompare(LocalisationEnglish);
            DataSetLineObjectCompare dataSetLineObjectCompareGerman = CreateDataSetLineObjectCompare(LocalisationGerman);

            LogMissingKeys(dataSetLineObjectCompareEnglish.keysUnique, dataSetLineObjectCompareGerman.keysUnique);
            LogMissingNamespaces(dataSetLineObjectCompareEnglish.keysUnique, dataSetLineObjectCompareGerman.keysUnique);
            LogMissingNestingStrings(dataSetLineObjectCompareEnglish.keysUnique, dataSetLineObjectCompareGerman.keysUnique);

            LogMultipleKeys(dataSetLineObjectCompareEnglish.keysMultiple);
            LogMultipleKeys(dataSetLineObjectCompareGerman.keysMultiple);
        }

        private DataSetLineObjectCompare CreateDataSetLineObjectCompare(List<TranslationFile> localisation)
        {
            DataSetLineObjectCompare dataSetLineObjectCompare = new DataSetLineObjectCompare();

            foreach (TranslationFile translationFile in localisation)
            {
                foreach (var item in translationFile.Lines)
                {
                    if (null == item.Value.Key)
                    {
                        continue;
                    }

                    if (true == dataSetLineObjectCompare.keysUnique.ContainsKey(item.Value.Key))
                    {
                        dataSetLineObjectCompare.keysMultiple.Add(item.Value);
                    }
                    else
                    {
                        dataSetLineObjectCompare.keysUnique.Add(item.Value.Key, item.Value);
                    }
                }
            }
            return dataSetLineObjectCompare;
        }

        private void LogMultipleKeys(List<LineObject> keysMultiple)
        {
            Console.WriteLine("##### Keys existing multiple times" + Environment.NewLine);
            keysMultiple.ForEach(key => { Console.WriteLine(key.TranslationFile + ": " + key.Key + ":" + key.LineNumber); });
            Console.WriteLine(Environment.NewLine);
        }

        private void LogMissingKeys(Dictionary<string, LineObject> keysBase, Dictionary<string, LineObject> keysShould)
        {
            Console.WriteLine("##### Missing keys #####" + Environment.NewLine);
            List<LineObject> missingKeys = new List<LineObject>();
            keysBase.ToList().ForEach
            (
                pair =>
                {
                    if (false == keysShould.ContainsKey(pair.Key))
                    {
                        missingKeys.Add(pair.Value);
                    }
                }
            );

            missingKeys.ForEach(keys => Console.WriteLine(keys.Key));
            Console.WriteLine(Environment.NewLine);
        }

        private void LogMissingNamespaces(Dictionary<string, LineObject> dictionaryEnglish, Dictionary<string, LineObject> dictionaryGerman)
        {
            Console.WriteLine("##### Missing namespaces [] #####" + Environment.NewLine);
            List<LineObject> missingNamespacesGerman = new List<LineObject>();
            List<LineObject> missingNamespacesEnglish = new List<LineObject>();

            dictionaryEnglish.ToList().ForEach
            (
                pair =>
                {
                    if (true == dictionaryGerman.ContainsKey(pair.Key))
                    {
                        LineObject lineObject = new LineObject(0);

                        if (true == dictionaryGerman.TryGetValue(pair.Key, out lineObject))
                        {
                            List<string> missingGermanNamespaces = pair.Value.NameSpaces.Except(lineObject.NameSpaces, StringComparer.OrdinalIgnoreCase).ToList();
                            if (missingGermanNamespaces.Count > 0)
                            {
                                missingNamespacesGerman.Add(CreateLineObjectMissingNamespaces(missingGermanNamespaces, pair));
                            }

                            List<string> missingEnglishNamespaces = lineObject.NameSpaces.Except(pair.Value.NameSpaces, StringComparer.OrdinalIgnoreCase).ToList();
                            if (missingEnglishNamespaces.Count > 0)
                            {
                                missingNamespacesEnglish.Add(CreateLineObjectMissingNamespaces(missingEnglishNamespaces, pair));
                            }
                        }
                    }
                }
            );

            string translationFileName = "";
            Console.WriteLine("##### Missing Namespaces [] german #####");
            missingNamespacesGerman.ForEach
            (
                item =>
                {
                    if (false == translationFileName.Equals(item.TranslationFile.FileName))
                    {
                        translationFileName = item.TranslationFile.FileName;
                        Console.WriteLine(translationFileName);
                    }
                    Console.WriteLine(item.Key + " (" + item.LineNumber + "): " + string.Join(", ", item.NameSpaces));
                }
            );
            Console.WriteLine();

            Console.WriteLine("##### Missing Namespaces [] english #####");
            missingNamespacesEnglish.ForEach
            (
                item =>
                {
                    if (false == translationFileName.Equals(item.TranslationFile.FileName))
                    {
                        translationFileName = item.TranslationFile.FileName;
                        Console.WriteLine(translationFileName);
                    }
                    Console.WriteLine(item.Key + " (" + item.LineNumber + "): " + string.Join(", ", item.NameSpaces));
                }
            );
        }

        private void LogMissingNestingStrings(Dictionary<string, LineObject> dictionaryEnglish, Dictionary<string, LineObject> dictionaryGerman)
        {
            Console.WriteLine("##### Missing NestingStrings $$ #####" + Environment.NewLine);
            List<LineObject> missingNestingStringsGerman = new List<LineObject>();
            List<LineObject> missingNestingStringsEnglish = new List<LineObject>();

            dictionaryEnglish.ToList().ForEach
            (
                pair =>
                {
                    if (true == dictionaryGerman.ContainsKey(pair.Key))
                    {
                        LineObject lineObject = new LineObject(0);

                        if (true == dictionaryGerman.TryGetValue(pair.Key, out lineObject))
                        {
                            List<string> missingGermanNestingStrings = pair.Value.NestingStrings.Except(lineObject.NestingStrings, StringComparer.OrdinalIgnoreCase).ToList();
                            if (missingGermanNestingStrings.Count > 0)
                            {
                                missingNestingStringsGerman.Add(CreateLineObjectMissingNestingStrings(missingGermanNestingStrings, pair));
                            }

                            List<string> missingEnglishNestingStrings = lineObject.NestingStrings.Except(pair.Value.NestingStrings, StringComparer.OrdinalIgnoreCase).ToList();
                            if (missingEnglishNestingStrings.Count > 0)
                            {
                                missingNestingStringsEnglish.Add(CreateLineObjectMissingNestingStrings(missingEnglishNestingStrings, pair));
                            }
                        }
                    }
                }
            );

            string translationFileName = "";
            Console.WriteLine("##### Missing NestingStrings $$ german #####");
            missingNestingStringsGerman.ForEach
            (
                item =>
                {
                    if (false == translationFileName.Equals(item.TranslationFile.FileName))
                    {
                        translationFileName = item.TranslationFile.FileName;
                        Console.WriteLine(translationFileName);
                    }
                    Console.WriteLine(item.Key + " (" + item.LineNumber + "): " + string.Join(", ", item.NestingStrings));
                }
            );
            Console.WriteLine();

            Console.WriteLine("##### Missing NestingStrings $$ english #####");
            missingNestingStringsEnglish.ForEach
            (
                item =>
                {
                    if (false == translationFileName.Equals(item.TranslationFile.FileName))
                    {
                        translationFileName = item.TranslationFile.FileName;
                        Console.WriteLine(translationFileName);
                    }
                    Console.WriteLine(item.Key + " (" + item.LineNumber + "): " + string.Join(", ", item.NestingStrings));
                }
            );
        }

        private LineObject CreateLineObjectMissingNestingStrings(List<string> missingEnglishNestingStrings, KeyValuePair<string, LineObject> pair)
        {
            LineObject missingLineObject = new LineObject(pair.Value.LineNumber);
            missingLineObject.NameSpaces = pair.Value.NameSpaces;
            missingLineObject.TranslationFile = pair.Value.TranslationFile;
            missingLineObject.Key = pair.Key;
            missingLineObject.NestingStrings = missingEnglishNestingStrings;
            return missingLineObject;
        }

        private LineObject CreateLineObjectMissingNamespaces(List<string> missingEnglishNamespaces, KeyValuePair<string, LineObject> pair)
        {
            LineObject missingLineObject = new LineObject(pair.Value.LineNumber);
            missingLineObject.NameSpaces = missingEnglishNamespaces;
            missingLineObject.TranslationFile = pair.Value.TranslationFile;
            missingLineObject.Key = pair.Key;
            missingLineObject.NestingStrings = pair.Value.NestingStrings;
            return missingLineObject;
        }

    }
}
