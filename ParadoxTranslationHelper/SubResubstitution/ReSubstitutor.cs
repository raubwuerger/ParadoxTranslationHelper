using ParadoxTranslationHelper.Helper;
using ParadoxTranslationHelper.SubResubstitution;
using ParadoxTranslationHelper.Utilities;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ParadoxTranslationHelper
{
    internal class ReSubstitutor
    {
        private Dictionary<string, string> _keyReSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _nestingStringsReSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _nameSpaceReSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _iconReSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _colorReSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _newLineReSubstitute = new Dictionary<string, string>();

        IResubstitutorFuction resubstitutonFuction = null;

        FileReaderSubstitutionItem _fileReaderSubstitutionItem = new FileReaderSubstitutionItem();

        private TranslationFileSetSubstitution _translationFileSetSubstitution;

        public TranslationFileSetSubstitution TranslationFileSetSubstitution { get => _translationFileSetSubstitution; set => _translationFileSetSubstitution = value; }

        public void ReSubstitute()
        {
            if (_translationFileSetSubstitution == null)
            {
                Log.Warning($"Parameter <TranslationFileSetSubstitution> must not be null!");
                return;
            }

            Log.Information("Resubstitution started ...");

            if( false == ReadSubstitutionFiles() )
            {
                Log.Warning($"Function ReadSubstitutionFiles() failed!");
                return;
            }

            ReSubstitute(_translationFileSetSubstitution.SubstitutedFile.Lines.Values.ToList());

            FileUtility.WriteLines(_translationFileSetSubstitution.SubstitutedFile.Lines.Values.ToList(), _translationFileSetSubstitution.SubstitutedFile.FileNameWithBasePath + FileSubstitutionConstants.FILE_SUFFIX_RESUBSTITUTED);
            Log.Information("Resubstitution finished ...");
        }

        private string ResubstituteAll()
        {
            Log.Information($"Validating file: {_translationFileSetSubstitution.SubstitutedFile.FileNameWithBasePath}");
            string allText = File.ReadAllText(_translationFileSetSubstitution.SubstitutedFile.FileNameWithBasePath);
            Log.Debug($"Text size: {allText.Length}");

            Log.Information($"Resubstituting keys (count={_keyReSubstitute.Count})");
            string resubText = ResubstitutePart(allText, _keyReSubstitute);

            Log.Information($"Resubstituting nesting strings (count={_nestingStringsReSubstitute.Count})");
            resubText = ResubstitutePart(resubText, _nestingStringsReSubstitute);

            Log.Information($"Resubstituting namespaces (count={_nameSpaceReSubstitute.Count})");
            resubText = ResubstitutePart(resubText, _nameSpaceReSubstitute);

            Log.Information($"Resubstituting icons (count={_iconReSubstitute.Count})");
            resubText = ResubstitutePart(resubText, _iconReSubstitute);

            Log.Information($"Resubstituting color codes (count={_colorReSubstitute.Count})");
            resubText = ResubstitutePart(resubText, _colorReSubstitute);



            Log.Information($"Resubstituting new lines");
            resubText = resubText.Replace(FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.NEW_LINE_SUFFIX + FileSubstitutionConstants.SUBSTITUTION_END, FileSubstitutionConstants.NEW_LINE);
            resubText = resubText.Replace(FileSubstitutionConstants.SUBSTITUTION_START.Trim() + FileSubstitutionConstants.NEW_LINE_SUFFIX + FileSubstitutionConstants.SUBSTITUTION_END, FileSubstitutionConstants.NEW_LINE);
            resubText = resubText.Replace(FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.NEW_LINE_SUFFIX + FileSubstitutionConstants.SUBSTITUTION_END.Trim(), FileSubstitutionConstants.NEW_LINE);
            resubText = resubText.Replace(FileSubstitutionConstants.SUBSTITUTION_START.Trim() + FileSubstitutionConstants.NEW_LINE_SUFFIX + FileSubstitutionConstants.SUBSTITUTION_END.Trim(), FileSubstitutionConstants.NEW_LINE);

            Log.Information($"Resubstituting tabulators");
            resubText = resubText.Replace(FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.TABULATOR_SUFFIX + FileSubstitutionConstants.SUBSTITUTION_END, FileSubstitutionConstants.NEW_LINE);
            resubText = resubText.Replace(FileSubstitutionConstants.SUBSTITUTION_START.Trim() + FileSubstitutionConstants.TABULATOR_SUFFIX + FileSubstitutionConstants.SUBSTITUTION_END, FileSubstitutionConstants.NEW_LINE);
            resubText = resubText.Replace(FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.TABULATOR_SUFFIX + FileSubstitutionConstants.SUBSTITUTION_END.Trim(), FileSubstitutionConstants.NEW_LINE);
            resubText = resubText.Replace(FileSubstitutionConstants.SUBSTITUTION_START.Trim() + FileSubstitutionConstants.TABULATOR_SUFFIX + FileSubstitutionConstants.SUBSTITUTION_END.Trim(), FileSubstitutionConstants.NEW_LINE);

            return resubText;
        }

        private string ResubstitutePart( string text, Dictionary<string,string> keyValuePairs )
        {
            string allTextTemp = text;
            int lastIndex = 0;
            int count = 0;
            foreach( KeyValuePair<string,string> keyValue in keyValuePairs)
            {
                int index = allTextTemp.IndexOf(keyValue.Key.Trim(), lastIndex);
                if( index == -1 )
                {
                    Log.Warning($"Item not found: {keyValue.Key}");
                    continue;
                }

                allTextTemp = allTextTemp.ReplaceFirst(keyValue.Key, keyValue.Value, lastIndex);
                allTextTemp = allTextTemp.ReplaceFirst(keyValue.Key.Trim(), keyValue.Value, lastIndex);
                allTextTemp = allTextTemp.ReplaceFirst(keyValue.Key.TrimStart(), keyValue.Value, lastIndex);
                allTextTemp = allTextTemp.ReplaceFirst(keyValue.Key.TrimEnd(), keyValue.Value, lastIndex);
                lastIndex = index;
                count++;
                if( count % 100 == 0 )
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
            if( false == _keyReSubstitute.Any() )
            {
                Log.Warning($"File contains no data: {_fileReaderSubstitutionItem.FileName}");
                return false;
            }

            _fileReaderSubstitutionItem.FileName = _translationFileSetSubstitution.PathNestingStringsFile;
            _nestingStringsReSubstitute = _fileReaderSubstitutionItem.Read();
            if (false == _nestingStringsReSubstitute.Any())
            {
                Log.Warning($"File contains no data: {_fileReaderSubstitutionItem.FileName}");
            }

            _fileReaderSubstitutionItem.FileName = _translationFileSetSubstitution.PathNamespaceFile;
            _nameSpaceReSubstitute = _fileReaderSubstitutionItem.Read();
            if (false == _nameSpaceReSubstitute.Any())
            {
                Log.Warning($"File contains no data: {_fileReaderSubstitutionItem.FileName}");
            }

            _fileReaderSubstitutionItem.FileName = _translationFileSetSubstitution.PathIconFile; 
            _iconReSubstitute = _fileReaderSubstitutionItem.Read();
            if (false == _iconReSubstitute.Any())
            {
                Log.Warning($"File contains no data: {_fileReaderSubstitutionItem.FileName}");
            }

            _fileReaderSubstitutionItem.FileName = _translationFileSetSubstitution.PathColorFile;
            _colorReSubstitute = _fileReaderSubstitutionItem.Read();
            if (false == _colorReSubstitute.Any())
            {
                Log.Warning($"File contains no data: {_fileReaderSubstitutionItem.FileName}");
            }

            _fileReaderSubstitutionItem.FileName = _translationFileSetSubstitution.PathNewLineFile;
            _newLineReSubstitute = _fileReaderSubstitutionItem.Read();
            if (false == _newLineReSubstitute.Any())
            {
                Log.Warning($"File contains no data: {_fileReaderSubstitutionItem.FileName}");
            }

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

            ResubstitutionHelper resubstitutionHelper = new ResubstitutionHelper();

            lineObjects = AnalyseAndCorrectLines(lineObjects);
            FileUtility.WriteLines(lineObjects, _translationFileSetSubstitution.SubstitutedFile.FileNameWithBasePath + FileSubstitutionConstants.FILE_SUFFIX_RESUBSTITUTED +".fixed");

            //TODO: 2025-12-29 - JHA - In neue Klasse CorrectAnalyse auslagern
            CorrectDoubleKeys(lineObjects);

            CopyOriginalLineToOrigialLineSubstituted(lineObjects);

            ReSubstituteLinesRemove(lineObjects, _keyReSubstitute);
            ReSubstituteLinesRemove(lineObjects, _nestingStringsReSubstitute);
            ReSubstituteLinesRemove(lineObjects, _nameSpaceReSubstitute);
            ReSubstituteLinesRemove(lineObjects, _iconReSubstitute);
            ReSubstituteLinesRemove(lineObjects, _colorReSubstitute);
            ReSubstituteLinesRemove(lineObjects, _newLineReSubstitute);
            ReSubstituteTabulators(lineObjects);

            WriteNotResubstitutedItems();

            //TODO: 2025-11-29 - JHA - Schreibe Dateien mit nicht gefundenen Token -> _SteamKeysToCreate.yml.notFound.IC

        }

        private void WriteNotResubstitutedItems()
        {
            if ( true == _keyReSubstitute.Any() )
            {
                FileUtility.WriteLines(_keyReSubstitute, _translationFileSetSubstitution.PathKeyFile + FileSubstitutionConstants.NOT_FOUND);
            }

            if (true == _nestingStringsReSubstitute.Any())
            {
                FileUtility.WriteLines(_nestingStringsReSubstitute, _translationFileSetSubstitution.PathNestingStringsFile + FileSubstitutionConstants.NOT_FOUND);
            }

            if (true == _nameSpaceReSubstitute.Any())
            {
                FileUtility.WriteLines(_nameSpaceReSubstitute, _translationFileSetSubstitution.PathNamespaceFile+ FileSubstitutionConstants.NOT_FOUND);
            }

            if (true == _iconReSubstitute.Any())
            {
                FileUtility.WriteLines(_iconReSubstitute, _translationFileSetSubstitution.PathIconFile + FileSubstitutionConstants.NOT_FOUND);
            }

            if (true == _colorReSubstitute.Any())
            {
                FileUtility.WriteLines(_colorReSubstitute, _translationFileSetSubstitution.PathColorFile + FileSubstitutionConstants.NOT_FOUND);
            }

            if (true == _newLineReSubstitute.Any())
            {
                FileUtility.WriteLines(_newLineReSubstitute, _translationFileSetSubstitution.PathNewLineFile + FileSubstitutionConstants.NOT_FOUND);
            }
        }

        private List<LineObject> AnalyseAndCorrectLines(List<LineObject> lineObjects)
        {
            List<LineObject> correctLineObjects = new List<LineObject>();
            foreach (LineObject lineObject in lineObjects)
            {
                if( true == lineObject.OriginalLine.Contains(">>>>>") )
                {
                    correctLineObjects.Add(lineObject);
                    continue;
                }

                if( true == string.IsNullOrWhiteSpace(lineObject.OriginalLine) )
                {
                    Log.Debug($"IsNullOrWhiteSpace: {LogStringCreator.Create(lineObject)}");
                    continue;
                }

                if( true == lineObject.OriginalLine.Length < 16 )
                {
                    Log.Debug($"Length is less then 16: {LogStringCreator.Create(lineObject)}");
                    continue;
                }

                if( false == lineObject.OriginalLine.Contains(FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.KEY_SUFFIX) )
                {
                    Log.Debug($"Contains no valid key: {LogStringCreator.Create(lineObject)}");
                    continue;
                }

                int count = Utility.CountStringOccurrences(lineObject.OriginalLine, FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.KEY_SUFFIX);
                if( count > 1 )
                {
                    Log.Debug($"Contains to many ({count}) valid keys: {LogStringCreator.Create(lineObject)}");
                    continue;
                }

                correctLineObjects.Add(lineObject);
            }

            return correctLineObjects;
        }
        private void CorrectDoubleKeys(List<LineObject> lineObjects)
        {
            Dictionary<string, LineObject> keys = new Dictionary<string, LineObject>();
            Dictionary<string, LineObject> doubleKeys = new Dictionary<string, LineObject>();
            foreach (LineObject lineObject in lineObjects)
            {
                if (true == string.IsNullOrWhiteSpace(lineObject.Key))
                {
                    continue;
                }

                if (false == keys.ContainsKey(lineObject.Key))
                {
                    keys.Add(lineObject.Key, lineObject);
                }
                else
                {
                    doubleKeys.Add(lineObject.Key, lineObject);
                }
            }
        }

        private void CopyOriginalLineToOrigialLineSubstituted(List<LineObject> lineObjects)
        {
            foreach (LineObject lineObject in lineObjects)
            {
                lineObject.OriginalLineSubstituted = lineObject.OriginalLine;
            }
        }

        private void ReSubstituteLinesRemove(List<LineObject> lineObjects, Dictionary<string, string> substituteTokens )
        {
            foreach (LineObject lineObject in lineObjects)
            {
                if( lineObject.OriginalLine.Trim().StartsWith(">") )
                {
                    continue;
                }

                //TODO: 2025-11-29 - JHA - Vorher schon entfernen?
                if ( true == string.IsNullOrWhiteSpace(lineObject.OriginalLine) )
                {
                    continue;
                }

                foreach (KeyValuePair<string, string> item in substituteTokens.ToList() )
                {
                    if( false == lineObject.OriginalLineSubstituted.Contains(item.Key) )
                    {
                        Log.Debug($"Item to replaced not found: {item.Key}, {LogStringCreator.Create(lineObject)}");
                        continue;
                    }

                    lineObject.OriginalLineSubstituted = lineObject.OriginalLineSubstituted.Replace(item.Key, item.Value);
                    Log.Debug($"Item replaced: {item.Key} --> {item.Value}");
                    substituteTokens.Remove(item.Key);
                    if( false == lineObject.OriginalLineSubstituted.Contains(item.Key.Substring(0,6)) )
                    {
                        break;
                    }
                }
            }
        }

        private void ReSubstituteLines(List<LineObject> lineObjects, Dictionary<string, string> substituteTokens)
        {
            foreach (LineObject lineObject in lineObjects)
            {
                if (lineObject.OriginalLine.StartsWith(">"))
                {
                    continue;
                }

                //TODO: 2025-11-29 - JHA - Vorher schon entfernen?
                if (true == string.IsNullOrWhiteSpace(lineObject.OriginalLine))
                {
                    continue;
                }

                foreach (KeyValuePair<string, string> item in substituteTokens.ToList())
                {
                    if (false == lineObject.OriginalLineSubstituted.Contains(item.Key))
                    {
                        Log.Debug($"Item to replaced not found: {item.Key}, {LogStringCreator.Create(lineObject)}");
                        continue;
                    }
                    lineObject.OriginalLineSubstituted = lineObject.OriginalLineSubstituted.Replace(item.Key, item.Value);
                    Log.Debug($"Item replaced: {item.Key} --> {item.Value}");
                }
            }
        }


        private string? GetSubstitutedKey( LineObject lineObject, KeyValuePair<string, string> keyValuePair )
        {
            if (false == lineObject.OriginalLineSubstituted.Contains(keyValuePair.Key))
            {
                return null;
            }

            lineObject.OriginalLineSubstituted = lineObject.OriginalLineSubstituted.Replace(keyValuePair.Key, keyValuePair.Value);
            Log.Debug($"Replaced item: {keyValuePair.Key} --> {keyValuePair.Value}");
            return keyValuePair.Key;
        }
        private void ReSubstituteTabulators(List<LineObject> lineObjects)
        {
            foreach (LineObject lineObject in lineObjects)
            {
                if (false == lineObject.OriginalLineSubstituted.Contains(FileSubstitutionConstants.TABULATOR))
                {
                    continue;
                }

                Log.Information($"Replacing item: {FileSubstitutionConstants.TABULATOR}");
                lineObject.OriginalLineSubstituted = lineObject.OriginalLineSubstituted.Replace(FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.TABULATOR_SUFFIX + FileSubstitutionConstants.SUBSTITUTION_END , FileSubstitutionConstants.TABULATOR);
            }
        }

        private void ReSubstituteNewLines(List<LineObject> lineObjects)
        {
            string newLineToken = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.NEW_LINE_SUFFIX + FileSubstitutionConstants.SUBSTITUTION_END;
            foreach (LineObject lineObject in lineObjects)
            {
                if (false == lineObject.OriginalLineSubstituted.Contains(newLineToken))
                {
                    continue;
                }

                Log.Information($"Replacing item: {newLineToken}");
                lineObject.OriginalLineSubstituted = lineObject.OriginalLineSubstituted.Replace(newLineToken, FileSubstitutionConstants.NEW_LINE);
            }
        }

        //TODO: 2025-01-14 - JHA - Extract in separate class SubstitutionFileValidator
        private void ValidateAgaintsSubstitutionDataFiles()
        {
            Log.Information($"Validating file: {_translationFileSetSubstitution.SubstitutedFile.FileNameWithBasePath}");
            string allText = File.ReadAllText(_translationFileSetSubstitution.SubstitutedFile.FileNameWithBasePath);
            Log.Debug($"Text size: {allText.Length}");
            int fileOriginal = GetItemCount();

            _keyReSubstitute = Validate(allText, _keyReSubstitute);
            _nestingStringsReSubstitute = Validate(allText, _nestingStringsReSubstitute);
            _nameSpaceReSubstitute = Validate(allText, _nameSpaceReSubstitute);
            _iconReSubstitute = Validate(allText, _iconReSubstitute);

            Log.Information($"Overall items missing: {(fileOriginal - GetItemCount())}");
        }

        private int GetItemCount()
        {
            return _nestingStringsReSubstitute.Count + _nameSpaceReSubstitute.Count + _iconReSubstitute.Count;
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
