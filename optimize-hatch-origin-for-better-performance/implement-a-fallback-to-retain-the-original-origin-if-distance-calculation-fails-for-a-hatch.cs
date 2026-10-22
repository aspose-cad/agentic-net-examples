using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.Primitives;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class HatchOriginFallbackExample
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output.dxf";

        // Guard against missing input file
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Load the CAD file
        CadImage cadImage = (CadImage)Image.Load(inputPath);

        // Prepare a list to hold modified entities
        List<CadEntityBase> modifiedEntities = new List<CadEntityBase>();

        // Iterate through all entities in the image
        foreach (CadEntityBase entity in cadImage.Entities)
        {
            // Process only hatch entities
            if (entity.TypeName == CadEntityTypeName.HATCH)
            {
                CadHatch hatch = (CadHatch)entity;

                // Ensure pattern definitions are present
                if (hatch.PatternDefinitions != null && hatch.PatternDefinitions.Count > 0)
                {
                    // Keep a copy of the original base points for fallback
                    List<Cad2DPoint> originalBasePoints = hatch.PatternDefinitions
                        .Select(p => new Cad2DPoint(p.LineBasePoint.X, p.LineBasePoint.Y))
                        .ToList();

                    try
                    {
                        // Example: shift the pattern origin by a user‑defined offset
                        double offsetX = 10.0;
                        double offsetY = 5.0;

                        foreach (CadHatchPatternData patternData in hatch.PatternDefinitions)
                        {
                            // Calculate new base point
                            Cad2DPoint newBasePoint = new Cad2DPoint(
                                patternData.LineBasePoint.X + offsetX,
                                patternData.LineBasePoint.Y + offsetY);

                            // Simple distance check (e.g., distance from origin should be > 0)
                            double distance = Math.Sqrt(newBasePoint.X * newBasePoint.X + newBasePoint.Y * newBasePoint.Y);

                            // If distance calculation yields an invalid result, trigger fallback
                            if (double.IsNaN(distance) || double.IsInfinity(distance))
                                throw new InvalidOperationException("Invalid distance calculated.");

                            // Apply the new base point
                            patternData.LineBasePoint = newBasePoint;
                        }

                        // Note: CadHatch.RecalculateSize is not available in the current SDK version.
                        // Size recalculation is omitted.
                    }
                    catch
                    {
                        // Fallback: restore original base points to retain original origin
                        for (int i = 0; i < hatch.PatternDefinitions.Count && i < originalBasePoints.Count; i++)
                        {
                            hatch.PatternDefinitions[i].LineBasePoint = originalBasePoints[i];
                        }

                        // Note: RecalculateSize is unavailable; no further action required.
                    }
                }
            }

            // Add the (potentially modified) entity to the new collection
            modifiedEntities.Add(entity);
        }

        // Replace the image's entity collection with the modified one
        cadImage.Entities = modifiedEntities.ToArray();

        // Save the modified CAD file preserving editable objects
        var dxfOptions = new DxfOptions
        {
            OutputMode = CadOutputMode.Convert,
            DxfFileFormat = CadFileFormat.Binary // Choose Binary or Ascii as needed
        };

        cadImage.Save(outputPath, dxfOptions);
    }
}