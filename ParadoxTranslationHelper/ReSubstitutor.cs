using ParadoxTranslationHelper.Utilities;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParadoxTranslationHelper
{
    internal class ReSubstitutor
    {
        private Dictionary<string, string> _nestingStringsReSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _namespaceReSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _iconReSubstitute = new Dictionary<string, string>();

        FileReaderSubstitutionItem _fileReaderSubstitutionItem = new FileReaderSubstitutionItem();

        private TranslationFileSetSubstitution _translationFileSetSubstitution;

        public TranslationFileSetSubstitution TranslationFileSetSubstitution { get => _translationFileSetSubstitution; set => _translationFileSetSubstitution = value; }

        public void ReSubstitute()
        {
            if (_translationFileSetSubstitution == null)
            {
                return;
            }

            ReadSubstitutionFiles();

            ValidateAgaintsSubstitutionDataFiles();

            ReSubstitute(_translationFileSetSubstitution.SubstitutedFile.Lines.Values.ToList());
        }

        private bool ReadSubstitutionFiles()
        {
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

            SubstituteLines(lineObjects, _nestingStringsReSubstitute);
            SubstituteLines(lineObjects, _namespaceReSubstitute);
            SubstituteLines(lineObjects, _iconReSubstitute);
            SubstituteLinesColorCodeEnd(lineObjects);

            FileUtility.WriteLines(lineObjects, Utility.ReplaceWithAnalyseDirectory(_translationFileSetSubstitution.SubstitutedFile) + FileSubstitutionConstants.FILE_SUFFIX_RESUBSTITUTED);
            Log.Information("Finished ...");
        }

        private void SubstituteLinesColorCodeEnd(List<LineObject> lineObjects)
        {
            foreach (LineObject lineObject in lineObjects)
            {
                if (false == lineObject.OriginalLine.Contains(FileSubstitutionConstants.COLOR_CODE_END))
                {
                    continue;
                }

                Log.Information("Replacing item: " + FileSubstitutionConstants.COLOR_CODE_END);
                lineObject.OriginalLine = lineObject.OriginalLine.Replace(FileSubstitutionConstants.COLOR_CODE_END, FileSubstitutionConstants.COLOR_CODE_SIGN_END);
            }
        }

        private void SubstituteLines(List<LineObject> lineObjects, Dictionary<string, string> substituteTokens )
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
                    Log.Information("Replacing item: " + keyToFind +" --> " +item.Value );
                    lineObject.OriginalLine = lineObject.OriginalLine.Replace( keyToFind, item.Value );
                    Log.Information("Replaced OriginalLine: " + lineObject.OriginalLine);

                    string keyToFindShortend = keyToFind.Substring(0, keyToFind.Length - 1);
                    if (false == lineObject.OriginalLine.Contains(keyToFindShortend))
                    {
                        continue;
                    }

                    Log.Information("Replacing item deformed: " + keyToFindShortend + " --> " + item.Value);
                    lineObject.OriginalLine = lineObject.OriginalLine.Replace(keyToFindShortend, item.Value);
                    Log.Information("Replaced OriginalLine: " + lineObject.OriginalLine);
                }
            }
        }

        //TODO: 2025-01-14 - JHA - Extract in separate class SubstitutionFileValidator
        private void ValidateAgaintsSubstitutionDataFiles()
        {
            Log.Information("Validating file: " + _translationFileSetSubstitution.SubstitutedFile.FileName);
            string allText = File.ReadAllText(_translationFileSetSubstitution.SubstitutedFile.FileName);
            int fileOriginal = GetItemCount();

            _nestingStringsReSubstitute = Validate(allText, _nestingStringsReSubstitute);
            _namespaceReSubstitute = Validate(allText, _namespaceReSubstitute);
            _iconReSubstitute = Validate(allText, _iconReSubstitute);

            Log.Information("Overall items missing: " + (fileOriginal - GetItemCount()));
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

                Log.Information("Substitution item not found: " + keyValuePair.Key + ";" + keyValuePair.Value);
            }
            Log.Information("Items missing: " + (substitutionSubSet.Count - validItems.Count).ToString());

            return validItems;
        }
    }
}
