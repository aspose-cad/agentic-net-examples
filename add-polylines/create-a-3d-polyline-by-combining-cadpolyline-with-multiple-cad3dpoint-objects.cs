using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace AddPolylinesExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                string inputPath = "input.dwg";
                string outputPath = "output.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Create a 3D polyline with the required vertices
                    Cad3DPoint[] vertices = new Cad3DPoint[]
                    {
                        new Cad3DPoint(0, 0, 0),
                        new Cad3DPoint(100, 0, 0),
                        new Cad3DPoint(100, 100, 0),
                        new Cad3DPoint(0, 100, 0),
                        new Cad3DPoint(0, 0, 0) // close the loop
                    };

                    CadPolyline polyline = new CadPolyline(vertices);

                    // Append the new polyline to the existing entities
                    cadImage.Entities = cadImage.Entities.Concat(new[] { polyline }).ToArray();

                    // Save the modified drawing
                    DwgOptions saveOptions = new DwgOptions();
                    cadImage.Save(outputPath, saveOptions);
                }

                Console.WriteLine("Polyline added and drawing saved successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
