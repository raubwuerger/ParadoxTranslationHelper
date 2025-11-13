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
        public static string GetTestLocation()
        {
            return Directory.GetParent(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName).FullName).FullName;
        }
    }
}
