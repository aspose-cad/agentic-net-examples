using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

namespace CadFbxToPdfExport
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input FBX file path
            string inputPath = "input.fbx";
            
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }
// Output PDF file path
            string outputPath = "output.pdf";

            try
            {
                // Load the FBX drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific features
                    CadImage cadImage = (CadImage)image;

                    // Prepare PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the drawing as PDF while preserving original CAD properties
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("Export completed successfully.");
            }
            catch (Exception ex)
            {
                // Log any errors that occur during loading or saving
                Console.WriteLine($"Error during export: {ex.Message}");
            }
        }
    }
}