using System;
using System.IO;
using System.Linq;
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
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Locate a specific entity to remove (example: first line entity)
                CadEntityBase entityToRemove = null;
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine)
                    {
                        entityToRemove = entity;
                        break;
                    }
                }

                // Remove the found entity from the drawing
                if (entityToRemove != null)
                {
                    var updatedEntities = cadImage.Entities
                        .Where(e => e != entityToRemove)
                        .ToArray();

                    // Replace the Entities collection with the updated one
                    cadImage.Entities = updatedEntities;
                }

                // Configure rasterization options (text quality setting not available in API)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example of setting other available options
                    UnitType = UnitType.Millimeter,
                    LineScale = 1.0f
                };

                // Set up PNG save options using the rasterization options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the modified drawing
                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}