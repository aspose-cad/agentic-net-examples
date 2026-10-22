using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.Primitives;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class HatchOriginAdjuster
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Gather all hatch entities from the main layout
            List<CadHatch> hatchEntities = cadImage.Entities
                .OfType<CadHatch>()
                .ToList();

            // Adjust each hatch's origin and color
            foreach (var hatch in hatchEntities)
            {
                // Set hatch indexed color (e.g., 1 = red)
                hatch.ColorId = (short)1;

                // Shift pattern origin if pattern definitions exist
                if (hatch.PatternDefinitions != null && hatch.PatternDefinitions.Count > 0)
                {
                    foreach (var patternDef in hatch.PatternDefinitions)
                    {
                        // Example offset – shift pattern origin by (10, 5) drawing units
                        double offsetX = 10.0;
                        double offsetY = 5.0;

                        var basePt = patternDef.LineBasePoint;
                        if (basePt == null)
                        {
                            basePt = new Cad2DPoint(0, 0);
                        }

                        patternDef.LineBasePoint = new Cad2DPoint(basePt.X + offsetX, basePt.Y + offsetY);
                    }
                }
            }

            // Ensure output directory exists
            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            // Save the modified drawing preserving editable objects
            DxfOptions saveOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Binary
            };

            cadImage.Save(outputPath, saveOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}