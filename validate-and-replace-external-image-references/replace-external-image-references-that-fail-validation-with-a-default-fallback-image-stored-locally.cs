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
        try
        {
            // Input CAD file (DWG/DXF/etc.)
            string inputCadPath = "input.dwg";
            // Fallback image to use when external reference is missing
            string fallbackImagePath = "fallback.png";

            // Verify input files exist
            if (!File.Exists(inputCadPath))
            {
                Console.Error.WriteLine($"Input CAD file not found: {inputCadPath}");
                return;
            }

            if (!File.Exists(fallbackImagePath))
            {
                Console.Error.WriteLine($"Fallback image file not found: {fallbackImagePath}");
                return;
            }

            // Prepare output directory
            string outputDir = "output";
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            string outputCadPath = Path.Combine(outputDir, "output.dwg");

            // Load CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputCadPath))
            {
                // Iterate over raster image entities
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadRasterImage rasterImage)
                    {
                        // Reference handle to the image definition
                        string defHandle = rasterImage.ImageDefReference;
                        if (string.IsNullOrEmpty(defHandle))
                            continue;

                        // Find the corresponding image definition object
                        var rasterDef = cadImage.Objects
                            .OfType<CadRasterImageDef>()
                            .FirstOrDefault(d => d.ObjectHandle == defHandle);

                        if (rasterDef == null)
                            continue;

                        // Check if the referenced external image file exists
                        string referencedPath = rasterDef.FileName;
                        if (string.IsNullOrEmpty(referencedPath) || !File.Exists(referencedPath))
                        {
                            // Replace with fallback image path
                            rasterDef.FileName = fallbackImagePath;
                        }
                    }
                }

                // Save the modified CAD file
                var saveOptions = new DwgOptions();
                cadImage.Save(outputCadPath, saveOptions);
            }

            Console.WriteLine($"Processing completed. Output saved to: {outputCadPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}