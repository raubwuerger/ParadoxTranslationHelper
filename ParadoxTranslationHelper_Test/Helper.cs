using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper_Test
{
    internal class Helper
    {
        public static string TEST_DATA = "testData";
        public static string FUNCTIONS = "functions";
        public static string VALIDATE = "VALIDATE";
        public static string LOCALISATION = "localisation";
        public static string STEAM = "steam";

        //Hier C:\\Projects\\ParadoxTranslationHelper\\ParadoxTranslationHelper_Test
        public static string GetTestLocation()
        {
            return Directory.GetParent(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName).FullName).FullName;
        }
    }
}
