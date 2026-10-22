using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;

namespace CadRectangleWithHatch
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define input and output paths
                string inputPath = "input.dxf"; // placeholder path to an existing DXF file
                string outputDir = "output";
                string outputPath = Path.Combine(outputDir, "RectangleWithHatch.dxf");

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the source CAD file (can be an empty template)
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Create a lightweight polyline representing a rectangle
                    CadLwPolyline rectangle = new CadLwPolyline();
                    rectangle.Coordinates.Add(new Cad2DPoint(0, 0));
                    rectangle.Coordinates.Add(new Cad2DPoint(100, 0));
                    rectangle.Coordinates.Add(new Cad2DPoint(100, 50));
                    rectangle.Coordinates.Add(new Cad2DPoint(0, 50));
                    rectangle.Flag = CadLwPolylineFlag.Closed;

                    // Create a solid-filled face to simulate a hatch
                    Cad3DFace solidFace = new Cad3DFace(
                        new Cad3DPoint(0, 0, 0),
                        new Cad3DPoint(100, 0, 0),
                        new Cad3DPoint(100, 50, 0),
                        new Cad3DPoint(0, 50, 0));

                    // Add the rectangle and the solid face to the model space block
                    if (!cadImage.BlockEntities.TryGetValue("*Model_Space", out CadBlockEntity modelBlock))
                    {
                        Console.Error.WriteLine("Model space block not found in the CAD file.");
                        return;
                    }

                    modelBlock.AddEntity(rectangle);
                    modelBlock.AddEntity(solidFace);

                    // Save the modified CAD drawing
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath, saveOptions);
                }

                Console.WriteLine("Rectangle with simulated hatch saved successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}