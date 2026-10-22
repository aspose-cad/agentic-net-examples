using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dxf";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Modify line weight for all polyline entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLwPolyline lwPolyline)
                    {
                        // Example line weight value (in hundredths of mm)
                        lwPolyline.LineWeight = 25;
                    }
                    else if (entity is CadPolyline polyline3D)
                    {
                        polyline3D.LineWeight = 25;
                    }
                }

                string outputDir = "output";
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, "modified.dxf");

                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                cadImage.Save(outputPath, saveOptions);
                Console.WriteLine($"Modified CAD saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}