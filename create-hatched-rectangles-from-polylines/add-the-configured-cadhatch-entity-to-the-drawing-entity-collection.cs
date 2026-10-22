using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

class Program
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputDir = "Output";
        string outputPath = Path.Combine(outputDir, "output.dxf");

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            Directory.CreateDirectory(outputDir);

            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Create a rectangular polyline (as a placeholder for hatch)
                var rectangle = new CadLwPolyline
                {
                    Flag = CadLwPolylineFlag.Closed
                };

                // Define rectangle corners (example coordinates)
                rectangle.Coordinates.Add(new Cad2DPoint(0, 0));
                rectangle.Coordinates.Add(new Cad2DPoint(100, 0));
                rectangle.Coordinates.Add(new Cad2DPoint(100, 50));
                rectangle.Coordinates.Add(new Cad2DPoint(0, 50));

                // Add the rectangle to the model space block entity
                CadBlockEntity modelSpace = cadImage.BlockEntities.ValuesTyped.First();
                modelSpace.AddEntity(rectangle);

                // Save the modified drawing preserving editable objects
                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                cadImage.Save(outputPath);
            }

            Console.WriteLine($"Drawing saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}