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
        string inputPath = "input.dwg";
        string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("Loaded image is not a CadImage.");
                    return;
                }

                // Find the first raster image definition in the Objects collection
                CadRasterImageDef rasterDef = cadImage.Objects.OfType<CadRasterImageDef>().FirstOrDefault();

                if (rasterDef != null)
                {
                    // Reorder: place the raster definition at the beginning of the Objects array
                    var reordered = new List<CadBaseObject> { rasterDef };
                    reordered.AddRange(cadImage.Objects.Where(o => !ReferenceEquals(o, rasterDef)));
                    cadImage.Objects = reordered.ToArray();
                }

                DwgOptions saveOptions = new DwgOptions();
                cadImage.Save(outputPath, saveOptions);
            }

            Console.WriteLine("Background image order adjusted and file saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
