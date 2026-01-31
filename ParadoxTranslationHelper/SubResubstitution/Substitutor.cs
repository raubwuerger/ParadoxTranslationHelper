using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using ParadoxTranslationHelper.LineObjects;
using ParadoxTranslationHelper.Helper;

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

            if(translationFile.Lines == null)
            {
                return false;
            }

            if (translationFile.Lines.Count == 0)
            {
                return false;
            }

            Log.Information("Substituting file: " + translationFile.FileNameWithBasePath);
            Substitute(translationFile.Lines.Values.ToList());
            Log.Information($"Substituted keys: {_keySubstitute.Count}");
            Log.Information($"Substituted nesting strings: {_nestingStringsSubstitute.Count}");
            Log.Information($"Substituted color codes: {_colorCodeSubstitute.Count}");
            Log.Information($"Substituted name spaces: {_namespaceSubstitute.Count}");
            Log.Information($"Substituted icons: {_iconSubstitute.Count}");
            Log.Information($"Substituted new lines: {_newLineSubstitute.Count}");
            Log.Information($"Substituted tabulator: {_tabulatorSubstitute.Count}");

            return WriteSubstitionFiles(translationFile);
        }

        private bool WriteSubstitionFiles(TranslationFile translationFile)
        {
            fileWriterSubstitutionItem.FileName = translationFile.FileNameWithBasePath;
            fileWriterSubstitutionItem.FileSuffix = "";
            WriteSubstitionFile(translationFile, translationFile.FileNameWithBasePath + FileSubstitutionConstants.FILE_SUFFIX_SUBSTITUTED);

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

            fileWriterSubstitutionItem.FileSuffix = "." + FileSubstitutionConstants.NEW_LINE_SUFFIX;
            WriteSubstitionFile(_newLineSubstitute);

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
                if( true == LineHelper.IgnoreLine(lineObject.OriginalLine) )
                {
                    continue;
                }
                lineObject.OriginalLineSubstituted = lineObject.OriginalLine;

                SubstituteKey(lineObject);
                SubstituteNestingString(lineObject);
                SubstituteNamespace(lineObject);
                SubstituteIcon(lineObject);
                SubstituteColorCodes(lineObject);
                SubstituteNewLine(lineObject);
                SubstituteTabulator(lineObject);
            }
        }

        private void SubstituteKey(LineObject lineObject)
        {
            int index = lineObject.OriginalLineSubstituted.IndexOf(FileSubstitutionConstants.KEY_END_SIGN);
            if (index == -1)
            {
                return;
            }

            string key = lineObject.OriginalLineSubstituted.Substring(0, index);
            string subString = $"{FileSubstitutionConstants.SUBSTITUTION_START}{FileSubstitutionConstants.KEY_SUFFIX + Utility.PadLeft_6_0(_keySubstitute.Count)}{FileSubstitutionConstants.SUBSTITUTION_END}";
            _keySubstitute.Add(subString, SubstituteFileHelper.CreateSubKeyLineTripel(key, lineObject));
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
            string subString = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.NESTING_STRING_SUFFIX + Utility.PadLeft_6_0(_nestingStringsSubstitute.Count()) + FileSubstitutionConstants.SUBSTITUTION_END;
            _nestingStringsSubstitute.Add(subString, SubstituteFileHelper.CreateSubKeyLineTripel(sub, lineObject));
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
            string subString = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.NEW_LINE_SUFFIX + Utility.PadLeft_6_0(_newLineSubstitute.Count()) + FileSubstitutionConstants.SUBSTITUTION_END;
            _newLineSubstitute.Add(subString, SubstituteFileHelper.CreateSubKeyLineTripel(sub, lineObject));
            return subString;
        }

        private void SubstituteColorCodes(LineObject lineObject)
        {
            List<string> token = lineObject.ColorCodes;

            string substitute = lineObject.OriginalLineSubstituted;
            foreach (string subs in token)
            {
                substitute = StringExtensionMethods.ReplaceFirst(substitute, GenerateCompleteColorCodeToken(subs), GenerateColorCodeSubstitute(GenerateCompleteColorCodeToken(subs), lineObject));
            }

            lineObject.OriginalLineSubstituted = substitute;
        }

        private string GenerateCompleteColorCodeToken(string subs)
        {
            return subs;
        }
        private string GenerateColorCodeSubstitute(string sub, LineObject lineObject)
        {
            string subString = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.COLOR_CODE_SUFFIX + Utility.PadLeft_6_0(_colorCodeSubstitute.Count()) + FileSubstitutionConstants.SUBSTITUTION_END;
            _colorCodeSubstitute.Add(subString, SubstituteFileHelper.CreateSubKeyLineTripel(sub, lineObject));
            return subString;
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
            string subString = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.NAMESPACE_SUFFIX + Utility.PadLeft_6_0(_namespaceSubstitute.Count()) + FileSubstitutionConstants.SUBSTITUTION_END;
            _namespaceSubstitute.Add(subString, SubstituteFileHelper.CreateSubKeyLineTripel(sub, lineObject));
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
            //TODO: 2025-11-28 - JHA - Problem mit folgendem String " synthetic_refinery_resource:0 "£resources_strip|$FRAME$""
            
            return subs;
        }

        private string GenerateIconSubstitute(string sub, LineObject lineObject)
        {
            string subString = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.ICON_SUFFIX + Utility.PadLeft_6_0(_iconSubstitute.Count()) + FileSubstitutionConstants.SUBSTITUTION_END;
            _iconSubstitute.Add(subString, SubstituteFileHelper.CreateSubKeyLineTripel(sub, lineObject));
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
            string subString = FileSubstitutionConstants.SUBSTITUTION_START + FileSubstitutionConstants.TABULATOR_SUFFIX + FileSubstitutionConstants.SUBSTITUTION_END;
            _tabulatorSubstitute.Add(subString + Utility.PadLeft_6_0(_tabulatorSubstitute.Count()), SubstituteFileHelper.CreateSubKeyLineTripel(sub, lineObject));
            return subString;
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
