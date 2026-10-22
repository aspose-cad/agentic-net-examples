using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        // Input and output file paths (replace with actual paths)
        string inputPath = @"input.dxf";
        string outputPath = @"output.dxf";

        // Guard against missing input file
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Mapping of old image file names to new file locations
        var replacementMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "oldImage1.png", @"C:\NewImages\newImage1.png" },
            { "oldImage2.jpg", @"C:\NewImages\newImage2.jpg" }
            // add more replacements as needed
        };

        CadImage cadImage = null;
        try
        {
            // Load the CAD file
            cadImage = (CadImage)Image.Load(inputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load CAD file: {ex.Message}");
            return;
        }

        try
        {
            // Update CadRasterImageDef objects stored in cadImage.Objects
            foreach (var rasterDef in cadImage.Objects.OfType<CadRasterImageDef>())
            {
                if (replacementMap.TryGetValue(rasterDef.FileName, out string newPath))
                {
                    rasterDef.FileName = newPath;
                }
            }

            // Build a lookup of definition handles by file name for quick matching
            var defByFileName = cadImage.Objects
                .OfType<CadRasterImageDef>()
                .ToDictionary(d => d.FileName, d => d.ObjectHandle, StringComparer.OrdinalIgnoreCase);

            // Update each CadRasterImage entity to reference the correct definition handle
            foreach (var rasterEntity in cadImage.Entities.OfType<CadRasterImage>())
            {
                if (defByFileName.TryGetValue(rasterEntity.ImageDefReference, out string handle))
                {
                    // The reference already matches a definition handle; no change needed
                }
                else if (defByFileName.TryGetValue(rasterEntity.ImageDefReference, out string newHandle))
                {
                    rasterEntity.ImageDefReference = newHandle;
                }
                else
                {
                    // Fallback: try to match by existing definition handle
                    var matchingDef = cadImage.Objects
                        .OfType<CadRasterImageDef>()
                        .FirstOrDefault(d => d.ObjectHandle == rasterEntity.ImageDefReference);
                    if (matchingDef != null)
                    {
                        rasterEntity.ImageDefReference = matchingDef.ObjectHandle;
                    }
                }
            }

            // Ensure output directory exists
            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            // Save the modified CAD file preserving editable objects
            var saveOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Binary
            };

            cadImage.Save(outputPath);
            Console.WriteLine($"CAD file saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during processing or saving: {ex.Message}");
        }
    }
}