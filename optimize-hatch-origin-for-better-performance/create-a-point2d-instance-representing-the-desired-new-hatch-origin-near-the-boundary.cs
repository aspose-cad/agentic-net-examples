using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;

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

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (var cadImage = (CadImage)Image.Load(inputPath))
            {
                var hatch = cadImage.Entities.OfType<CadHatch>().FirstOrDefault();

                if (hatch != null && hatch.PatternDefinitions != null && hatch.PatternDefinitions.Count > 0)
                {
                    double newX = 100.0;
                    double newY = 50.0;

                    // Create a Point2D instance representing the new hatch origin
                    Point2D newOrigin = new Point2D(newX, newY);
                    Console.WriteLine($"New hatch origin: ({newOrigin.X}, {newOrigin.Y})");

                    // Apply the new origin to each pattern definition's LineBasePoint
                    foreach (var pattern in hatch.PatternDefinitions)
                    {
                        pattern.LineBasePoint = new Cad2DPoint(newX, newY);
                    }

                    // Optional: change hatch color using indexed color
                    hatch.ColorId = 1; // 1 = red in the default color table
                }

                cadImage.Save(outputPath);
                Console.WriteLine($"Modified drawing saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}