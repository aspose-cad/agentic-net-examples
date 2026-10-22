using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            Image cadImage = Image.Load(inputPath);
            CadImage cad = (CadImage)cadImage;

            // Define polyline points (square)
            List<Cad2DPoint> points = new List<Cad2DPoint>
            {
                new Cad2DPoint(0, 0),
                new Cad2DPoint(100, 0),
                new Cad2DPoint(100, 100),
                new Cad2DPoint(0, 100),
                new Cad2DPoint(0, 0)
            };

            // Create lightweight polyline
            CadLwPolyline polyline = new CadLwPolyline();
            polyline.Coordinates.AddRange(points);
            polyline.Flag = CadLwPolylineFlag.Closed;
            // polyline.LayerName = "0"; // Set layer if required

            // Locate the Model Space block
            CadBlockEntity modelBlock = null;
            foreach (CadBlockEntity block in cad.BlockEntities.ValuesTyped)
            {
                if (block.Name.Equals("*Model_Space", StringComparison.OrdinalIgnoreCase))
                {
                    modelBlock = block;
                    break;
                }
            }

            if (modelBlock == null)
            {
                Console.WriteLine("Model space block not found.");
                return;
            }

            // Add the polyline to the model space
            modelBlock.AddEntity(polyline);

            // Set rasterization options
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions();

            // Save as PDF
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            cad.Save(outputPath, pdfOptions);
            Console.WriteLine($"PDF saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}