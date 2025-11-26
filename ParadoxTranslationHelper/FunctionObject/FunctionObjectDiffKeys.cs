using ParadoxTranslationHelper.Utilities;
using System;
using System.Collections.Generic;
using Serilog;
using System.Linq;
using ParadoxTranslationHelper.Comparator;
using System.IO;

namespace ParadoxTranslationHelper
{
    public class FunctionObjectDiffKeys : FunctionObjectBase
    {
        private string _localisationFilePathSteam;
        private string _localisationFilePathGerman;
        private string _localisationFilePathAnalyze;

        public string LocalisationFilePathGerman { get => _localisationFilePathGerman; set => _localisationFilePathGerman = value; }
        public string LocalisationFilePathAnalyze { get => _localisationFilePathAnalyze; set => _localisationFilePathAnalyze = value; }
        public string LocalisationFilePathSteam { get => _localisationFilePathSteam; set => _localisationFilePathSteam = value; }

        public FunctionObjectDiffKeys(string name) : base(name)
        {
        }

        public override bool DoWork()
        {
            if (true == string.IsNullOrEmpty(_localisationFilePathSteam))
            {
                Log.Verbose("Member <LocalisationFilePathSteam> must not be null!");
                return false;
            }

            if (true == string.IsNullOrEmpty(_localisationFilePathGerman))
            {
                Log.Verbose("Member <LocalisationFilePathGerman> must not be null!");
                return false;
            }

            LocalisationFilesGerman = FileUtility.CreateTranslationFilesFromDirectory(_localisationFilePathGerman);
            if (LocalisationFilesGerman == null)
            {
                return false;
            }

            if (LocalisationFilesGerman.Count == 0)
            {
                Log.Verbose("Path contains no files:" + _localisationFilePathGerman);
                return false;
            }

            LocalisationFilesSteam = FileUtility.CreateTranslationFilesFromDirectory(_localisationFilePathSteam);
            if (LocalisationFilesSteam == null)
            {
                return false;
            }

            if (LocalisationFilesSteam.Count == 0)
            {
                Log.Verbose("Path contains no files:" + _localisationFilePathSteam);
                return false;
            }

            DiffNestingStrings(LocalisationFilesSteam, LocalisationFilesGerman);
//            DiffKeys();

            return true;
        }

        private void DiffKeys()
        {
            foreach (TranslationFile translationFile in LocalisationFilesSteam)
            {
                DiffKeys(translationFile, FunctionUtility.FindCorrespondingTranslationFile(LocalisationFilesGerman, translationFile));
            }
        }

        private void DiffKeys(TranslationFile org, TranslationFile toVerify)
        {
            if (org == null)
            {
                Log.Verbose("Parameter <org> must not be null!");
                return;
            }

            if (toVerify == null)
            {
                Log.Verbose("Parameter <toVerify> must not be null!");
                return;
            }

            foreach (LineObject line in org.Lines.Values.ToList())
            {
                if (false == line.HasKey())
                {
                    continue;
                }

                if (false == DiffColorCodes(line, FunctionUtility.FindCorrespondingLineObject(toVerify.Lines.Values.ToList(), line)))
                {
                    Log.Information("Key not found: " + line.Key);
                }
            }
        }

        private bool DiffColorCodes(LineObject org, LineObject toVerify)
        {
            if (org == null)
            {
                Log.Verbose("Parameter <org> must not be null!");
                return false;
            }

            if (toVerify == null)
            {
                Log.Verbose("Parameter <toVerify> must not be null!");
                return false;
            }

            List<string> orgCopy = org.ColorCodes.ConvertAll(x => String.Copy(x));
            List<string> toVerifyCopy = toVerify.ColorCodes.ConvertAll(x => String.Copy(x));

            foreach (string item in toVerify.ColorCodes)
            {
                if (false == orgCopy.Contains(item))
                {
                    continue;
                }
                orgCopy.Remove(item);
                toVerifyCopy.Remove(item);
            }

            if (orgCopy.Count > 0)
            {
                Log.Information(LoggerConstants.MAP_DIFF_COLOR_CODES + " ColorCodes not found in toVerify: " + org.Key + ": " + string.Join(",", orgCopy));
            }

            if (toVerifyCopy.Count > 0)
            {
                Log.Information(LoggerConstants.MAP_DIFF_COLOR_CODES + " ColorCodes wrong in toVerify: " + toVerify.Key + ": " + string.Join(",", toVerifyCopy));
            }

            return true;
        }

        private void DiffNestingStrings(List<TranslationFile> org, List<TranslationFile> toVerify)
        {
            ComparatorNestedString comparatorNestedString = new ComparatorNestedString();
            foreach (TranslationFile file in org)
            {
                TranslationFile correspondingTranslationFile = FunctionUtility.FindCorrespondingTranslationFile(toVerify, file);
                if (correspondingTranslationFile == null)
                {
                    Log.Warning("### Corresponding translation file not found!");
                    continue;
                }

                List<LineObject> lineObjects = file.Lines.Values.ToList<LineObject>();
                foreach (LineObject lineObject in lineObjects)
                {
                    LineObject correspondingLineObject = FunctionUtility.FindCorrespondingLineObject(correspondingTranslationFile.Lines.Values.ToList<LineObject>(), lineObject);
                    if (correspondingLineObject == null)
                    {
                        Log.Verbose("Corresponding LineObject not found! " + lineObject.ToString());
                        continue;
                    }
                    comparatorNestedString.Compare(lineObject.NestingStrings, correspondingLineObject.NestingStrings);
                    if (true == comparatorNestedString.Ok())
                    {
                        continue;
                    }

                    List<string> onlyInToVerify = comparatorNestedString.OnlyInToVerify;
                    List<string> onlyInOrg = comparatorNestedString.OnlyInOrg;

                    if (onlyInToVerify.Count == 1 && onlyInOrg.Count == 1)
                    {
                        Log.Information("Change NestingString from " + onlyInToVerify[0] + " --> " + onlyInOrg[0] + ": Key: " + lineObject.Key);
                        ChangeNestingString(onlyInToVerify[0], correspondingLineObject, onlyInOrg[0]);
                        continue;
                    }

                    if( onlyInOrg.Count > onlyInToVerify.Count )
                    {
                        Log.Information("Missing in translation: " + string.Join(", ", onlyInOrg) +": Key: " + lineObject.Key);
                        continue;
                    }

                    if (onlyInOrg.Count < onlyInToVerify.Count)
                    {
                        Log.Information("Unnecessary (to delete): " + string.Join(", ", onlyInOrg) + ": Key: " + lineObject.Key);
                        continue;
                    }
                }

                FileUtility.Write(correspondingTranslationFile);
            }
        }

        private void ChangeNestingString( string nestingStringWrong, LineObject lineObject, string nestingStringToChange )
        {
            if( null == lineObject )
            {
                Log.Verbose("Parameter <LineObject::lineObject> must not be null!");
                return;
            }

            if( false == lineObject.NestingStrings.Any() )
            {
                Log.Verbose("Parameter <LineObject::lineObject> has no nesting strings!");
                return;
            }

            if ( true == string.IsNullOrEmpty(nestingStringToChange) ) 
            {
                Log.Verbose("Parameter <string::nestingStringToChange> must not be null or empty!");
                return;
            }

            if (true == string.IsNullOrEmpty(nestingStringWrong))
            {
                Log.Verbose("Parameter <string::nestingStringWrong> must not be null or empty!");
                return;
            }

            //TODO: 2025-06-12 - JHA - In separate Funktion auslagern
            //TODO: 2025-06-12 - JHA - Fehler: Wenn mehrere nestringStrings in der falschen Reihenfolge vorhanden sind wird das nicht erkannt. Kann nicht immer korrigiert werden. Andere Satzstruktur im englischen!
            int indexOfWring = lineObject.NestingStrings.IndexOf(nestingStringWrong);
            if( indexOfWring == -1 )
            {
                Log.Verbose("Parameter <string::nestingStringWrong> not found in NestingString: " +string.Join(", ", lineObject.NestingStrings));
                return;
            }
            lineObject.NestingStrings[indexOfWring] = nestingStringToChange;
            lineObject.OriginalLine = lineObject.OriginalLine.Replace(StringParserFactory.NESTING_STRINGS_START +nestingStringWrong + StringParserFactory.NESTING_STRINGS_END, StringParserFactory.NESTING_STRINGS_START +nestingStringToChange + StringParserFactory.NESTING_STRINGS_END);
            Log.Information("Changed nesting string from " + nestingStringWrong + " to " + nestingStringToChange);
        }
    }
}
