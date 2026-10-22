using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace HatchOriginAdjustment
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source DWG/DXF file
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Path for the modified drawing
                string outputPath = "output.dwg";

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the loaded image to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate over top‑level entities in the drawing
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Identify hatch entities by type name (avoids direct CadHatch reference)
                        if (entity.GetType().Name == "CadHatch")
                        {
                            var hatchType = entity.GetType();

                            // Get the PatternOrigin property via reflection
                            var originProp = hatchType.GetProperty("PatternOrigin");
                            if (originProp != null && originProp.CanRead && originProp.CanWrite)
                            {
                                var currentOrigin = originProp.GetValue(entity) as Cad2DPoint;
                                if (currentOrigin != null)
                                {
                                    // Adjust the hatch pattern origin (shift by +10 units on X and Y)
                                    var newOrigin = new Cad2DPoint(currentOrigin.X + 10.0, currentOrigin.Y + 10.0);
                                    originProp.SetValue(entity, newOrigin);
                                }
                            }
                        }
                    }

                    // Save the modified drawing preserving the original format
                    cadImage.Save(outputPath, new DxfOptions { OutputMode = CadOutputMode.Convert });
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}