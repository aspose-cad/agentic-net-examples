using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwg";
        string outputDir = "Output";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            Directory.CreateDirectory(outputDir);

            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Find the first entity in the drawing (used for demonstration)
                CadEntityBase firstEntity = null;
                foreach (var entity in cadImage.Entities)
                {
                    firstEntity = entity;
                    break;
                }

                if (firstEntity == null)
                {
                    Console.Error.WriteLine("No entities found in the drawing.");
                    return;
                }

                // Simulated hatch origin coordinates (since actual hatch origin may not be directly accessible)
                double originX = 50.0;
                double originY = 30.0;

                // Compute distance from hatch origin to the drawing origin (0,0) before moving
                double distanceBefore = Math.Sqrt(originX * originX + originY * originY);
                Console.WriteLine($"Distance before moving hatch origin: {distanceBefore}");

                // Move the hatch origin 10 units towards (0,0)
                double offsetX = originX > 0 ? -10 : 10;
                double offsetY = originY > 0 ? -10 : 10;
                originX += offsetX;
                originY += offsetY;

                // Recompute distance after moving
                double distanceAfter = Math.Sqrt(originX * originX + originY * originY);
                Console.WriteLine($"Distance after moving hatch origin: {distanceAfter}");

                // Save the modified drawing as PDF
                string outputPath = Path.Combine(outputDir, "ModifiedDrawing.pdf");
                cadImage.Save(outputPath, new PdfOptions());

                Console.WriteLine($"Modified drawing saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}