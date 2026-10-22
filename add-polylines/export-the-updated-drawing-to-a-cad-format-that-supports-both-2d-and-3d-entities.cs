using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AddPolylinesExample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "input.dwg";
            const string outputPath = "output.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Create a 2D closed polyline.
                    CadLwPolyline polyline = new CadLwPolyline();
                    polyline.Coordinates.Add(new Cad2DPoint(0, 0));
                    polyline.Coordinates.Add(new Cad2DPoint(100, 0));
                    polyline.Coordinates.Add(new Cad2DPoint(100, 100));
                    polyline.Coordinates.Add(new Cad2DPoint(0, 100));
                    polyline.Flag = CadLwPolylineFlag.Closed;

                    // Add the polyline to the drawing entities.
                    cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { polyline }).ToArray();

                    // List block definitions.
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block name: {block.Name}");
                    }

                    // Save the updated drawing.
                    cadImage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}