using System;
using System.IO;
using System.Text;
using System.Xml;

namespace ParadoxTranslationHelper
{
    public class XMLFileUtility
    {
        static string FileName;

        public static XmlDocument Load(string fileName)
        {
            if (false == File.Exists(fileName))
            {
                return null;
            }
            try
            {
                FileName = fileName;
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = false;
                doc.Load(fileName);
                return doc;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
