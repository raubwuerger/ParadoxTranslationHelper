using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;
using Serilog.Core;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectAnalyse : FunctionObjectBase
    {
        string _pathGerman;
        string _pathSteam;

        public string PathGerman { get => _pathGerman; set => _pathGerman = value; }
        public string PathSteam { get => _pathSteam; set => _pathSteam = value; }

        DataSetLineObjectCompare _dataSetLineObjectCompareSteam = null;
        DataSetLineObjectCompare _dataSetLineObjectCompareGerman = null;

        public FunctionObjectAnalyse(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            if (true == string.IsNullOrEmpty(_pathGerman))
            {
                Log.Verbose("Member <PathGerman> must not be null or empty!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_pathSteam))
            {
                Log.Verbose("Member <PathSteam> must not be null or empty!");
                return false;
            }

            LocalisationFilesSteam = FileUtility.CreateTranslationFilesFromDirectory(_pathSteam);
            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_pathGerman);

            return CheckMissingKeys();
        }


        private bool CheckMissingKeys()
        {
            try
            {
                _dataSetLineObjectCompareSteam = CreateDataSetLineObjectMultipleKeys(LocalisationFilesSteam);
                _dataSetLineObjectCompareGerman = CreateDataSetLineObjectMultipleKeys(LocalisationFilesGerman);

                EvaluateKeysInWrongFileGlobal(_dataSetLineObjectCompareSteam.keysUnique, _dataSetLineObjectCompareGerman.keysUnique);
                EvaluateKeysToDeleteGlobal(_dataSetLineObjectCompareGerman.keysUnique, _dataSetLineObjectCompareSteam.keysUnique);
                EvaluateKeysMissingGlobal(_dataSetLineObjectCompareSteam.keysUnique, _dataSetLineObjectCompareGerman.keysUnique);

                LogMultipleKeys(_dataSetLineObjectCompareSteam.keysMultiple);
                LogMultipleKeys(_dataSetLineObjectCompareGerman.keysMultiple);

                return true;
            }
            catch(Exception ex) 
            {
                Log.Fatal("Exception occurred! " + ex.Message);
                return false;
            }
        }

        private DataSetLineObjectCompare CreateDataSetLineObjectMultipleKeys(List<TranslationFile> localisation)
        {
            DataSetLineObjectCompare dataSetLineObjectCompare = new DataSetLineObjectCompare();

            foreach (TranslationFile translationFile in localisation)
            {
                foreach (var item in translationFile.Lines)
                {
                    if (true == string.IsNullOrWhiteSpace(item.Value.Key))
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
            Log.Information(">>>>> Keys existing multiple times <<<<<");
            keysMultiple.ForEach(key => { Log.Information(key.TranslationFile + ": " + key.Key + ":" + key.LineNumber); });
        }

        private void EvaluateKeysMissingGlobal(Dictionary<string, LineObject> keysSteam, Dictionary<string, LineObject> keysGerman)
        {
            Log.Information(">>>>> EvaluateKeysMissingGlobal started <<<<<");
            List<LineObject> keysMissing = new List<LineObject>();
            List<LineObject> keysInWrongFile = new List<LineObject>();
            keysSteam.ToList().ForEach
            (
                x =>
                {
                    if (false == keysGerman.ContainsKey(x.Key))
                    {
                        keysMissing.Add(x.Value);
                    }
                    else
                    {
                        if( true == IsKeyInWrongFile(ref keysGerman, x) )
                        {
                            keysInWrongFile.Add(x.Value);
                        }
                    }
                }
            );

            keysMissing.ForEach(keys => Log.Information(LoggerConstants.MAP_KEYS_TO_CREATE + " in file {file}", keys.Key, keys.TranslationFile));
            Log.Information(">>>>> EvaluateKeysMissingGlobal stopped <<<<<");

        }

        private void EvaluateKeysInWrongFileGlobal(Dictionary<string, LineObject> keysSteam, Dictionary<string, LineObject> keysGerman)
        {
            Log.Information(">>>>> EvaluateKeysInWrongFileGlobal started <<<<<");
            List<LineObject> keysInWrongFile = new List<LineObject>();
            keysSteam.ToList().ForEach
            (
                x =>
                {
                    if (true == keysGerman.ContainsKey(x.Key))
                    {
                        if( true == IsKeyInWrongFile(ref keysGerman, x) )
                        {
                            Log.Information(LoggerConstants.MAP_KEYS_IN_WRONG_FILE + " [is][should] [{is}][{should}]", x.Value.Key, keysGerman[x.Key].TranslationFile.FileNameWithoutLocalisation, x.Value.TranslationFile.FileNameWithoutLocalisation);
                        }
                    }
                }
            );

            Log.Information(">>>>> EvaluateKeysInWrongFileGlobal stopped <<<<<");
        }

        private void EvaluateKeysToDeleteGlobal(Dictionary<string, LineObject> keysGerman, Dictionary<string, LineObject> keysSteam)
        {
            Log.Information(">>>>> EvaluateKeysToDeleteGlobal started <<<<<");
            List<LineObject> toDelete = new List<LineObject>();
            keysGerman.ToList().ForEach
            (
                x =>
                {
                    if (false == keysSteam.ContainsKey(x.Key))
                    {
                        toDelete.Add(x.Value);
                    }
                }
            );

            toDelete.ForEach(keys => Log.Information(LoggerConstants.MAP_KEYS_TO_DELETE + " in file {file}", keys.Key, keys.TranslationFile));
            Log.Information(">>>>> EvaluateKeysToDeleteGlobal stopped <<<<<");
        }

        private bool IsKeyInWrongFile(ref Dictionary<string, LineObject> keysGerman, KeyValuePair<string, LineObject> x)
        {
            TranslationFile translationFile = FunctionUtility.FindCorrespondingTranslationFile(LocalisationFilesGerman, x.Value.TranslationFile);
            if (translationFile == null)
            {
                return false;
            }

            List<LineObject> lineObjects = translationFile.Lines.Values.ToList();
            if (lineObjects.Count <= 0)
            {
                return false;
            }

            LineObject lineObject = lineObjects.Find(y => y.Key.Equals(x.Value.Key));
            if (lineObject != null)
            {
                return false;
            }

            LineObject keyInGerman = keysGerman.Values.ToList().Find(z => z.Key.Equals(x.Key));
            if( keyInGerman == null)
            {
                return false;
            }

            return true;
        }

        private void LogMissingNamespaces(Dictionary<string, LineObject> dictionaryEnglish, Dictionary<string, LineObject> dictionaryGerman)
        {
            Log.Information(">>>>> Missing namespaces [] <<<<<");
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
            Log.Information(">>>>> Missing Namespaces [] german <<<<<");
            missingNamespacesGerman.ForEach
            (
                item =>
                {
                    if (false == translationFileName.Equals(item.TranslationFile.FileName))
                    {
                        translationFileName = item.TranslationFile.FileName;
                        Log.Information(translationFileName);
                    }
                    Log.Information(item.Key + " (" + item.LineNumber + "): " + string.Join(", ", item.NameSpaces));
                }
            );

            Log.Information(">>>>> Missing Namespaces [] english <<<<<");
            missingNamespacesEnglish.ForEach
            (
                item =>
                {
                    if (false == translationFileName.Equals(item.TranslationFile.FileName))
                    {
                        translationFileName = item.TranslationFile.FileName;
                        Log.Information(translationFileName);
                    }
                    Log.Information(item.Key + " (" + item.LineNumber + "): " + string.Join(", ", item.NameSpaces));
                }
            );
        }

        private void LogMissingNestingStrings(Dictionary<string, LineObject> dictionaryEnglish, Dictionary<string, LineObject> dictionaryGerman)
        {
            Log.Information(">>>>> Missing NestingStrings $$ <<<<<");
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
            Log.Information(">>>>> Missing NestingStrings $$ german <<<<<");
            missingNestingStringsGerman.ForEach
            (
                item =>
                {
                    if (false == translationFileName.Equals(item.TranslationFile.FileName))
                    {
                        translationFileName = item.TranslationFile.FileName;
                        Log.Information(translationFileName);
                    }
                    Log.Information(item.Key + " (" + item.LineNumber + "): " + string.Join(", ", item.NestingStrings));
                }
            );

            Log.Information(">>>>> Missing NestingStrings $$ english <<<<<");
            missingNestingStringsEnglish.ForEach
            (
                item =>
                {
                    if (false == translationFileName.Equals(item.TranslationFile.FileName))
                    {
                        translationFileName = item.TranslationFile.FileName;
                        Log.Information(translationFileName);
                    }
                    Log.Information(item.Key + " (" + item.LineNumber + "): " + string.Join(", ", item.NestingStrings));
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
