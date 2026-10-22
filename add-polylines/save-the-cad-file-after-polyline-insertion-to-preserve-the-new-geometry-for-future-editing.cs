using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.dwg";
        string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image loadedImage = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)loadedImage;

                // Define vertices for the polyline
                Cad2DPoint[] vertices = new Cad2DPoint[]
                {
                    new Cad2DPoint(0, 0),
                    new Cad2DPoint(100, 0),
                    new Cad2DPoint(100, 100),
                    new Cad2DPoint(0, 100),
                    new Cad2DPoint(0, 0)
                };

                // Create a lightweight polyline entity and assign vertices
                CadLwPolyline polyline = new CadLwPolyline();
                polyline.Coordinates.AddRange(vertices);
                polyline.Flag = CadLwPolylineFlag.Closed;
                polyline.LayerName = "0";

                // Add the polyline to the CAD image entities
                cadImage.Entities = cadImage.Entities
                    .Concat(new CadEntityBase[] { polyline })
                    .ToArray();

                // Save the modified CAD file
                cadImage.Save(outputPath);
            }

            Console.WriteLine($"CAD file saved successfully to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}
