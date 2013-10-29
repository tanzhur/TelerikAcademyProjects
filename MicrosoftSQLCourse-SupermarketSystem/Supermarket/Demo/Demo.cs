using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenZipFile;
using PDFGeneratorConsole;
using WriteXMLFile;
namespace Demo
{
    class Demo
    {
        static void Main(string[] args)
        {
            string zipFileName = @"../../File.zip";
            string outputDirectory = "../../Extracted";
            string XMLFile = @"../../createdXML.xml";

            OpenZipFile.OpenZipFileClient.OpenZipFile(zipFileName, outputDirectory);
            OpenZipFile.OpenZipFileClient.GetExcelData();
            OpenZipFile.OpenZipFileClient.DeleteExtractedFiles(outputDirectory);

            PDFGeneratorConsole.PDFGeneratorClient.CreatePDFDocument();

            WriteXMLFile.WriteXMLFile.CreateXMLFile(XMLFile);
        }
    }
}
