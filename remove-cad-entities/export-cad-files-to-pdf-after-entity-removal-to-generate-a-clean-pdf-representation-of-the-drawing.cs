using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadToPdfExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input DWG/DXF file path
                string inputFilePath = "input.dwg";

                // Output PDF file path
                string outputFilePath = "output.pdf";

                // Verify input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD file
                using (Image cadImageBase = Image.Load(inputFilePath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)cadImageBase;

                    // Remove all line entities from the drawing
                    cadImage.Entities = cadImage.Entities
                        .Where(entity => !(entity is CadLine))
                        .ToArray();

                    // Prepare PDF save options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the modified CAD drawing as PDF
                    cadImage.Save(outputFilePath, pdfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}