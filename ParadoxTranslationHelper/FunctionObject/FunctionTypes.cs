using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public static class FunctionTypes
    {
        public static readonly string DiffFiles = "DIFF_FILES";
        public static readonly string Sub = "SUB";
        public static readonly string Resub = "RESUB";
        public static readonly string Insert = "INSERT";
        public static readonly string Remove = "REMOVE";
        public static readonly string DiffKeys = "DIFF_KEYS";
        public static readonly string Analyse = "ANALYSE";
        public static readonly string Validate = "VALIDATE";
        public static readonly string CheckForDoubleKeys = "CHECK_FOR_DOUBLE_KEYS";
        public static readonly string CheckForDoubleKeysAllFiles = "CHECK_FOR_DOUBLE_KEYS_ALL_FILES";
        public static readonly string CheckForDoubleKeysAllFilesFix = "CHECK_FOR_DOUBLE_KEYS_ALL_FILES_FIX";
    }
}
