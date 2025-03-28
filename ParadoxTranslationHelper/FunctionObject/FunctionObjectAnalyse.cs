using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;

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

            CheckTranslationFilesMissingLocal();
            CheckTranslationFilesDeletedUpdate();

            CheckMissingKeys();
//            EvaluateMissingKeysFileByFile(_dataSetLineObjectCompareSteam.keysUnique, LocalisationFilesGerman);
//            CheckWrongLocatedKeys();

            return false;
        }

        private void CheckTranslationFilesMissingLocal()
        {
            if (false == LocalisationFilesSteam.Any() || false == LocalisationFilesGerman.Any())
            {
                Log.Verbose("No translation files found!");
                return;
            }

            List<string> localisationFileNamesGerman = LocalisationFilesGerman.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> localisationFileNamesSteam = LocalisationFilesSteam.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> missingTranslationFiles = localisationFileNamesGerman.Except(localisationFileNamesSteam).ToList<string>();

            if (missingTranslationFiles.Count > 0)
            {
                Log.Warning(">>>> Translation files missing local START <<<<<");
                foreach (string translationFile in missingTranslationFiles)
                {
                    Log.Warning(translationFile);
                }
                Log.Warning(">>>> Translation files missing local STOP  <<<<<");
            }
        }

        private void CheckTranslationFilesDeletedUpdate()
        {
            if (false == LocalisationFilesSteam.Any() || false == LocalisationFilesGerman.Any())
            {
                Log.Verbose("No translation files found!");
                return;
            }

            List<string> localisationFileNamesEnglish = LocalisationFilesGerman.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> localisationFileNamesEnglishUpdated = LocalisationFilesSteam.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> translationFilesToDelete = localisationFileNamesEnglishUpdated.Except(localisationFileNamesEnglish).ToList<string>();

            if(translationFilesToDelete.Count > 0) 
            {
                Log.Warning(">>>> Translation files to delete local START <<<<<");
                foreach (string translationFile in translationFilesToDelete)
                {
                    Log.Warning(translationFile);
                }
                Log.Warning(">>>> Translation files to delete local STOP <<<<<");
            }
        }

        private void CheckMissingKeys()
        {
            _dataSetLineObjectCompareSteam = CreateDataSetLineObjectMultipleKeys(LocalisationFilesSteam);
            _dataSetLineObjectCompareGerman = CreateDataSetLineObjectMultipleKeys(LocalisationFilesGerman);

            EvaluateMissingKeysGlobal(_dataSetLineObjectCompareSteam.keysUnique, _dataSetLineObjectCompareGerman.keysUnique);
            //            LogMissingNamespaces(_dataSetLineObjectCompareSteam.keysUnique, _dataSetLineObjectCompareGerman.keysUnique);
            //            LogMissingNestingStrings(_dataSetLineObjectCompareSteam.keysUnique, _dataSetLineObjectCompareGerman.keysUnique);

            LogMultipleKeys(_dataSetLineObjectCompareSteam.keysMultiple);
            LogMultipleKeys(_dataSetLineObjectCompareGerman.keysMultiple);
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
            Log.Warning("##### Keys existing multiple times");
            keysMultiple.ForEach(key => { Log.Information(key.TranslationFile + ": " + key.Key + ":" + key.LineNumber); });
        }

        private void EvaluateMissingKeysGlobal(Dictionary<string, LineObject> keysSteam, Dictionary<string, LineObject> keysGerman)
        {
            Log.Warning("##### Missing keys global started #####");
            List<LineObject> missingKeys = new List<LineObject>();
            keysSteam.ToList().ForEach
            (
                x =>
                {
                    if (false == keysGerman.ContainsKey(x.Key))
                    {
                        missingKeys.Add(x.Value);
                    }
                    else
                    {
                        TranslationFile translationFile = FunctionUtility.FindCorrespondingTranslationFile(LocalisationFilesGerman, x.Value.TranslationFile);
                        if (translationFile != null) 
                        {
                            List<LineObject> lineObjects = translationFile.Lines.Values.ToList();
                            if( lineObjects.Count > 0) 
                            {
                                LineObject lineObject = lineObjects.Find(y => y.Key.Equals(x.Value.Key));
                                if (lineObject == null)
                                {
                                    LineObject keyInGerman = keysGerman.Values.ToList().Find(z => z.Key.Equals(x.Key));
                                    Log.Warning("Key {key} not in correct file: {is} --> {should}", x.Value.Key, keyInGerman.TranslationFile.FileNameWithoutLocalisation, x.Value.TranslationFile.FileNameWithoutLocalisation);
                                    int notInCorrectFile = 0;
                                }
                                else
                                {
                                    int inCorrectFile = 0;
                                }
                            }
                        }
                    }
                }
            );

            missingKeys.ForEach(keys => Log.Information(keys.Key));
            Log.Warning("##### Missing keys global stopped #####");
        }

        private void LogMissingNamespaces(Dictionary<string, LineObject> dictionaryEnglish, Dictionary<string, LineObject> dictionaryGerman)
        {
            Log.Warning("##### Missing namespaces [] #####");
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
            Log.Information("##### Missing Namespaces [] german #####");
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

            Log.Information("##### Missing Namespaces [] english #####");
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
            Log.Information("##### Missing NestingStrings $$ #####" + Environment.NewLine);
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
            Log.Information("##### Missing NestingStrings $$ german #####");
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

            Log.Information("##### Missing NestingStrings $$ english #####");
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
