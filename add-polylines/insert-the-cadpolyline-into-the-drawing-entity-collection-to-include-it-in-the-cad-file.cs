using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AddPolylineExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Create a lightweight polyline
                    CadLwPolyline polyline = new CadLwPolyline();

                    // Add vertices to the polyline
                    polyline.Coordinates.Add(new Cad2DPoint(0.0, 0.0));
                    polyline.Coordinates.Add(new Cad2DPoint(100.0, 0.0));
                    polyline.Coordinates.Add(new Cad2DPoint(100.0, 100.0));
                    polyline.Coordinates.Add(new Cad2DPoint(0.0, 100.0));

                    // Insert the polyline into the Model Space block
                    if (cadImage.BlockEntities.TryGetValue("*Model_Space", out CadBlockEntity modelSpace))
                    {
                        modelSpace.Entities.Add(polyline);
                    }
                    else
                    {
                        Console.Error.WriteLine("Model Space block not found in the CAD document.");
                        return;
                    }

                    // Save the modified CAD document
                    cadImage.Save(outputPath);
                }

                Console.WriteLine($"Polyline added and file saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}