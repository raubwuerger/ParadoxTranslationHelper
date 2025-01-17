using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class FileWriter
    {
        public static bool Write(FileWithToken fileWithToken)
        {
            if( null == fileWithToken )
            {
                return false;
            }

            string pathToSave = fileWithToken.PathNameToSave;
            if( null == pathToSave )
            {
                return false;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(fileWithToken.PathNameToSave));

            if (File.Exists(pathToSave))
            {
                File.Delete(pathToSave);
            }

            try
            {
                using (FileStream fs = File.Create(pathToSave))
                {
                    foreach (LineTextTupel lineTextTupel in fileWithToken.GetLineTextTupels)
                    {
                        string lineTemp = lineTextTupel.LineNumber.ToString();
                        lineTemp += "\t";
                        lineTemp += lineTextTupel.Token;
                        lineTemp += Environment.NewLine;

                        byte[] line = new UTF8Encoding(true).GetBytes(lineTemp);

                        fs.Write(line, 0, line.Length);
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

    }
}
