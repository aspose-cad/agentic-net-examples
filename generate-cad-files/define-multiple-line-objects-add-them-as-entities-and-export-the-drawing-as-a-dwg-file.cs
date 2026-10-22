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
        // Create a new empty DWG image
        using (var cadImage = new CadImage())
        {
            // First line entity
            var line1 = new CadLine
            {
                FirstPoint = new Cad3DPoint(0, 0, 0),
                SecondPoint = new Cad3DPoint(100, 0, 0),
                LayerName = "0"
            };

            // Second line entity
            var line2 = new CadLine
            {
                FirstPoint = new Cad3DPoint(100, 0, 0),
                SecondPoint = new Cad3DPoint(100, 100, 0),
                LayerName = "0"
            };

            // Add lines to the drawing
            cadImage.Entities = cadImage.Entities
                .Concat(new CadEntityBase[] { line1, line2 })
                .ToArray();

            // Ensure output directory exists
            string outputPath = "output.dwg";
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the drawing as DWG
            var dwgOptions = new DwgOptions();
            try
            {
                cadImage.Save(outputPath, dwgOptions);
                Console.WriteLine($"DWG file saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save DWG file: {ex.Message}");
            }
        }
    }
}