using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Blocks;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.dwg";

        // Validate source file existence
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Source file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            // Load the DWG file
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Create a line entity (3D points required)
                var line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 100, 0)
                };

                // Create a closed lightweight polyline entity (2D points)
                var polyline = new CadLwPolyline
                {
                    Flag = CadLwPolylineFlag.Closed,
                    Coordinates = new List<Cad2DPoint>
                    {
                        new Cad2DPoint(10, 10),
                        new Cad2DPoint(50, 10),
                        new Cad2DPoint(50, 50),
                        new Cad2DPoint(10, 50)
                    }
                };

                // Add entities to the Model Space block
                if (cadImage.BlockEntities.TryGetValue("*Model_Space", out var blockObj) && blockObj is CadBlockEntity modelSpace)
                {
                    modelSpace.AddEntity(line);
                    modelSpace.AddEntity(polyline);
                }
                else
                {
                    Console.WriteLine("Model Space block not found.");
                }

                // Export with callback error handling
                try
                {
                    var exportOptions = new DwgOptions { OutputMode = CadOutputMode.Convert };
                    cadImage.Save(outputPath, exportOptions);
                    Console.WriteLine($"Export succeeded: {outputPath}");
                }
                catch (Exception exportEx)
                {
                    Console.WriteLine($"Export error: {exportEx.Message}");
                }
            }
        }
        catch (Exception loadEx)
        {
            Console.WriteLine($"Load error: {loadEx.Message}");
        }
    }
}