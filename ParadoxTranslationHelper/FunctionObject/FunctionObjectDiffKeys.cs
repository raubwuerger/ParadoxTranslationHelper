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

            DiffKeys();

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
    }
}
