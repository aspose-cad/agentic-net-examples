using Aspose.CAD.FileFormats.Cad.CadConsts;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

namespace CadEntityReplacementExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path (replace with actual file)
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Output image file path
                string outputPath = "output.png";

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Create a new list of CAD entities
                    var newEntityList = new List<CadEntityBase>();

                    // Create a line entity
                    var line = new CadLine(
                        new Cad3DPoint(0, 0, 0),
                        new Cad3DPoint(100, 100, 0));
                    newEntityList.Add(line);

                    // Create a lightweight polyline entity
                    var polyline = new CadLwPolyline
                    {
                        Coordinates = new List<Cad2DPoint>
                        {
                            new Cad2DPoint(150, 150),
                            new Cad2DPoint(200, 150),
                            new Cad2DPoint(200, 200),
                            new Cad2DPoint(150, 200)
                        },
                        Flag = CadLwPolylineFlag.Closed
                    };
                    newEntityList.Add(polyline);

                    // Replace the drawing's entity collection with the new list
                    cadImage.Entities = newEntityList.ToArray();

                    // Save the modified drawing as PNG
                    var pngOptions = new PngOptions();
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"Image saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}