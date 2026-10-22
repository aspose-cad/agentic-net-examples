using System;
using System.Collections.Generic;
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
            // Load the DWG/DXF file
            using (Image loadedImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)loadedImage;

                // Filter only line entities from the top‑level drawing entities
                List<CadEntityBase> filteredLines = cadImage.Entities
                    .Where(entity => entity is CadLine)
                    .ToList();

                // Log the type of each filtered entity
                foreach (CadEntityBase entity in filteredLines)
                {
                    Console.WriteLine($"Filtered entity type: {entity.TypeName}");
                }

                // Replace the original entity collection with the filtered list
                // (Assuming the setter is available in the SDK version used)
                cadImage.Entities = filteredLines.ToArray();

                // Recalculate drawing extents after modifications
                cadImage.UpdateSize();

                // Prepare rasterization options for PNG output
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    DrawType = CadDrawTypeMode.UseObjectColor
                };

                // Set PNG save options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified drawing to PNG
                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
    }
}