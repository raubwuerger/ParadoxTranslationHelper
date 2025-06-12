using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParadoxTranslationHelper;
using ParadoxTranslationHelper.Comparator;
using ParadoxTranslationHelper.FunctionObject;
using ParadoxTranslationHelper.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper_Test
{
    [TestClass]
    public class AnalyzeNestingStrings_Test
    {
        [TestMethod]
        [DataRow(DisplayName = "AnalyzeNestingStrings: null,null --> null")]
        public void TestMethod001()
        {
            string pwd = System.IO.Directory.GetCurrentDirectory();

            List<TranslationFile> steam = FileUtility.CreateTranslationFilesFromDirectory(@"..\..\..\testData\diffNestingStrings\steam");
            List<TranslationFile> german = FileUtility.CreateTranslationFilesFromDirectory(@"..\..\..\testData\diffNestingStrings\german");

            ComparatorNestedString comparatorNestedString = new ComparatorNestedString();
            foreach(TranslationFile file in steam) 
            {
                TranslationFile correspondingTranslationFile = FunctionUtility.FindCorrespondingTranslationFile(german, file);
                if(correspondingTranslationFile == null)
                {
                    Log.Warning("### Corresponding translation file not found!");
                    continue;
                }

                List<LineObject> lineObjects = file.Lines.Values.ToList<LineObject>();
                foreach (LineObject lineObject in lineObjects )
                {
                    LineObject correspondingLineObject = FunctionUtility.FindCorrespondingLineObject(correspondingTranslationFile.Lines.Values.ToList<LineObject>(),lineObject);
                    if(  correspondingLineObject == null )
                    {
                        Log.Verbose("Corresponding LineObject not found! " +lineObject.ToString() );
                        continue;
                    }
                    comparatorNestedString.Compare(lineObject.NestingStrings, correspondingLineObject.NestingStrings);
                    if( true == comparatorNestedString.Ok() )
                    {
                        continue;
                    }

                    List<string> onlyInToVerify = comparatorNestedString.OnlyInToVerify;
                    List<string> onlyInOrg = comparatorNestedString.OnlyInOrg;

                    if( onlyInToVerify.Count == 1 && onlyInOrg.Count == 1 )
                    {
                        Log.Information("Change NestingString from " + onlyInToVerify[0] +" --> " + onlyInOrg[0] +": Key: " + lineObject.Key);
                    }
                }
            }


            AnalyzerMissingFiles.GenerateMissingGermanTranslationFiles(german, steam);
            Assert.IsNull( AnalyzerMissingFiles.GenerateMissingGermanTranslationFiles(null,null) );
        }
    }
}
