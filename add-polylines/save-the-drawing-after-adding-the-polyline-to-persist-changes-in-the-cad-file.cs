using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            string outputDirectory = "output";
            string outputPath = Path.Combine(outputDirectory, "output.dwg");

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            using (Image loadedImage = Image.Load(inputPath))
            {
                CadImage cadImage = loadedImage as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                var polyline = new CadLwPolyline
                {
                    Flag = CadLwPolylineFlag.None
                };
                polyline.Coordinates.Add(new Cad2DPoint(0, 0));
                polyline.Coordinates.Add(new Cad2DPoint(100, 0));
                polyline.Coordinates.Add(new Cad2DPoint(100, 100));

                const string modelSpaceKey = "*MODEL_SPACE";
                bool added = false;
                if (cadImage.BlockEntities != null && cadImage.BlockEntities.ContainsKey(modelSpaceKey))
                {
                    var modelSpace = cadImage.BlockEntities[modelSpaceKey];
                    modelSpace.AddEntity(polyline);
                    added = true;
                }

                if (!added)
                {
                    Console.Error.WriteLine("Unable to add polyline: ModelSpace block not found or AddEntity not available.");
                }

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