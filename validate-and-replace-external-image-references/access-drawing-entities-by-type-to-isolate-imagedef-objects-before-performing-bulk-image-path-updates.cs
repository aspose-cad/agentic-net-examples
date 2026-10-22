using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Paths
            string inputPath = "input.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputPath = "output.dxf";
            string newImageFolder = @"C:\NewImages";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load CAD drawing
            using (var cadImage = (Image)Image.Load(inputPath))
            {
                var cad = (CadImage)cadImage;

                // ------------------------------------------------------------
                // Isolate external raster definitions (IMAGEDEF objects)
                // ------------------------------------------------------------
                var rasterDefs = cad.Objects
                    .OfType<CadRasterImageDef>()
                    .ToList();

                // Update the file path for each raster definition
                foreach (var def in rasterDefs)
                {
                    // Preserve original file name, replace folder
                    string fileName = Path.GetFileName(def.FileName);
                    string newPath = Path.Combine(newImageFolder, fileName);
                    def.FileName = newPath;
                }

                // ------------------------------------------------------------
                // Ensure raster image entities reference the updated definitions
                // ------------------------------------------------------------
                var rasterImages = cad.Entities
                    .OfType<CadRasterImage>()
                    .ToList();

                foreach (var img in rasterImages)
                {
                    // Find the definition that matches the current reference
                    var matchingDef = rasterDefs.FirstOrDefault(d => d.ObjectHandle == img.ImageDefReference);
                    if (matchingDef != null)
                    {
                        // Re‑assign reference to guarantee consistency
                        img.ImageDefReference = matchingDef.ObjectHandle;
                    }
                }

                // Save the modified drawing preserving editable objects
                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };
                cad.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}