using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class HatchOriginModifier
{
    static void Main()
    {
        // Input and output paths
        string inputFile = @"C:\CAD\input.dxf";
        string outputFile = @"C:\CAD\output.dxf";

        // Validate input file
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputFile);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputFile))
            {
                // Iterate over all entities (model space and paper space)
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadHatch hatch)
                    {
                        // Change hatch color (indexed color 1 = red)
                        hatch.ColorId = 1;

                        // Adjust pattern origin for patterned hatches
                        if (hatch.PatternDefinitions != null)
                        {
                            foreach (CadHatchPatternData pattern in hatch.PatternDefinitions)
                            {
                                double offsetX = 10.0;
                                double offsetY = 20.0;
                                pattern.LineBasePoint = new Cad2DPoint(offsetX, offsetY);
                            }
                        }
                    }
                }

                // Save the modified CAD file with explicit DXF format
                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };
                cadImage.Save(outputFile, saveOptions);
                Console.WriteLine($"File saved successfully to {outputFile}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}