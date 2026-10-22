using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadEntityExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input DWG/DXF file path
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Output PNG file path
                string outputPath = "output.png";

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // NOTE: The Aspose.CAD API does not expose a CustomEntityList property on CadRasterizationOptions,
                    // so selective entity export cannot be performed directly via rasterization options.
                    // To achieve such filtering you would need to create a new CAD document containing only the desired entities,
                    // which is beyond the scope of this example.

                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        Layouts = new[] { "Model" }
                    };

                    // Set PNG output options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the CAD drawing to PNG
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Image saved to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}