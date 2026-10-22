using System;
using System.Diagnostics;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output DXF file paths
            string inputPath = "input.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputPath = "output.dxf";

            // Load the DXF drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // NOTE: Aspose.CAD.Image does not expose a Cache property.
                // The requested Cache.ExactReallocateOnly setting is not available in the current SDK.
                Console.WriteLine("Cache.ExactReallocateOnly setting is not supported by the current Aspose.CAD API.");

                // Record memory usage before export
                long memoryBefore = GC.GetTotalMemory(true);

                // Configure DXF save options
                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    Version = DxfOutputVersion.R12,
                    DxfFileFormat = CadFileFormat.Binary
                };

                // Export the drawing
                cadImage.Save(outputPath, dxfOptions);

                // Record memory usage after export
                long memoryAfter = GC.GetTotalMemory(true);

                // Display memory consumption during export
                Console.WriteLine($"Memory used during export: {memoryAfter - memoryBefore} bytes");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}