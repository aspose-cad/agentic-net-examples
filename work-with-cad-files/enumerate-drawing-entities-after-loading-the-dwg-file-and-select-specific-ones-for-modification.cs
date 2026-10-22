using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the DWG file using Aspose.CAD.Image.Load
            using (Image image = Image.Load(inputPath))
            {
                // Cast the loaded image to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Enumerate top‑level drawing entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Select line entities that belong to a specific layer (e.g., "0")
                    if (entity is CadLine line && line.LayerName == "0")
                    {
                        // Modify the first point of the line (shift X coordinate by 10 units)
                        line.FirstPoint = new Cad3DPoint(
                            line.FirstPoint.X + 10,
                            line.FirstPoint.Y,
                            line.FirstPoint.Z);
                    }
                }

                // Prepare rasterization options for saving the modified drawing as PNG
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f
                };

                // Create PNG save options and assign the rasterization options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the modified drawing to a PNG file
                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}