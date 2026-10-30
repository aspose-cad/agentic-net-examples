using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;

namespace Add3DPolylineExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input CAD file path (must exist)
                string inputPath = "input.dwg";
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Create a 3D polyline with vertices
                    var points = new Cad3DPoint[]
                    {
                        new Cad3DPoint(0, 0, 0),
                        new Cad3DPoint(100, 0, 0),
                        new Cad3DPoint(100, 100, 0),
                        new Cad3DPoint(0, 100, 0),
                        new Cad3DPoint(0, 0, 10)
                    };
                    CadPolyline polyline = new CadPolyline(points);

                    // Insert the polyline into the native ModelSpace block
                    if (cadImage.BlockEntities.TryGetValue("*MODEL_SPACE", out CadBlockEntity modelSpace))
                    {
                        modelSpace.AddEntity(polyline);
                    }
                    else
                    {
                        Console.Error.WriteLine("ModelSpace block not found in the CAD file.");
                        return;
                    }

                    // Prepare output directory
                    string outputDir = "output";
                    Directory.CreateDirectory(outputDir);
                    string outputPath = Path.Combine(outputDir, "output.pdf");

                    // Set rasterization options for PDF export
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        Layouts = new[] { "Model" }
                    };

                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the modified drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"Saved output to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}