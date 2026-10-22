using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace AdjustCadDrawingProperties
{
    class Program
    {
        static int Main(string[] args)
        {
            // Example input and output paths (replace with actual paths)
            string inputPath = "input.dxf";
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine("Input file not found: " + inputPath);
                return 0;
            }

            bool success = ConvertCadToPdf(inputPath, outputPath);
            return success ? 0 : 1;
        }

        private static bool ConvertCadToPdf(string inputFile, string outputFile)
        {
            try
            {
                // Verify input file exists
                if (!File.Exists(inputFile))
                {
                    Console.WriteLine($"Input file not found: {inputFile}");
                    return false;
                }

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputFile);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD image
                using (Image cadImage = Image.Load(inputFile))
                {
                    // Cast to CadImage to access CAD‑specific members
                    CadImage cad = cadImage as CadImage;
                    if (cad == null)
                    {
                        Console.WriteLine("The loaded file is not a CAD image.");
                        return false;
                    }

                    // Example: iterate layers and write their names
                    foreach (CadLayerTable layer in cad.Layers)
                    {
                        Console.WriteLine($"Layer: {layer.Name}");
                        // Additional layer property adjustments could be placed here
                    }

                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200
                        // Layers = new string[] { "0", "Layer1" } // optional layer filter
                    };

                    // Pass the rasterization options to PDF options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the CAD drawing as PDF
                    cad.Save(outputFile, pdfOptions);
                }

                Console.WriteLine("Conversion completed successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during conversion: {ex.Message}");
                return false;
            }
        }
    }
}