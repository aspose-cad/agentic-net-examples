using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

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

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Create a new lightweight polyline
                CadLwPolyline polyline = new CadLwPolyline();

                // Define vertices of the polyline
                polyline.Coordinates.Add(new Cad2DPoint(0, 0));
                polyline.Coordinates.Add(new Cad2DPoint(100, 0));
                polyline.Coordinates.Add(new Cad2DPoint(100, 100));
                polyline.Coordinates.Add(new Cad2DPoint(0, 100));
                polyline.Coordinates.Add(new Cad2DPoint(0, 0)); // close the polyline

                // Set optional appearance properties
                // LineWeight is in hundredths of millimeters; 0.25 mm = 25
                polyline.LineWeight = (short)25;

                // Add the polyline to the Model Space block's entity collection
                const string modelSpaceName = "*Model_Space";
                if (cadImage.BlockEntities.ContainsKey(modelSpaceName))
                {
                    cadImage.BlockEntities[modelSpaceName].Entities.Add(polyline);
                }
                else
                {
                    Console.Error.WriteLine("Model space block not found.");
                    return;
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
            }

            Console.WriteLine("Polyline added and drawing saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}