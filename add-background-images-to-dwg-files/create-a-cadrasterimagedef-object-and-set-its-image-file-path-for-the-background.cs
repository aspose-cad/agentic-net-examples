using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Input DWG file and background raster image
            string inputFilePath = "input.dwg";
            string backgroundImagePath = "background.png";

            // Output directory and file
            string outputDirectory = "output";
            string outputFilePath = Path.Combine(outputDirectory, "output_with_background.dwg");

            // Validate input files
            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Input DWG file not found: {inputFilePath}");
                return;
            }

            if (!File.Exists(backgroundImagePath))
            {
                Console.Error.WriteLine($"Background image file not found: {backgroundImagePath}");
                return;
            }

            // Ensure output directory exists
            Directory.CreateDirectory(outputDirectory);

            // Load the DWG file
            using (Image loadedImage = Image.Load(inputFilePath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = (CadImage)loadedImage;

                // Create a raster image definition for the background
                CadRasterImageDef rasterDef = new CadRasterImageDef(backgroundImagePath, 0, 0);

                // Add the raster definition to the drawing objects
                CadBaseObject[] existingObjects = cadImage.Objects ?? Array.Empty<CadBaseObject>();
                CadBaseObject[] updatedObjects = existingObjects.Concat(new[] { rasterDef }).ToArray();
                cadImage.Objects = updatedObjects; // setter is available in supported versions

                // Create a raster image entity that references the definition
                CadRasterImage rasterEntity = new CadRasterImage(
                    rasterDef,
                    new Cad3DPoint(0, 0, 0),   // Insertion point
                    new Cad3DPoint(1, 0, 0),   // U vector
                    new Cad3DPoint(0, 1, 0)    // V vector
                );

                // Add the raster entity to the drawing entities
                CadEntityBase[] existingEntities = cadImage.Entities?.ToArray() ?? Array.Empty<CadEntityBase>();
                CadEntityBase[] updatedEntities = existingEntities.Concat(new CadEntityBase[] { rasterEntity }).ToArray();
                cadImage.Entities = updatedEntities; // setter is available in supported versions

                // Save the modified DWG file
                cadImage.Save(outputFilePath);
            }

            Console.WriteLine($"DWG file saved to: {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
