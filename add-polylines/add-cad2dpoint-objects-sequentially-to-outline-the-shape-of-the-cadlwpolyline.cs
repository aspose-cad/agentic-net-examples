using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.cad";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the CAD image
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Create a lightweight polyline
            CadLwPolyline polyline = new CadLwPolyline
            {
                Flag = CadLwPolylineFlag.Closed,
                // LineWeight is a short (in hundredths of mm). 0.25 mm = 25 hundredths.
                LineWeight = (short)25
            };

            // Add vertices sequentially to outline the shape
            polyline.Coordinates.Add(new Cad2DPoint(0, 0));
            polyline.Coordinates.Add(new Cad2DPoint(100, 0));
            polyline.Coordinates.Add(new Cad2DPoint(100, 100));
            polyline.Coordinates.Add(new Cad2DPoint(0, 100));
            // Closing point is implicit due to the Closed flag

            // Add the polyline to the model space block
            if (cadImage.BlockEntities.TryGetValue("*Model_Space", out var modelSpace))
            {
                modelSpace.AddEntity(polyline);
            }
            else
            {
                Console.Error.WriteLine("Model space block not found.");
                return;
            }

            // Save the modified CAD file
            cadImage.Save(outputPath);

            Console.WriteLine($"File saved successfully to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
