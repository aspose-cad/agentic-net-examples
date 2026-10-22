using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dxf";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the existing CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Define the offset that will be applied to each hatch pattern origin
                double offsetX = 10.0; // shift in X direction
                double offsetY = 5.0;  // shift in Y direction

                // Iterate through all entities in the drawing and process only hatches
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadHatch cadHatch)
                    {
                        // Ensure the hatch has pattern definitions before attempting to modify them
                        if (cadHatch.PatternDefinitions != null)
                        {
                            foreach (var patternData in cadHatch.PatternDefinitions)
                            {
                                // Retrieve the current line base point (pattern origin)
                                Cad2DPoint basePoint = patternData.LineBasePoint;

                                // Apply the desired offset
                                basePoint.X += offsetX;
                                basePoint.Y += offsetY;

                                // Store the updated base point back into the pattern data
                                patternData.LineBasePoint = basePoint;
                            }
                        }
                    }
                }

                // Save the modified drawing with explicit DXF options
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
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