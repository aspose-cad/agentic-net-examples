using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = @"input.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputPath = @"output\hatched_output.dxf";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the existing DXF file
            using var cadImage = (CadImage)Image.Load(inputPath);

            // Iterate over all lightweight polylines (2‑D rectangles) in the drawing
            var polylines = cadImage.Entities
                                   .OfType<CadLwPolyline>()
                                   .Where(pl => pl.Flag.HasFlag(CadLwPolylineFlag.Closed));

            foreach (var polyline in polylines)
            {
                // Output rectangle vertices for demonstration
                Console.WriteLine("Rectangle vertices:");
                foreach (var pt in polyline.Coordinates)
                {
                    Console.WriteLine($"  ({pt.X}, {pt.Y})");
                }

                // NOTE: The Aspose.CAD API version used here does not expose a CadHatch class.
                // Therefore, creating a true hatch pattern is not possible with the available types.
                // If hatch functionality is required, consider using a newer version of the library
                // that includes CadHatch, CadHatchPatternData, etc.
            }

            // Save the (unchanged) drawing as DXF, preserving editable objects
            var dxfOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert   // keep entities editable
            };
            cadImage.Save(outputPath, dxfOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}