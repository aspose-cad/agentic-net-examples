using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerExtraction
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source DWF file
                string sourcePath = "input.dwf";
                // Path for the exported BMP file
                string outputPath = "output.bmp";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the DWF drawing
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Extract and list layer information
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        Console.WriteLine($"Layer Name: {layer.Name}");
                    }

                    // Prepare BMP export options
                    BmpOptions bmpOptions = new BmpOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200
                        }
                    };

                    // Save the drawing as BMP
                    image.Save(outputPath, bmpOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}