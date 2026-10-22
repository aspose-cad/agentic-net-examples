using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.Primitives;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CadHatchOriginOptimization
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dxf";
            string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            CadImage cadImage;
            try
            {
                cadImage = (CadImage)Image.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load CAD file: {ex.Message}");
                return;
            }

            foreach (CadEntityBase entity in cadImage.Entities)
            {
                if (entity.TypeName == CadEntityTypeName.HATCH)
                {
                    CadHatch hatch = (CadHatch)entity;

                    // Example: set true color to red (optional)
                    hatch.ColorValue = 0xFF0000; // RGB hex for red

                    if (hatch.PatternDefinitions != null)
                    {
                        foreach (CadHatchPatternData patternData in hatch.PatternDefinitions)
                        {
                            // Move the pattern origin by a fixed offset (+10, +10)
                            Cad2DPoint basePoint = patternData.LineBasePoint;
                            patternData.LineBasePoint = new Cad2DPoint(basePoint.X + 10, basePoint.Y + 10);
                        }
                    }
                }
            }

            try
            {
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                cadImage.Save(outputPath, new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                });

                Console.WriteLine($"CAD file saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save CAD file: {ex.Message}");
            }
        }
    }
}