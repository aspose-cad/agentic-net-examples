using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

namespace CadExportApp
{
    class Program
    {
        static void Main()
        {
            // Input CAD file path
            string inputPath = "input.dwg";
            // Desired output PDF file path
            string outputPath = "output.pdf";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            try
            {
                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Check DefaultFont property for potential font substitution issues
                    if (string.IsNullOrWhiteSpace(cadImage.DefaultFont))
                    {
                        Console.WriteLine("Warning: DefaultFont is not set on the CAD image. Font substitution may fail.");
                    }

                    // Configure rasterization options
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1600
                        // Additional options can be set here if needed
                    };

                    // Configure PDF export options
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Export the CAD image to PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"CAD file successfully exported to PDF: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during processing: {ex.Message}");
            }
        }
    }
}
