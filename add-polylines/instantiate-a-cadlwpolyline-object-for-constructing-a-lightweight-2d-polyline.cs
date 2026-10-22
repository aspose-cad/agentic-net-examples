using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

namespace CadPolylineExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input and output file paths (placeholders)
                string inputPath = "input.dwg";
                string outputPath = "output/output.dwg";

                // Guard input file existence
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the source CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Create a lightweight 2D polyline
                    CadLwPolyline polyline = new CadLwPolyline();

                    // Define vertices of the polyline (a simple rectangle)
                    List<Cad2DPoint> vertices = new List<Cad2DPoint>
                    {
                        new Cad2DPoint(0, 0),
                        new Cad2DPoint(100, 0),
                        new Cad2DPoint(100, 100),
                        new Cad2DPoint(0, 100)
                    };
                    polyline.Coordinates = vertices;

                    // Close the polyline to form a closed shape
                    polyline.Flag = CadLwPolylineFlag.Closed;

                    // Add the polyline to the CAD image entities
                    cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { polyline }).ToArray();

                    // Save the modified CAD file with default options
                    cadImage.Save(outputPath);
                }

                Console.WriteLine("Polyline added and file saved successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
