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
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image loadedImage = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)loadedImage;

                // Create a lightweight polyline (polygon) and add vertices
                CadLwPolyline polyline = new CadLwPolyline();
                polyline.Coordinates.Add(new Cad2DPoint(0, 0));
                polyline.Coordinates.Add(new Cad2DPoint(100, 0));
                polyline.Coordinates.Add(new Cad2DPoint(100, 100));
                polyline.Coordinates.Add(new Cad2DPoint(0, 100));
                polyline.Flag = CadLwPolylineFlag.Closed;
                polyline.PointCount = polyline.Coordinates.Count;

                // Add the polyline to the Model space block
                if (cadImage.BlockEntities.TryGetValue("Model", out CadBlockEntity modelBlock))
                {
                    modelBlock.AddEntity(polyline);
                }
                else
                {
                    Console.WriteLine("Model block not found.");
                }

                // Set rasterization options for rendering
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600,
                    Layouts = new[] { "Model" }
                };

                // Set PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the modified CAD as PNG
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
