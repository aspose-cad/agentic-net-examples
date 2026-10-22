using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file (DWG, DXF, etc.)
            string sourcePath = "input.dwg";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the CAD file using the generic Image.Load method
            using (Image loadedImage = Image.Load(sourcePath))
            {
                // Cast the loaded Image to CadImage to access layout information
                CadImage cadImage = (CadImage)loadedImage;

                // Ensure output directory exists
                string outputDir = "output";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Iterate through all layout identifiers present in the CAD file
                foreach (string layoutName in cadImage.Layouts.Keys)
                {
                    Console.WriteLine("Found layout: " + layoutName);

                    // Example processing: export each layout to a separate PNG file
                    PngOptions pngOptions = new PngOptions();

                    // Configure rasterization options to render only the current layout
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new string[] { layoutName }
                    };
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Define output file name based on the layout identifier
                    string outputPath = Path.Combine(outputDir, $"output_{layoutName}.png");

                    // Save the current layout as a PNG image
                    cadImage.Save(outputPath, pngOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}