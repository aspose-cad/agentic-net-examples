using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare input and output paths
            string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);

            string inputPath = Path.Combine(dataDir, "sample.dwg");
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("Input CAD file not found: " + inputPath);
                return;
            }

            string outputPath = Path.Combine(outputDir, "result.png");

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Create a lightweight polyline
                var polyline = new CadLwPolyline
                {
                    Coordinates = new List<Cad2DPoint>
                    {
                        new Cad2DPoint(0, 0),
                        new Cad2DPoint(100, 0),
                        new Cad2DPoint(100, 100),
                        new Cad2DPoint(0, 100)
                    },
                    Flag = CadLwPolylineFlag.Closed
                };

                // Add the polyline to the model space block or first available block
                if (cadImage is CadImage cadImg)
                {
                    if (cadImg.BlockEntities.Contains("*Model_Space"))
                    {
                        var modelSpace = (CadBlockEntity)cadImg.BlockEntities["*Model_Space"];
                        modelSpace.AddEntity(polyline);
                    }
                    else
                    {
                        // Fallback: add to the first available block
                        foreach (DictionaryEntry entry in cadImg.BlockEntities)
                        {
                            var block = (CadBlockEntity)entry.Value;
                            block.AddEntity(polyline);
                            break;
                        }
                    }
                }

                // Set rasterization options for PNG output
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1000,
                    PageHeight = 1000,
                    DrawType = CadDrawTypeMode.UseObjectColor
                };

                // Configure PNG options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the modified drawing as PNG
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine("Saved output to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}