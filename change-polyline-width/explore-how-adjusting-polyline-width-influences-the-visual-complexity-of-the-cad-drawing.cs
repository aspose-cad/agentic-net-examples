using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

namespace PolylineWidthDemo
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Prepare output directory
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                Directory.CreateDirectory(outputDir);

                // Input CAD file (placeholder)
                string inputPath = Path.Combine(Directory.GetCurrentDirectory(), "sample.dwg");
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Create first polyline with thin line weight
                    CadLwPolyline thinPolyline = new CadLwPolyline
                    {
                        Coordinates = new List<Cad2DPoint>
                        {
                            new Cad2DPoint(0, 0),
                            new Cad2DPoint(100, 0),
                            new Cad2DPoint(100, 100),
                            new Cad2DPoint(0, 100)
                        },
                        Flag = CadLwPolylineFlag.Closed,
                        LineWeight = 0 // default thin line
                    };

                    // Create second polyline with thick line weight
                    CadLwPolyline thickPolyline = new CadLwPolyline
                    {
                        Coordinates = new List<Cad2DPoint>
                        {
                            new Cad2DPoint(150, 0),
                            new Cad2DPoint(250, 0),
                            new Cad2DPoint(250, 100),
                            new Cad2DPoint(150, 100)
                        },
                        Flag = CadLwPolylineFlag.Closed,
                        LineWeight = 200 // example thick line weight
                    };

                    // Add polylines to the first block entity (ModelSpace)
                    var blockEntity = cadImage.BlockEntities.Values
                        .Cast<CadBlockEntity>()
                        .FirstOrDefault();

                    if (blockEntity != null)
                    {
                        blockEntity.AddEntity(thinPolyline);
                        blockEntity.AddEntity(thickPolyline);
                    }
                    else
                    {
                        Console.Error.WriteLine("No block entity found in the drawing.");
                    }

                    // Save modified drawing
                    string outputPath = Path.Combine(outputDir, "Modified.dxf");
                    var saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath, saveOptions);
                    Console.WriteLine($"Modified drawing saved to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}