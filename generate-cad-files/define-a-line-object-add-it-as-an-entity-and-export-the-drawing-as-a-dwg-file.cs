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
        try
        {
            // Define input and output paths
            string inputPath = "input.dwg";
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            string outputPath = Path.Combine(outputDir, "output.dwg");

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the existing DWG drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Create start and end points for the line
                var startPoint = new Cad3DPoint(0, 0, 0);
                var endPoint = new Cad3DPoint(100, 100, 0);

                // Create a line entity using the appropriate constructor
                CadLine line = new CadLine(startPoint, endPoint);

                // Add the line to the drawing's entities
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { line }).ToArray();

                // Save the modified drawing as DWG
                DwgOptions dwgOptions = new DwgOptions();
                cadImage.Save(outputPath, dwgOptions);
            }

            Console.WriteLine($"Drawing saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}