using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    internal class TranslationFileAnalyser
    {
        static string FILE_PATTERN = "*.yml";

        static List<TranslationFile> _localisationEnglish = null;
        static List<TranslationFile> _localisationEnglishUpdated = null;
        static List<TranslationFile> _localisationGerman = null;

        public static void ReSubstitueSourceFiles()
        {
            Analyse();
            foreach (TranslationFile translationFile in _localisationGerman)
            {
                FileSubstitutor fileSubstitutor = new FileSubstitutor();
                fileSubstitutor.ReSubstitute(Create(translationFile, FindCorrespondingTranslationFile(translationFile)));
            }
        }

        private static string FindCorrespondingTranslationFile( TranslationFile translationFile )
        {
            TranslationFile corresponding = _localisationEnglish.Find( x => x.FileNameWithoutLocalisation.Equals( translationFile.FileNameWithoutLocalisation ) );
            if( corresponding == null ) 
            { 
                return null;
            }

            return corresponding.FileName;
        }

        public static void SubstitueSourceFiles()
        {
            Analyse();

            foreach (TranslationFile translationFile in _localisationEnglish)
            {
                FileSubstitutor fileSubstitutor = new FileSubstitutor();
                fileSubstitutor.Substitute(translationFile);
            }
        }

        public static void Compare()
        {
            Analyse();

            CheckTranslationFilesMissingUpdate();
            CheckTranslationFilesDeletedUpdate();

            CheckMissingTranslationFile();
            CheckMissingKeys();
        }

        public static void DiffTranslationVersions()
        {
            Analyse();
            CheckNewKeysUpdate();
        }

        private static void Analyse()
        {
            _localisationEnglish = AnalyseDirectory(ParadoxTranslationHelperConfig.PathEnglish);
            _localisationEnglishUpdated = AnalyseDirectory(ParadoxTranslationHelperConfig.PathEnglishUpdated);
            _localisationGerman = AnalyseDirectory(ParadoxTranslationHelperConfig.PathGerman);
        }

        private static TranslationFileSetSubstitution Create(TranslationFile substitutedFile, string pathToSubstiteFile)
        {
            TranslationFileSetSubstitution translationFileSetSubstitution = new TranslationFileSetSubstitution();

            translationFileSetSubstitution.SubstitutedFile = substitutedFile;
            translationFileSetSubstitution.PathNestingStringsFile = pathToSubstiteFile +"." +FileSubstitutionConstants.NESTING_STRING_SUFFIX;
            translationFileSetSubstitution.PathNamespaceFile = pathToSubstiteFile + "." + FileSubstitutionConstants.NAMESPACE_SUFFIX;
            translationFileSetSubstitution.PathIconFile = pathToSubstiteFile + "." + FileSubstitutionConstants.ICON_SUFFIX;

            return translationFileSetSubstitution;
        }

        private static List<TranslationFile> AnalyseDirectory( string directory )
        {
            string[] files = Directory.GetFiles(directory, FILE_PATTERN, SearchOption.AllDirectories);
            if (files.Length <= 0)
            {
                return null;
            }

            List<TranslationFile> translationFiles = new List<TranslationFile>();
            TranslationFileCreator translationFileCreator = new TranslationFileCreator();

            foreach (string file in files)
            {
                translationFiles.Add(translationFileCreator.Create(file));
            }

            return translationFiles;
        }

        private static void CheckMissingTranslationFile()
        {
            List<string> localisationFileNamesEnglish = _localisationEnglish.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> localisationFileNamesGerman = _localisationGerman.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> missingTranslationFiles = localisationFileNamesEnglish.Except(localisationFileNamesGerman).ToList<string>();

            if ( missingTranslationFiles.Count > 0 )
            {
                Console.WriteLine("Following transtlation files are missing:" + Environment.NewLine);
                foreach (string translationFile in missingTranslationFiles)
                {
                    Console.WriteLine(translationFile + Environment.NewLine);
                }
            }
        }

        private static void CheckTranslationFilesMissingUpdate()
        {
            if (false == _localisationEnglishUpdated.Any() || false == _localisationEnglish.Any())
            {
                Console.WriteLine("No translation files found!");
                return;
            }

            Console.WriteLine("Following transtlation files are no more existant in update: " + Path.GetFullPath(_localisationEnglishUpdated[0].FileName) +Environment.NewLine);
            List<string> localisationFileNamesEnglish = _localisationEnglish.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> localisationFileNamesEnglishUpdated = _localisationEnglishUpdated.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> missingTranslationFiles = localisationFileNamesEnglish.Except(localisationFileNamesEnglishUpdated).ToList<string>();

            if (missingTranslationFiles.Count > 0)
            {
                foreach (string translationFile in missingTranslationFiles)
                {
                    Console.WriteLine(translationFile + Environment.NewLine);
                }
            }
        }

        private static void CheckTranslationFilesDeletedUpdate()
        {
            if( false == _localisationEnglishUpdated.Any() || false == _localisationEnglish.Any() )
            {
                Console.WriteLine("No translation files found!");
                return;
            }

            Console.WriteLine("Following transtlation files are new in update: " + Path.GetFullPath(_localisationEnglishUpdated[0].FileName) +Environment.NewLine);
            List<string> localisationFileNamesEnglish = _localisationEnglish.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> localisationFileNamesEnglishUpdated = _localisationEnglishUpdated.ConvertAll(s => s.FileNameWithoutLocalisation);
            List<string> translationFilesToDelete = localisationFileNamesEnglishUpdated.Except(localisationFileNamesEnglish).ToList<string>();

            foreach (string translationFile in translationFilesToDelete)
            {
                Console.WriteLine(translationFile + Environment.NewLine);
            }
        }

        private static void CheckNewKeysUpdate()
        {
            Dictionary<string, LineObject> updated = GetKeys(_localisationEnglishUpdated);
            Dictionary<string, LineObject> old = GetKeys(_localisationEnglish);
            Dictionary<string, LineObject> toCreate = new Dictionary<string, LineObject>();

            foreach ( KeyValuePair<string, LineObject> pair in updated )
            {
                if( old.ContainsKey(pair.Key) )
                {
                    continue;
                }
                toCreate.Add(pair.Key, pair.Value);
            }

            string containedInFile = "";
            Console.WriteLine("Following keys (" + toCreate.Count + ") are new in update: " + Path.GetFullPath(_localisationEnglishUpdated[0].FileName) + Environment.NewLine);
            foreach( KeyValuePair<string, LineObject> pair in toCreate )
            {
                if( false == containedInFile.Equals(pair.Value.TranslationFile.FileName) )
                {
                    Console.WriteLine("##### " + pair.Value.TranslationFile);
                    containedInFile = pair.Value.TranslationFile.FileName;
                }
                Console.WriteLine(pair.Key +";" +pair.Value.OriginalLine);
            }

            //TODO: 2025-01-28 - JHA - Path analysation (<-- All created stuff in this directory)
            //TODO: 2025-01-28 - JHA - Refactor function part to dictionary Dictionary<string(sub,resub,...), IFunctionObject>
            string directory = CreateDirectory();
            if( null == directory )
            {
                Console.WriteLine("Unable to create directory! " + Path.Combine(ParadoxTranslationHelperConfig.PathBase, ParadoxTranslationHelperConfig.PathResult));
                return;
            }

            Utility.WriteLines(toCreate.Values.ToList(), Path.Combine(directory, "MissingTranslationKeys.yml"));

            Dictionary<string, LineObject> toDelete = new Dictionary<string, LineObject>();
            foreach (KeyValuePair<string, LineObject> pair in old)
            {
                if (updated.ContainsKey(pair.Key))
                {
                    continue;
                }
                toDelete.Add(pair.Key, pair.Value);
            }

            Console.WriteLine();
            Console.WriteLine();
            containedInFile = "";
            Console.WriteLine("Following keys (" + toDelete.Count + ") should be deleted: " + Path.GetFullPath(_localisationEnglish[0].FileName) + Environment.NewLine);
            foreach (KeyValuePair<string, LineObject> pair in toDelete)
            {
                if (false == containedInFile.Equals(pair.Value.TranslationFile.FileName))
                {
                    Console.WriteLine("##### " + pair.Value.TranslationFile);
                    containedInFile = pair.Value.TranslationFile.FileName;
                }
                Console.WriteLine(pair.Key + ";" + pair.Value.OriginalLine);
            }
        }

        private static string? CreateDirectory()
        {
            string pathDiff = Path.Combine(ParadoxTranslationHelperConfig.PathBase, ParadoxTranslationHelperConfig.PathResult);
            if (false == Directory.Exists(pathDiff))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(pathDiff);
                if( null == directoryInfo) 
                {
                    return null;
                }
            }
            return pathDiff;
        }

        private static Dictionary<string, LineObject> GetKeys( List<TranslationFile> files )
        {
            if (false == files.Any())
            {
                Console.WriteLine("No translation files found!");
                return null;
            }

            Dictionary<string, LineObject> keys = new Dictionary<string, LineObject>();
            foreach ( TranslationFile translationFile in files)
            {
                keys = keys.Union(Utility.GetValidKeys(translationFile.Lines.Values.ToList()).Where(k => !keys.ContainsKey(k.Key))).ToDictionary(k => k.Key, v => v.Value);
            }

            return keys;
        }
        private static DataSetLineObjectCompare CreateDataSetLineObjectCompare(List<TranslationFile> localisation)
        {
            DataSetLineObjectCompare dataSetLineObjectCompare = new DataSetLineObjectCompare();

            foreach (TranslationFile translationFile in localisation)
            {
                foreach (var item in translationFile.Lines)
                {
                    if( null == item.Value.Key )
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

        private static void CheckMissingKeys()
        {
            DataSetLineObjectCompare dataSetLineObjectCompareEnglish = CreateDataSetLineObjectCompare(_localisationEnglish);
            DataSetLineObjectCompare dataSetLineObjectCompareGerman = CreateDataSetLineObjectCompare(_localisationGerman);

            LogMissingKeys(dataSetLineObjectCompareEnglish.keysUnique, dataSetLineObjectCompareGerman.keysUnique);
            LogMissingNamespaces(dataSetLineObjectCompareEnglish.keysUnique, dataSetLineObjectCompareGerman.keysUnique);
            LogMissingNestingStrings(dataSetLineObjectCompareEnglish.keysUnique, dataSetLineObjectCompareGerman.keysUnique);

            LogMultipleKeys(dataSetLineObjectCompareEnglish.keysMultiple);
            LogMultipleKeys(dataSetLineObjectCompareGerman.keysMultiple);
        }

        private static void LogMultipleKeys(List<LineObject> keysMultiple)
        {
            Console.WriteLine("##### Keys existing multiple times" + Environment.NewLine);
            keysMultiple.ForEach(key => { Console.WriteLine(key.TranslationFile + ": " + key.Key + ":" + key.LineNumber); });
            Console.WriteLine(Environment.NewLine);
        }

        private static void LogMissingKeys(Dictionary<string, LineObject> keysBase, Dictionary<string, LineObject> keysShould)
        {
            Console.WriteLine("##### Missing keys #####" + Environment.NewLine);
            List<LineObject> missingKeys = new List<LineObject>();
            keysBase.ToList().ForEach
            (
                pair =>
                {
                    if( false == keysShould.ContainsKey(pair.Key) )
                    {
                        missingKeys.Add(pair.Value);
                    }
                }
            );

            missingKeys.ForEach(keys => Console.WriteLine(keys.Key));
            Console.WriteLine(Environment.NewLine);
        }

        private static LineObject CreateLineObjectMissingNamespaces(List<string> missingEnglishNamespaces, KeyValuePair<string, LineObject> pair )
        {
            LineObject missingLineObject = new LineObject(pair.Value.LineNumber);
            missingLineObject.NameSpaces = missingEnglishNamespaces;
            missingLineObject.TranslationFile = pair.Value.TranslationFile;
            missingLineObject.Key = pair.Key;
            missingLineObject.NestingStrings = pair.Value.NestingStrings;
            return missingLineObject;
        }

        private static LineObject CreateLineObjectMissingNestingStrings(List<string> missingEnglishNestingStrings, KeyValuePair<string, LineObject> pair)
        {
            LineObject missingLineObject = new LineObject(pair.Value.LineNumber);
            missingLineObject.NameSpaces = pair.Value.NameSpaces;
            missingLineObject.TranslationFile = pair.Value.TranslationFile;
            missingLineObject.Key = pair.Key;
            missingLineObject.NestingStrings = missingEnglishNestingStrings;
            return missingLineObject;
        }

        private static void LogMissingNamespaces(Dictionary<string, LineObject> dictionaryEnglish, Dictionary<string, LineObject> dictionaryGerman)
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
                    if( false == translationFileName.Equals(item.TranslationFile.FileName) )
                    {
                        translationFileName = item.TranslationFile.FileName;
                        Console.WriteLine(translationFileName);
                    }
                    Console.WriteLine(item.Key +" (" +item.LineNumber + "): " + string.Join(", ", item.NameSpaces));
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
                    Console.WriteLine(item.Key + " (" + item.LineNumber + "): "  + string.Join(", ", item.NameSpaces));
                }
            );
        }

        private static void LogMissingNestingStrings(Dictionary<string, LineObject> dictionaryEnglish, Dictionary<string, LineObject> dictionaryGerman)
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
    }
}
