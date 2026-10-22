using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

namespace CadPolylineExample
{
    class Program
    {
        static void Main()
        {
            const string inputPath = "input.dwg";
            const string outputPath = "output.pdf";

            // Ensure output directory exists
            var outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load CAD image (or create a new empty one if file not found)
            CadImage cadImage;
            if (File.Exists(inputPath))
            {
                cadImage = (CadImage)Image.Load(inputPath);
            }
            else
            {
                Console.WriteLine($"Input file \"{inputPath}\" not found. Creating a new empty CAD image.");
                cadImage = new CadImage();
            }

            // Define the vertices of the 2D polyline
            Cad2DPoint[] vertices = new Cad2DPoint[]
            {
                new Cad2DPoint(0, 0),
                new Cad2DPoint(100, 0),
                new Cad2DPoint(100, 100),
                new Cad2DPoint(0, 100)
            };

            // Create an open lightweight polyline (LwPolyline) using the defined vertices
            var polyline = new CadLwPolyline
            {
                Coordinates = vertices.ToList(),
                Flag = CadLwPolylineFlag.None // use None for open polyline; set Closed for closed
            };

            // Add the polyline to the CAD image
            cadImage.Entities = cadImage.Entities.Concat(new[] { polyline }).ToArray();

            // Save the modified CAD file as PDF
            var saveOptions = new PdfOptions();
            try
            {
                cadImage.Save(outputPath, saveOptions);
                Console.WriteLine($"Successfully saved PDF to \"{outputPath}\".");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during save: {ex.Message}");
            }
        }
    }
}
