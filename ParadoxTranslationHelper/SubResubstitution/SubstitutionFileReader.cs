using ParadoxTranslationHelper.LineObjects;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.SubResubstitution
{
    internal class SubstitutionFileReader
    {
        string _basePath;
        string _substituteFile;

        public string BasePath { get => _basePath; set => _basePath = value; }
        public string SubstituteFile { get => _substituteFile; set => _substituteFile = value; }

        public Dictionary<string, LineObjectSubstitutionFile>? ReadFile(string substitute)
        {
            string fileName = Path.Combine(_basePath, _substituteFile, substitute);
            if( false == File.Exists(fileName) )
            {
                Log.Warning($"File not found: {fileName}");
                return null;
            }

            Dictionary<string, LineObjectSubstitutionFile> lines = new Dictionary<string, LineObjectSubstitutionFile>();
            using (TextReader reader = File.OpenText(fileName))
            {
                string line;
                while((line = reader.ReadLine()) != null )
                {
                    LineObjectSubstitutionFile? lineObject = SubstituteFileHelper.Create(line);
                    if( null == lineObject)
                    {
                        Log.Warning($"Failed splitting line: {line} ");
                        continue;
                    }
                    lines.Add(lineObject.Substitute, lineObject);
                }
            }

            return lines;
        }
    }
}
