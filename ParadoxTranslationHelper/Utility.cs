using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ParadoxTranslationHelper
{
    public class Utility
    {
        public static Dictionary<string, string> RemoveStringFromKey(Dictionary<string, string> list, string toRemove)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> s in list)
            {
                result.Add(s.Key.Replace(toRemove, ""), s.Value);
            }

            return result;
        }

        public static Dictionary<string, string> RemoveStringFromValue(Dictionary<string, string> list, string toRemove)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> s in list)
            {
                result.Add(s.Key, s.Value.Replace(toRemove, ""));
            }

            return result;
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
        public static string GetAttributeValueByName(XmlAttributeCollection xmlAttributeCollection, string name )
        {
            if ( xmlAttributeCollection == null )
            {
                return null;
            }

            foreach ( XmlAttribute attribute in xmlAttributeCollection )
            { 
                if ( attribute.Name == name ) 
                {  
                    return attribute.Value; 
                } 
            }

            return null;
        }

        public static string CreateStringFromTranslationFile(TranslationFile translationFile)
        {
            if (translationFile == null)
            {
                return null;
            }

            Dictionary<int, LineObject> _lines = translationFile.Lines;
            List<LineObject> lines = _lines.Values.ToList();

            string completeString = "";
            foreach ( LineObject lineObject in lines )
            {
                string substitutedLine = lineObject.OriginalLineSubstituted;
                if ( substitutedLine == null )
                {
                    substitutedLine = lineObject.OriginalLine;
                }
                completeString += substitutedLine;
                completeString += Environment.NewLine;
            }

            return completeString;
        }

        /**
         * Double keys will be bypassed.
         */
        public static Dictionary<string, string> ConvertToDictionary(List<string> lines)
        {
            Dictionary<string, string> resubstitutes = new Dictionary<string, string>();
            if (lines == null)
            {
                return resubstitutes;
            }

            foreach (string line in lines)
            {
                string[] splitted = line.Split(';');
                if (splitted.Length < 2)
                {
                    continue;
                }

                if (true == string.IsNullOrEmpty(splitted[0]))
                {
                    continue;
                }
                try
                {
                    resubstitutes.Add(splitted[0], splitted[1]);
                }
                catch(Exception ex)
                {
                    continue;
                }
            }

            return resubstitutes;
        }

        public static void WriteLines( List<LineObject> lineObjects, string fileName )
        {
            Console.WriteLine("Writing substituted source file started: " + fileName);
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                string missingKeyFile = "";
                foreach (LineObject line in lineObjects)
                {
                    if( false == missingKeyFile.Equals(line.TranslationFile.FileName) )
                    {
                        missingKeyFile = line.TranslationFile.FileName;
                        outputFile.WriteLine( "##### " +missingKeyFile );
                    }
                    outputFile.WriteLine(GetSubstitutedLineTabbed(line));
                }
            }
            Console.WriteLine("Writing substituted source file finished ...");
        }

        public static void WriteTranslationFile(TranslationFile translationFile)
        {
            if (translationFile == null)
            {
                return;
            }

            WriteTranslationFile(translationFile, translationFile.FileName);
        }

        public static void WriteTranslationFile(TranslationFile translationFile, string fileName )
        {
            if (translationFile == null)
            {
                return;
            }

            Dictionary<int, LineObject> _lines = translationFile.Lines;
            List<LineObject> lineObjects = _lines.Values.ToList();

            Console.WriteLine("Writing substituted source file started: " + fileName);
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                foreach (LineObject line in lineObjects)
                {
                    outputFile.WriteLine(GetSubstitutedLine(line));
                }
            }
            Console.WriteLine("Writing substituted source file finished ...");

        }
        private static string GetSubstitutedLineTabbed(LineObject lineObject)
        {
            if (lineObject.OriginalLineSubstituted == null)
            {
                lineObject.OriginalLineSubstituted = lineObject.OriginalLine;
            }

            return GetStringTabbed(lineObject);
        }

        private static string GetSubstitutedLine(LineObject lineObject)
        {
            if (lineObject.OriginalLineSubstituted == null)
            {
                lineObject.OriginalLineSubstituted = lineObject.OriginalLine;
            }

            return lineObject.OriginalLineSubstituted;
        }

        private static string GetStringTabbed(LineObject lineObject)
        {
            if( string.IsNullOrEmpty(lineObject.Key) )
            {
                return lineObject.OriginalLineSubstituted;
            }

            int indexOfKay = lineObject.OriginalLineSubstituted.IndexOf(lineObject.Key);
            int endOfKey = indexOfKay +lineObject.Key.Length;
            StringBuilder sb = new StringBuilder();
            sb.Append(lineObject.Key);
            sb.Append("\t");
            sb.Append(lineObject.OriginalLineSubstituted.Substring(endOfKey));
            lineObject.OriginalLineSubstituted = sb.ToString();

            return lineObject.OriginalLineSubstituted;
        }

        public static Dictionary<string,LineObject> GetValidKeys( List<LineObject> lines)
        {
            if( null == lines )
            {
                return null;
            }

            Dictionary<string,LineObject> keys = new Dictionary<string,LineObject>();  

            foreach (LineObject line in lines) 
            {
                if (  string.IsNullOrEmpty(line.Key) )
                {
                    continue;
                }

                keys[line.Key] = line;
            }

            return keys;
        }

    }
}
