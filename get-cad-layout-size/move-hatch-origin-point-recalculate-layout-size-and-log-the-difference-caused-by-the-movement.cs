using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class HatchOriginMover
{
    static void Main()
    {
        try
        {
            // Paths to source and destination CAD files
            string inputPath = "input.dxf";
            string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir))
                Directory.CreateDirectory(outDir);

            // Load the CAD drawing
            using (var cadImage = (CadImage)Image.Load(inputPath))
            {
                // Capture original layout extents
                var originalMin = cadImage.MinPoint;
                var originalMax = cadImage.MaxPoint;

                // Locate the first hatch entity in the drawing
                CadHatch hatch = null;
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadHatch foundHatch)
                    {
                        hatch = foundHatch;
                        break;
                    }
                }

                if (hatch != null)
                {
                    // NOTE: The Aspose.CAD SDK does not expose a direct API to move the hatch pattern origin.
                    // The pattern origin is defined by internal data not accessible via public members.
                    // Therefore, moving the hatch origin cannot be performed with the current SDK.
                    Console.WriteLine("Moving hatch origin is not supported by the Aspose.CAD SDK.");
                }
                else
                {
                    Console.WriteLine("No hatch entity found in the drawing.");
                }

                // Recalculate layout extents after attempted modification
                // The SDK does not provide an explicit method to force layout size recalculation.
                // Accessing MinPoint/MaxPoint after modifications reflects any changes that affect geometry.
                var newMin = cadImage.MinPoint;
                var newMax = cadImage.MaxPoint;

                // Log the differences in extents
                Console.WriteLine("Layout extents change caused by moving hatch origin:");
                Console.WriteLine($"Min X: {originalMin.X} -> {newMin.X} (Δ {newMin.X - originalMin.X})");
                Console.WriteLine($"Min Y: {originalMin.Y} -> {newMin.Y} (Δ {newMin.Y - originalMin.Y})");
                Console.WriteLine($"Max X: {originalMax.X} -> {newMax.X} (Δ {newMax.X - originalMax.X})");
                Console.WriteLine($"Max Y: {originalMax.Y} -> {newMax.Y} (Δ {newMax.Y - originalMax.Y})");

                // Save the (unchanged) drawing as DXF (binary format)
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary,
                    Version = DxfOutputVersion.R12
                };
                cadImage.Save(outputPath, dxfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}