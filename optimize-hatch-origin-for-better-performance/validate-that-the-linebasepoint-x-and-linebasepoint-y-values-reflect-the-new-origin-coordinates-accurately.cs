using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;

class HatchOriginValidator
{
    static void Main()
    {
        try
        {
            // Load an existing CAD drawing (DXF, DWG, etc.)
            string inputPath = "input.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Define the new origin coordinates for the hatch pattern
                double newOriginX = 150.0;
                double newOriginY = 75.0;

                // Iterate through all entities in the drawing to find hatches
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadHatch hatch)
                    {
                        // Ensure the PatternDefinitions collection is initialized
                        if (hatch.PatternDefinitions == null)
                            hatch.PatternDefinitions = new List<CadHatchPatternData>();

                        // Update each pattern definition's LineBasePoint
                        foreach (CadHatchPatternData patternData in hatch.PatternDefinitions)
                        {
                            patternData.LineBasePoint = new Cad2DPoint(newOriginX, newOriginY);
                        }

                        // Validation: confirm that the LineBasePoint values were set correctly
                        foreach (CadHatchPatternData patternData in hatch.PatternDefinitions)
                        {
                            if (Math.Abs(patternData.LineBasePoint.X - newOriginX) > 1e-6 ||
                                Math.Abs(patternData.LineBasePoint.Y - newOriginY) > 1e-6)
                            {
                                throw new InvalidOperationException(
                                    $"Hatch pattern origin mismatch. Expected ({newOriginX}, {newOriginY}) " +
                                    $"but found ({patternData.LineBasePoint.X}, {patternData.LineBasePoint.Y}).");
                            }
                        }
                    }
                }

                // Save the modified drawing (optional, if you want to persist changes)
                string outputPath = "output.dxf";
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}