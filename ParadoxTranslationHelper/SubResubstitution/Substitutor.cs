using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using System.Diagnostics;

namespace ParadoxTranslationHelper
{
    internal class Substitutor
    {
        private Dictionary<string, string> _keySubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _nestingStringsSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _colorCodeSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _namespaceSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _iconSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _newLineSubstitute = new Dictionary<string, string>();
        private Dictionary<string, string> _tabulatorSubstitute = new Dictionary<string, string>();
        FileWriterSubstitutionItem fileWriterSubstitutionItem = new FileWriterSubstitutionItem();

        public bool Substitute(TranslationFile translationFile)
        {
            if (translationFile == null)
            {
                return false;
            }

            Log.Information("Substituting file: " + translationFile.FileName);
            Substitute(translationFile.Lines.Values.ToList());
            Log.Information($"Substituted keys: {_keySubstitute.Count}");
            Log.Information($"Substituted nesting strings: {_nestingStringsSubstitute.Count}");
            Log.Information($"Substituted color codes: {_colorCodeSubstitute.Count}");
            Log.Information($"Substituted name spaces: {_namespaceSubstitute.Count}");
            Log.Information($"Substituted icons: {_iconSubstitute.Count}");
            Log.Information($"Substituted new lines: {_newLineSubstitute.Count}");
            Log.Information($"Substituted tabulator: {_tabulatorSubstitute.Count}");

            WriteSubstitionFiles(translationFile);
            return true;
        }

        private bool WriteSubstitionFiles(TranslationFile translationFile)
        {
            string replacedPath = Utility.ReplaceWithAnalyseDirectory(translationFile);

            fileWriterSubstitutionItem.FileName = replacedPath;
            fileWriterSubstitutionItem.FileSuffix = "";
            WriteSubstitionFile(translationFile, replacedPath + FileSubstitutionConstants.FILE_SUFFIX_SUBSTITUTED);

            fileWriterSubstitutionItem.FileSuffix = "." + FileSubstitutionConstants.KEY_SUFFIX;
            WriteSubstitionFile(_keySubstitute);

            fileWriterSubstitutionItem.FileSuffix = "." + FileSubstitutionConstants.NESTING_STRING_SUFFIX;
            WriteSubstitionFile(_nestingStringsSubstitute);

            fileWriterSubstitutionItem.FileSuffix = "." + FileSubstitutionConstants.COLOR_CODE_SUFFIX;
            WriteSubstitionFile(_colorCodeSubstitute);

            fileWriterSubstitutionItem.FileSuffix = "." + FileSubstitutionConstants.NAMESPACE_SUFFIX;
            WriteSubstitionFile(_namespaceSubstitute);

            fileWriterSubstitutionItem.FileSuffix = "." + FileSubstitutionConstants.ICON_SUFFIX;
            WriteSubstitionFile(_iconSubstitute);

            Log.Information($"Overall items substituted: {(_keySubstitute.Count + _nestingStringsSubstitute.Count + _colorCodeSubstitute.Count + _namespaceSubstitute.Count + _iconSubstitute.Count + _newLineSubstitute.Count)}");

            //TODO: 2025-01-14 - JHA - Check if all files have been successfully written
            return true;

        }

        private void Substitute(List<LineObject> lineObjects)
        {
            if (lineObjects == null)
            {
                return;
            }

            if (lineObjects.Count == 0)
            {
                return;
            }

            foreach (var lineObject in lineObjects)
            {
                if( true == IgnoreLine(lineObject) )
                {
                    continue;
                }
                lineObject.OriginalLineSubstituted = lineObject.OriginalLine;

                SubstituteKey(lineObject);
                SubstituteNestingString(lineObject);
                SubstituteNamespace(lineObject);
                SubstituteIcon(lineObject);
                SubstituteColorCodeEnd(lineObject);
                SubstituteColorCodes(lineObject);
                SubstituteNewLine(lineObject);
                SubstituteTabulator(lineObject);
            }
        }

        private bool IgnoreLine(LineObject lineObject)
        {
            if( true == string.IsNullOrWhiteSpace(lineObject.OriginalLine) )
            {
                return true;
            }

            return lineObject.OriginalLine.Contains(Constants.TRANSLATION_FILE_IDENTIFIER);
        }
        private void SubstituteKey(LineObject lineObject)
        {
            int index = lineObject.OriginalLineSubstituted.IndexOf(FileSubstitutionConstants.KEY_END_SIGN);
            if (index == -1)
            {
                return;
            }

            string key = lineObject.OriginalLineSubstituted.Substring(0, index);
            string subString = $"{FileSubstitutionConstants.SUBSTITUTION_START}{FileSubstitutionConstants.KEY_SUFFIX + _keySubstitute.Count}{FileSubstitutionConstants.SUBSTITUTION_END}";
            _keySubstitute.Add(subString, CreateSubKeyLineTripel(key, lineObject));
            lineObject.KeySubstituted = subString;

            string substitute = lineObject.OriginalLineSubstituted;
            lineObject.OriginalLineSubstituted = StringExtensionMethods.ReplaceFirst(substitute, key, subString);
        }
        private void SubstituteNestingString(LineObject lineObject)
        {
            List<string> token = lineObject.NestingStrings;

            string substitute = lineObject.OriginalLineSubstituted;
            foreach (string subs in token)
            {
                substitute = StringExtensionMethods.ReplaceFirst(substitute, GenerateCompleteNestingStringToken(subs), GenerateNestingStringSubstitute(GenerateCompleteNestingStringToken(subs), lineObject));
            }

            lineObject.OriginalLineSubstituted = substitute;
        }
        private string GenerateCompleteNestingStringToken(string subs)
        {
            return FileSubstitutionConstants.NESTING_STRING_SIGN_START + subs + FileSubstitutionConstants.NESTING_STRING_SIGN_END;
        }

        private string GenerateNestingStringSubstitute(string sub, LineObject lineObject)
        {
            int count = _nestingStringsSubstitute.Count();
            string subString = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.NESTING_STRING_SUFFIX + count.ToString() + FileSubstitutionConstants.SUBSTITUTION_END;
            _nestingStringsSubstitute.Add(subString, CreateSubKeyLineTripel(sub, lineObject));
            count++;
            return subString;
        }

        private void SubstituteNewLine(LineObject lineObject)
        {
            List<string> token = lineObject.NewLines;         
            
            string substitute = lineObject.OriginalLineSubstituted;
            foreach (string sub in token)
            {
                substitute = StringExtensionMethods.ReplaceFirst(substitute, GenerateCompleteNewLineStringToken(sub), GenerateNewLineSubsitute(GenerateCompleteNewLineStringToken(sub), lineObject));
            }

            lineObject.OriginalLineSubstituted = substitute;
        }

        private string GenerateCompleteNewLineStringToken(string sub)
        {
            return sub;
        }

        private string GenerateNewLineSubsitute(string sub, LineObject lineObject)
        {
            int count = _newLineSubstitute.Count();
            string subString = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.NEW_LINE_SUFFIX +FileSubstitutionConstants.SUBSTITUTION_END;
            _newLineSubstitute.Add(subString +count.ToString(), CreateSubKeyLineTripel(sub, lineObject));
            count++;
            return subString;
        }

        private void SubstituteColorCodeEnd(LineObject lineObject)
        {
            lineObject.OriginalLineSubstituted = lineObject.OriginalLineSubstituted.Replace(FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.COLOR_CODE_SIGN_END + FileSubstitutionConstants.SUBSTITUTION_END, FileSubstitutionConstants.COLOR_CODE_SIGN_END);
        }

        //INFO: 2025-11-20 - JHA - Funktioniert eigentlich nur wenn vorher schon ColorCodeEnd substituiert wurde.
        private void SubstituteColorCodes(LineObject lineObject)
        {
            int index = lineObject.OriginalLineSubstituted.IndexOf(FileSubstitutionConstants.COLOR_CODE_SIGN_START);
            if (index == -1)
            {
                return;
            }

            if (index + 1 >= lineObject.OriginalLineSubstituted.Length)
            {
                return;
            }

            char indexSecond = lineObject.OriginalLineSubstituted[index + 1];
            if (indexSecond == '!')
            {
                return;
            }

            string colorCode = FileSubstitutionConstants.COLOR_CODE_SIGN_START + indexSecond.ToString();
            string colorCodeSubstitute = FileSubstitutionConstants.SUBSTITUTION_START + colorCode + FileSubstitutionConstants.SUBSTITUTION_END;

            lineObject.OriginalLineSubstituted = lineObject.OriginalLineSubstituted.Replace(colorCode, colorCodeSubstitute);
        }

        private void SubstituteNamespace(LineObject lineObject)
        {
            List<string> token = lineObject.NameSpaces;

            string substitute = lineObject.OriginalLineSubstituted;
            foreach (string subs in token)
            {
                substitute = StringExtensionMethods.ReplaceFirst(substitute, GenerateCompleteNamespaceToken(subs), GenerateNamespaceSubstitute(GenerateCompleteNamespaceToken(subs), lineObject));
            }

            lineObject.OriginalLineSubstituted = substitute;
        }

        private string GenerateCompleteNamespaceToken(string subs)
        {
            return FileSubstitutionConstants.NAMESPACE_START_SIGN_START + subs + FileSubstitutionConstants.NAMESPACE_START_SIGN_END;
        }

        private string GenerateNamespaceSubstitute(string sub, LineObject lineObject)
        {
            int count = _namespaceSubstitute.Count();
            string subString = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.NAMESPACE_SUFFIX + count.ToString() + FileSubstitutionConstants.SUBSTITUTION_END;
            _namespaceSubstitute.Add(subString, CreateSubKeyLineTripel(sub, lineObject));
            count++;
            return subString;
        }
        private void SubstituteIcon(LineObject lineObject)
        {
            List<string> token = lineObject.Icons;

            string substitute = lineObject.OriginalLineSubstituted;
            foreach (string subs in token)
            {
                substitute = StringExtensionMethods.ReplaceFirst(substitute, GenerateCompleteIconToken(subs), GenerateIconSubstitute(GenerateCompleteIconToken(subs), lineObject));
            }

            lineObject.OriginalLineSubstituted = substitute;
        }

        private string GenerateCompleteIconToken(string subs)
        {
            //TODO: 2025-11-16 - JHA - Im STNC Mod steht das Icon-Zeichen (£) sowohl am Anfang, wie auch am ende
            //            return FileSubstitutionConstants.ICON_START_SIGN_START + subs;
            return subs;
        }

        private string GenerateIconSubstitute(string sub, LineObject lineObject)
        {
            int count = _iconSubstitute.Count();
            string subString = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.ICON_SUFFIX + count.ToString() + FileSubstitutionConstants.SUBSTITUTION_END;
            _iconSubstitute.Add(subString, CreateSubKeyLineTripel(sub, lineObject));
            count++;
            return subString;
        }

        private void SubstituteTabulator(LineObject lineObject)
        {
            List<string> token = lineObject.Tabulators;

            string substitute = lineObject.OriginalLineSubstituted;
            foreach (string sub in token)
            {
                substitute = StringExtensionMethods.ReplaceFirst(substitute, GenerateCompleteTabulatorStringToken(sub), GenerateTabulatorSubsitute(GenerateCompleteTabulatorStringToken(sub), lineObject));
            }

            lineObject.OriginalLineSubstituted = substitute;
        }

        private string GenerateCompleteTabulatorStringToken(string sub)
        {
            return sub;
        }

        private string GenerateTabulatorSubsitute(string sub, LineObject lineObject)
        {
            int count = _tabulatorSubstitute.Count();
            string subString = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.TABULATOR_SUFFIX + FileSubstitutionConstants.SUBSTITUTION_END;
            _tabulatorSubstitute.Add(subString + count.ToString(), CreateSubKeyLineTripel(sub, lineObject));
            count++;
            return subString;
        }


        private string CreateSubKeyLineTripel(string sub, LineObject lineObject)
        {
            return sub + ";" + lineObject.Key + ";" + lineObject.LineNumber;
        }

        private void WriteSubstitionFile(TranslationFile translationFile, string fileName)
        {
            FileUtility.Write(translationFile, fileName);
        }
        private void WriteSubstitionFile(Dictionary<string, string> nestingStrings)
        {
            if (nestingStrings == null)
            {
                return;
            }
            fileWriterSubstitutionItem.Write(nestingStrings);
        }

    }
}
