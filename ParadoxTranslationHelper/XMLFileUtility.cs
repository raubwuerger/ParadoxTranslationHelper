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

        public static string FindNodeByNameAttribute(XmlNodeList nodes, string nodeName)
        {
            foreach (XmlNode node in nodes)
            {
                return node[nodeName].InnerText;
            }
            return null;
        }
        public static string FindChildNodeByName(XmlNodeList nodes, string nodeName)
        {
            foreach (XmlNode node in nodes)
            {
                return node[nodeName].InnerText;
            }
            return null;
        }

        public static string FindNodeByName(XmlNodeList nodes, string nodeName)
        {
            foreach (XmlNode node in nodes)
            {
                if (node.Name == nodeName)
                {
                    return node.InnerText;
                }
            }
            return null;
        }
        public static string GetAttributeValueByName(XmlAttributeCollection xmlAttributeCollection, string name)
        {
            if (xmlAttributeCollection == null)
            {
                return null;
            }

            foreach (XmlAttribute attribute in xmlAttributeCollection)
            {
                if (attribute.Name == name)
                {
                    return attribute.Value;
                }
            }

            return null;
        }

    }
}
