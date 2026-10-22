using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Input DWG file, background image, and output file paths
        string sourcePath = "input.dwg";
        string backgroundPath = "background.png";
        string outputPath = "output.dwg";

        // Verify that the source DWG and background image files exist
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source DWG file not found: {sourcePath}");
            return;
        }

        if (!File.Exists(backgroundPath))
        {
            Console.Error.WriteLine($"Background image file not found: {backgroundPath}");
            return;
        }

        try
        {
            // Load the DWG file as a CadImage instance
            using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
            {
                // Define default size for the background image (you may adjust as needed)
                int defaultWidth = 1000;
                int defaultHeight = 1000;

                // Create a raster image definition for the background image
                CadRasterImageDef rasterDef = new CadRasterImageDef(backgroundPath, defaultWidth, defaultHeight);

                // Preserve existing objects and add the new raster definition
                var objects = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                objects.Add(rasterDef);
                cadImage.Objects = objects.ToArray();

                // Create a raster image entity that references the definition
                CadRasterImage rasterImage = new CadRasterImage(
                    rasterDef,
                    new Cad3DPoint(0, 0, 0),               // Insertion point
                    new Cad3DPoint(defaultWidth, 0, 0),    // U vector
                    new Cad3DPoint(0, defaultHeight, 0)    // V vector
                );

                // Preserve existing entities and add the new raster image entity
                var entities = cadImage.Entities?.ToList() ?? new List<CadEntityBase>();
                entities.Add(rasterImage);
                cadImage.Entities = entities.ToArray();

                // Save the modified DWG file (no additional options required for DWG output)
                cadImage.Save(outputPath);
            }

            Console.WriteLine("Background image added and file saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}
