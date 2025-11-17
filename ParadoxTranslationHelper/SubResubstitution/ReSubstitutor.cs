using ParadoxTranslationHelper.SubResubstitution;
using ParadoxTranslationHelper.Utilities;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParadoxTranslationHelper
{
    internal class ReSubstitutor
    {
        private Dictionary<string, string> _keyReSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _nestingStringsReSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _namespaceReSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _iconReSubstitute = new Dictionary<string, string>();
        IResubstitutorFuction resubstitutonFuction = null;

        FileReaderSubstitutionItem _fileReaderSubstitutionItem = new FileReaderSubstitutionItem();

        private TranslationFileSetSubstitution _translationFileSetSubstitution;

        public TranslationFileSetSubstitution TranslationFileSetSubstitution { get => _translationFileSetSubstitution; set => _translationFileSetSubstitution = value; }

        public void ReSubstitute()
        {
            if (_translationFileSetSubstitution == null)
            {
                Log.Information($"Parameter <TranslationFileSetSubstitution> must not be null!");
                return;
            }

            Log.Information("Resubstitution started ...");

            ReadSubstitutionFiles();

            string resubstitute = ResubstituteAll();
            string fileName = Utility.ReplaceWithAnalyseDirectory(_translationFileSetSubstitution.SubstitutedFile) + FileSubstitutionConstants.FILE_SUFFIX_RESUBSTITUTED;
            Log.Debug($"Writing text file: {fileName}");
            File.WriteAllText(Utility.ReplaceWithAnalyseDirectory(_translationFileSetSubstitution.SubstitutedFile) + FileSubstitutionConstants.FILE_SUFFIX_RESUBSTITUTED, resubstitute);
            Log.Information("Resubstitution finished ...");

            //            ReSubstitute(_translationFileSetSubstitution.SubstitutedFile.Lines.Values.ToList());
        }


        private string ResubstituteAll()
        {
            Log.Information($"Validating file: {_translationFileSetSubstitution.SubstitutedFile.FileName}");
            string allText = File.ReadAllText(_translationFileSetSubstitution.SubstitutedFile.FileName);
            Log.Debug($"Text size: {allText.Length}");

            Log.Information($"Resubstituting keys (count={_keyReSubstitute.Count})");
            string resubText = ResubstitutePart(allText, _keyReSubstitute);

            Log.Information($"Resubstituting nesting strings (count={_nestingStringsReSubstitute.Count})");
            resubText = ResubstitutePart(resubText, _nestingStringsReSubstitute);

            Log.Information($"Resubstituting namespaces (count={_namespaceReSubstitute.Count})");
            resubText = ResubstitutePart(resubText, _namespaceReSubstitute);

            Log.Information($"Resubstituting icons (count={_iconReSubstitute.Count})");
            resubText = ResubstitutePart(resubText, _iconReSubstitute);

            return resubText;
        }

        private string ResubstitutePart( string text, Dictionary<string,string> keyValuePairs )
        {
            string allTextTemp = text;
            int lastIndex = 0;
            int count = 0;
            foreach( KeyValuePair<string,string> keyValue in keyValuePairs)
            {
                int index = allTextTemp.IndexOf(keyValue.Key, lastIndex);
                if( index == -1 )
                {
                    Log.Warning($"Item not found: {keyValue.Key}");
                    continue;
                }

                allTextTemp = allTextTemp.ReplaceFirst(keyValue.Key, keyValue.Value, lastIndex);
                lastIndex = index;
                count++;
                if( count % 1000 == 0 )
                {
                    Log.Debug($"Processed items: {count}");
                }
            }

            Log.Debug($"Resubstituted {keyValuePairs.Count} keys");
            return allTextTemp;
        }

        private bool ReadSubstitutionFiles()
        {
            _fileReaderSubstitutionItem.FileName = _translationFileSetSubstitution.PathKeyFile;
            _keyReSubstitute = _fileReaderSubstitutionItem.Read();

            _fileReaderSubstitutionItem.FileName = _translationFileSetSubstitution.PathNestingStringsFile;
            _nestingStringsReSubstitute = _fileReaderSubstitutionItem.Read();

            _fileReaderSubstitutionItem.FileName = _translationFileSetSubstitution.PathNamespaceFile;
            _namespaceReSubstitute = _fileReaderSubstitutionItem.Read();

            _fileReaderSubstitutionItem.FileName = _translationFileSetSubstitution.PathIconFile; 
            _iconReSubstitute = _fileReaderSubstitutionItem.Read();

            return true;
        }

        private void ReSubstitute(List<LineObject> lineObjects )
        {
            if (lineObjects == null)
            {
                return;
            }

            if (lineObjects.Count == 0)
            {
                return;
            }

            ReSubstituteLines(lineObjects, _keyReSubstitute);
            ReSubstituteLines(lineObjects, _nestingStringsReSubstitute);
            ReSubstituteLines(lineObjects, _namespaceReSubstitute);
            ReSubstituteLines(lineObjects, _iconReSubstitute);
            SubstituteLinesColorCodeEnd(lineObjects);

            FileUtility.WriteLines(lineObjects, Utility.ReplaceWithAnalyseDirectory(_translationFileSetSubstitution.SubstitutedFile) + FileSubstitutionConstants.FILE_SUFFIX_RESUBSTITUTED);
            Log.Information("Resubstitution finished ...");
        }

        private void SubstituteLinesColorCodeEnd(List<LineObject> lineObjects)
        {
            foreach (LineObject lineObject in lineObjects)
            {
                if (false == lineObject.OriginalLine.Contains(FileSubstitutionConstants.COLOR_CODE_END))
                {
                    continue;
                }

                Log.Information($"Replacing item: {FileSubstitutionConstants.COLOR_CODE_END}");
                lineObject.OriginalLine = lineObject.OriginalLine.Replace(FileSubstitutionConstants.COLOR_CODE_END, FileSubstitutionConstants.COLOR_CODE_SIGN_END);
            }
        }

        private void ReSubstituteLines(List<LineObject> lineObjects, Dictionary<string, string> substituteTokens )
        {
            foreach ( KeyValuePair<string,string> item in substituteTokens) 
            { 
                string keyToFind = item.Key;
                foreach ( LineObject lineObject in lineObjects ) 
                { 
                    if( false == lineObject.OriginalLine.Contains( keyToFind ) )
                    {
                        continue;
                    }
                    Log.Debug($"Replacing item: {keyToFind} --> {item.Value}" );
                    lineObject.OriginalLine = lineObject.OriginalLine.Replace( keyToFind, item.Value );
                    Log.Debug($"Replaced OriginalLine: {lineObject.OriginalLine}");

                    string keyToFindShortend = keyToFind.Substring(0, keyToFind.Length - 1);
                    if (false == lineObject.OriginalLine.Contains(keyToFindShortend))
                    {
                        continue;
                    }

                    Log.Debug($"Replacing item deformed: {keyToFindShortend} --> {item.Value}");
                    lineObject.OriginalLine = lineObject.OriginalLine.Replace(keyToFindShortend, item.Value);
                    Log.Debug($"Replaced OriginalLine:  {lineObject.OriginalLine}");
                }
            }
        }

        //TODO: 2025-01-14 - JHA - Extract in separate class SubstitutionFileValidator
        private void ValidateAgaintsSubstitutionDataFiles()
        {
            Log.Information($"Validating file: {_translationFileSetSubstitution.SubstitutedFile.FileName}");
            string allText = File.ReadAllText(_translationFileSetSubstitution.SubstitutedFile.FileName);
            Log.Debug($"Text size: {allText.Length}");
            int fileOriginal = GetItemCount();

            _keyReSubstitute = Validate(allText, _keyReSubstitute);
            _nestingStringsReSubstitute = Validate(allText, _nestingStringsReSubstitute);
            _namespaceReSubstitute = Validate(allText, _namespaceReSubstitute);
            _iconReSubstitute = Validate(allText, _iconReSubstitute);

            Log.Information($"Overall items missing: {(fileOriginal - GetItemCount())}");
        }

        private int GetItemCount()
        {
            return _nestingStringsReSubstitute.Count + _namespaceReSubstitute.Count + _iconReSubstitute.Count;
        }

        private Dictionary<string, string> Validate( string text, Dictionary<string, string> substitutionSubSet )
        {
            Dictionary<string, string> validItems = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> keyValuePair in substitutionSubSet)
            {
                if (text.Contains(keyValuePair.Key))
                {
                    validItems.Add(keyValuePair.Key, keyValuePair.Value);
                    continue;
                }

                //TODO: 2025-11-15 - JHA - Jede Zeile wird auf alle Substitutionsitems geprüft. Umso mehr Zeilen umso mehr Substitutionsitems
                Log.Debug($"Substitution item not found: {keyValuePair.Key}; {keyValuePair.Value}");
            }
            Log.Information($"Items missing: {(substitutionSubSet.Count - validItems.Count).ToString()}");

            return validItems;
        }
    }
}
