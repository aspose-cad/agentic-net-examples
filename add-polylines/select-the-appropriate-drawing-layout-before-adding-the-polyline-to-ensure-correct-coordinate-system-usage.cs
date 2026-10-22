using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

namespace AddPolylineExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input DWG/DXF file path
                string inputPath = "input.dwg";
                // Output PNG file path
                string outputPath = "output.png";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Locate the Model Space block where drawing entities reside
                    CadBlockEntity modelSpace = null;
                    foreach (DictionaryEntry entry in cadImage.BlockEntities)
                    {
                        if ((string)entry.Key == "*Model_Space")
                        {
                            modelSpace = (CadBlockEntity)entry.Value;
                            break;
                        }
                    }

                    // Fallback to the first block if Model Space is not found
                    if (modelSpace == null && cadImage.BlockEntities.Count > 0)
                    {
                        foreach (DictionaryEntry entry in cadImage.BlockEntities)
                        {
                            modelSpace = (CadBlockEntity)entry.Value;
                            break;
                        }
                    }

                    if (modelSpace == null)
                    {
                        Console.WriteLine("No block entities found in the drawing.");
                        return;
                    }

                    // Create a lightweight polyline (LWPOLYLINE) entity
                    CadLwPolyline polyline = new CadLwPolyline();

                    // Define vertices – a simple square
                    var points = new List<Cad2DPoint>
                    {
                        new Cad2DPoint(0, 0),
                        new Cad2DPoint(100, 0),
                        new Cad2DPoint(100, 100),
                        new Cad2DPoint(0, 100)
                    };
                    polyline.Coordinates = points;
                    polyline.Flag = CadLwPolylineFlag.Closed;

                    // Add the polyline to the Model Space block
                    modelSpace.AddEntity(polyline);

                    // Prepare PNG export options with layout selection
                    PngOptions pngOptions = new PngOptions();
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new[] { "*Model_Space" },
                        PageWidth = 1600,
                        PageHeight = 1200
                    };
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the modified drawing as PNG
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"Polyline added and image saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}
