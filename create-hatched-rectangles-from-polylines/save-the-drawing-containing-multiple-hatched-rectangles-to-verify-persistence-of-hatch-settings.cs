using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "template.dwg";
            CadImage cadImage;

            if (File.Exists(inputPath))
            {
                cadImage = (CadImage)Image.Load(inputPath);
            }
            else
            {
                Console.WriteLine($"Input file '{inputPath}' not found. Creating a new empty CAD image.");
                cadImage = new CadImage();
            }

            // First rectangle (2D lightweight polyline)
            var rect1 = new CadLwPolyline();
            rect1.Coordinates.Add(new Cad2DPoint(0, 0));
            rect1.Coordinates.Add(new Cad2DPoint(100, 0));
            rect1.Coordinates.Add(new Cad2DPoint(100, 50));
            rect1.Coordinates.Add(new Cad2DPoint(0, 50));
            rect1.Flag = CadLwPolylineFlag.Closed;

            // Second rectangle (offset from the first)
            var rect2 = new CadLwPolyline();
            rect2.Coordinates.Add(new Cad2DPoint(150, 0));
            rect2.Coordinates.Add(new Cad2DPoint(250, 0));
            rect2.Coordinates.Add(new Cad2DPoint(250, 80));
            rect2.Coordinates.Add(new Cad2DPoint(150, 80));
            rect2.Flag = CadLwPolylineFlag.Closed;

            // Ensure the model space block exists
            const string modelSpaceName = "*Model_Space";
            if (!cadImage.BlockEntities.ContainsKey(modelSpaceName))
            {
                cadImage.BlockEntities.Add(modelSpaceName, new CadBlockEntity());
            }

            var modelSpace = cadImage.BlockEntities[modelSpaceName];
            modelSpace.AddEntity(rect1);
            modelSpace.AddEntity(rect2);

            // Save the drawing preserving entities
            string outputPath = "output.dxf";
            var saveOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert
            };
            cadImage.Save(outputPath, saveOptions);

            Console.WriteLine($"Drawing saved to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}