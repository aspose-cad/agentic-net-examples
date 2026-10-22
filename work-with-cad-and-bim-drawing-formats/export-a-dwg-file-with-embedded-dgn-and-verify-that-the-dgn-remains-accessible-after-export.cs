using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExportDwgWithDgn
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input and output file paths (replace with actual paths as needed)
                string inputPath = "input.dwg";
                string outputPath = "exported.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the DWG file (which contains an embedded DGN)
                using (Image loadedImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    if (loadedImage is not CadImage cadImage)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Export the DWG while preserving embedded objects
                    var saveOptions = new DwgOptions
                    {
                        // Preserve native CAD objects; no conversion to render mode
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath, saveOptions);
                }

                // Verify that the exported DWG still contains the embedded DGN
                if (!File.Exists(outputPath))
                {
                    Console.Error.WriteLine($"Exported file not found: {outputPath}");
                    return;
                }

                using (Image exportedImage = Image.Load(outputPath))
                {
                    if (exportedImage is not CadImage exportedCad)
                    {
                        Console.Error.WriteLine("Failed to cast exported image to CadImage.");
                        return;
                    }

                    // Simple verification: check that there is at least one block entity,
                    // which is where an embedded DGN would be stored.
                    int blockCount = exportedCad.BlockEntities.Count;
                    Console.WriteLine($"Export succeeded. Block entity count in exported DWG: {blockCount}");

                    // List block names
                    foreach (KeyValuePair<string, CadBlockEntity> kvp in exportedCad.BlockEntities)
                    {
                        Console.WriteLine($"Block name: {kvp.Key}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}