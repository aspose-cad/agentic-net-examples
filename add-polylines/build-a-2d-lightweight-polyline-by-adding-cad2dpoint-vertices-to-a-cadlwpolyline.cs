using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputDirectory = "output";
        string outputPath = Path.Combine(outputDirectory, "output.pdf");

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Create a lightweight polyline
                var polyline = new CadLwPolyline
                {
                    Coordinates = new List<Cad2DPoint>
                    {
                        new Cad2DPoint(0, 0),
                        new Cad2DPoint(100, 0),
                        new Cad2DPoint(100, 100),
                        new Cad2DPoint(0, 100),
                        new Cad2DPoint(0, 0) // close the shape
                    },
                    Flag = CadLwPolylineFlag.Closed
                };

                // Add the polyline to the native ModelSpace block if available
                const string modelSpaceKey = "*MODEL_SPACE";
                if (cadImage.BlockEntities != null && cadImage.BlockEntities.ContainsKey(modelSpaceKey))
                {
                    var modelSpace = cadImage.BlockEntities[modelSpaceKey];
                    modelSpace.AddEntity(polyline);
                }
                else
                {
                    Console.Error.WriteLine("Cannot add polyline to CAD image: ModelSpace block not found.");
                }

                // Save the modified CAD image as PDF
                var pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Output saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}