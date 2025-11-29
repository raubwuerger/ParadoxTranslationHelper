using ParadoxTranslationHelper.FunctionObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Repositories
{
    public sealed class ColorRepository
    {
        private static ColorRepository _instance;
        private ColorRepository()
        {
        }

        public static ColorRepository Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new ColorRepository();
                }
                return _instance;
            }
        }

        Dictionary<string, string> colorsKeySuffix = new Dictionary<string, string>();
        Dictionary<string, string> colorsKeySign = new Dictionary<string, string>();

        public Dictionary<string, string> ColorsKeySuffix { get => colorsKeySuffix; }
        public Dictionary<string, string> ColorsKeySign { get => colorsKeySign; }

        public void Init()
        {
            InitInternalSuffix();
            InitInternalSign();
        }

        public void Clear()
        {
            colorsKeySuffix.Clear();
            colorsKeySign.Clear();
        }


        private void InitInternalSuffix()
        {
            if( true == colorsKeySuffix.Any() )
            {
                return;
            }

            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_END, "§!");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_A, "§A");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_B, "§B");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_C, "§C");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_D, "§D");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_E, "§E");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_F, "§F");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_G, "§G");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_H, "§H");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_I, "§I");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_J, "§J");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_K, "§K");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_L, "§L");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_M, "§M");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_N, "§N");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_O, "§O");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_P, "§P");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_Q, "§Q");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_R, "§R");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_S, "§S");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_T, "§T");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_U, "§U");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_V, "§V");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_W, "§W");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_X, "§X");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_Y, "§Y");
            colorsKeySuffix.Add(FileSubstitutionConstants.COLOR_CODE_SUFFIX_Z, "§Z");
        }

        private void InitInternalSign()
        {
            if (true == colorsKeySign.Any())
            {
                return;
            }

            colorsKeySign.Add("§!", FileSubstitutionConstants.COLOR_CODE_SUFFIX_END);
            colorsKeySign.Add("§A", FileSubstitutionConstants.COLOR_CODE_SUFFIX_A);
            colorsKeySign.Add("§B", FileSubstitutionConstants.COLOR_CODE_SUFFIX_B);
            colorsKeySign.Add("§C", FileSubstitutionConstants.COLOR_CODE_SUFFIX_C);
            colorsKeySign.Add("§D", FileSubstitutionConstants.COLOR_CODE_SUFFIX_D);
            colorsKeySign.Add("§E", FileSubstitutionConstants.COLOR_CODE_SUFFIX_E);
            colorsKeySign.Add("§F", FileSubstitutionConstants.COLOR_CODE_SUFFIX_F);
            colorsKeySign.Add("§G", FileSubstitutionConstants.COLOR_CODE_SUFFIX_G);
            colorsKeySign.Add("§H", FileSubstitutionConstants.COLOR_CODE_SUFFIX_H);
            colorsKeySign.Add("§I", FileSubstitutionConstants.COLOR_CODE_SUFFIX_I);
            colorsKeySign.Add("§J", FileSubstitutionConstants.COLOR_CODE_SUFFIX_J);
            colorsKeySign.Add("§K", FileSubstitutionConstants.COLOR_CODE_SUFFIX_K);
            colorsKeySign.Add("§L", FileSubstitutionConstants.COLOR_CODE_SUFFIX_L);
            colorsKeySign.Add("§M", FileSubstitutionConstants.COLOR_CODE_SUFFIX_M);
            colorsKeySign.Add("§N", FileSubstitutionConstants.COLOR_CODE_SUFFIX_N);
            colorsKeySign.Add("§O", FileSubstitutionConstants.COLOR_CODE_SUFFIX_O);
            colorsKeySign.Add("§P", FileSubstitutionConstants.COLOR_CODE_SUFFIX_P);
            colorsKeySign.Add("§Q", FileSubstitutionConstants.COLOR_CODE_SUFFIX_Q);
            colorsKeySign.Add("§R", FileSubstitutionConstants.COLOR_CODE_SUFFIX_R);
            colorsKeySign.Add("§S", FileSubstitutionConstants.COLOR_CODE_SUFFIX_S);
            colorsKeySign.Add("§T", FileSubstitutionConstants.COLOR_CODE_SUFFIX_T);
            colorsKeySign.Add("§U", FileSubstitutionConstants.COLOR_CODE_SUFFIX_U);
            colorsKeySign.Add("§V", FileSubstitutionConstants.COLOR_CODE_SUFFIX_V);
            colorsKeySign.Add("§W", FileSubstitutionConstants.COLOR_CODE_SUFFIX_W);
            colorsKeySign.Add("§X", FileSubstitutionConstants.COLOR_CODE_SUFFIX_X);
            colorsKeySign.Add("§Y", FileSubstitutionConstants.COLOR_CODE_SUFFIX_Y);
            colorsKeySign.Add("§Z", FileSubstitutionConstants.COLOR_CODE_SUFFIX_Z);
        }

        public string? GetValueSigns( string key )
        {
            if( colorsKeySuffix.Any() == false )
            {
                InitInternalSuffix();
            }

            string sign = null;
            colorsKeySuffix.TryGetValue(key, out sign);

            return sign;
        }
        public string? GetValueSuffix(string key)
        {
            if (colorsKeySign.Any() == false)
            {
                InitInternalSign();
            }

            string suffix = null;
            colorsKeySign.TryGetValue(key, out suffix);

            return suffix;
        }

    }
}
