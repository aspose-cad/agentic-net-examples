using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Ifc;

class Program
{
    static void Main()
    {
        try
        {
            // Paths to the source IFC file and the output PNG file
            string sourcePath = "input.ifc";
            string outputPath = "output.png";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the IFC drawing
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to IfcImage to access IFC‑specific properties
                IfcImage ifcImage = image as IfcImage;
                if (ifcImage != null)
                {
                    // NOTE: The Aspose.CAD SDK does not expose a GridSpacing property for IfcImage.
                    // Custom grid spacing cannot be set via the current API.
                }

                // Configure PNG export options
                PngOptions pngOptions = new PngOptions();

                // Rasterization parameters such as page size
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200
                };
                pngOptions.VectorRasterizationOptions = rasterOptions;

                // Export the drawing to PNG
                image.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}