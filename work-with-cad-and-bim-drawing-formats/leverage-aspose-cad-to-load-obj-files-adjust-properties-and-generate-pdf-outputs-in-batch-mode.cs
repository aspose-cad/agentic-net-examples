using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadBatchPdfExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output directories (can be passed via command line or hard‑coded)
                string inputFolder = args.Length > 0 ? args[0] : @"C:\Cad\Input";
                string outputFolder = args.Length > 1 ? args[1] : @"C:\Cad\Output";

                // Ensure output directory exists
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                // Get all OBJ files in the input folder
                string[] objFiles = Directory.GetFiles(inputFolder, "*.obj", SearchOption.TopDirectoryOnly);

                foreach (string objFilePath in objFiles)
                {
                    if (!File.Exists(objFilePath))
                        continue;

                    // Load the OBJ file using Aspose.CAD.Image.Load
                    Image cadImage = Image.Load(objFilePath);

                    // Prepare rasterization options for PDF export
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600,
                        // Layouts expects an array of layout names (e.g., "Model")
                        Layouts = new[] { "Model" }
                    };

                    // Create PDF export options and assign rasterization options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Build output PDF file path
                    string outputFileName = Path.GetFileNameWithoutExtension(objFilePath) + ".pdf";
                    string outputFilePath = Path.Combine(outputFolder, outputFileName);

                    // Save the CAD image as PDF
                    cadImage.Save(outputFilePath, pdfOptions);
                }

                Console.WriteLine("Batch conversion completed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}