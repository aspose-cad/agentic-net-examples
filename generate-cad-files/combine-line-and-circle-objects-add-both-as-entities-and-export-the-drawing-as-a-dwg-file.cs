using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Create a new empty CAD image (DWG)
        using (var cadImage = new CadImage())
        {
            // Create a LINE entity
            var line = new CadLine
            {
                FirstPoint = new Cad3DPoint(0, 0, 0),
                SecondPoint = new Cad3DPoint(100, 100, 0),
                LayerName = "0"
            };

            // Create a CIRCLE entity
            var circle = new CadCircle
            {
                CenterPoint = new Cad3DPoint(50, 50, 0),
                Radius = 25,
                LayerName = "0"
            };

            // Add entities to the drawing
            cadImage.Entities = cadImage.Entities
                .Concat(new CadEntityBase[] { line, circle })
                .ToArray();

            // Prepare output path and ensure directory exists
            string outputPath = "output.dwg";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the drawing as DWG with error handling
            try
            {
                var dwgOptions = new DwgOptions();
                cadImage.Save(outputPath, dwgOptions);
                Console.WriteLine($"DWG file saved successfully to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving DWG file: {ex.Message}");
            }
        }
    }
}