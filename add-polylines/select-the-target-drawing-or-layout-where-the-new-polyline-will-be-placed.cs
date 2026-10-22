using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            string outputPath = "output.png";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine("Input file not found.");
                return;
            }

            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Add a closed polyline to the Model Space block
                AddPolylineToModelSpace(cadImage);

                // Rasterization options (optional layout selection)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = new[] { "*Model_Space*" }
                };

                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine("Polyline added and saved to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    static void AddPolylineToModelSpace(CadImage cadImage)
    {
        // Create a lightweight closed polyline
        CadLwPolyline polyline = new CadLwPolyline
        {
            Flag = CadLwPolylineFlag.Closed
        };

        // Define vertices of the polyline
        polyline.Coordinates.Add(new Cad2DPoint(0, 0));
        polyline.Coordinates.Add(new Cad2DPoint(100, 0));
        polyline.Coordinates.Add(new Cad2DPoint(100, 100));
        polyline.Coordinates.Add(new Cad2DPoint(0, 100));

        // Insert the polyline into the Model Space block
        if (cadImage.BlockEntities.TryGetValue("*Model_Space*", out var modelSpaceBlock))
        {
            modelSpaceBlock.AddEntity(polyline);
        }
        else
        {
            Console.WriteLine("Model Space block not found. Polyline not added.");
        }
    }
}
