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
            // Input and output paths
            string inputPath = "input.dwg";
            string outputDir = "output";
            string outputPath = Path.Combine(outputDir, "result.dwg");

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage for entity manipulation
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Move all entities by an offset (e.g., 10 units right, 5 units up)
                double offsetX = 10.0;
                double offsetY = 5.0;
                double offsetZ = 0.0;

                // Note: Direct translation of entities is not exposed in the public API.
                // If needed, custom logic would be applied here per entity type.

                // Add a title text entity at a specific location
                string titleText = "Drawing Title";
                // Position the title at (0,0,0) – adjust as needed
                Cad3DPoint titlePosition = new Cad3DPoint(0, 0, 0);
                CadText titleEntity = new CadText
                {
                    DefaultValue = titleText,
                    FirstAlignment = titlePosition,
                    TextHeight = 2.5
                };

                // Append the title entity to the drawing
                cadImage.Entities = cadImage.Entities
                    .Concat(new CadEntityBase[] { titleEntity })
                    .ToArray();

                // Save the modified drawing as DWG
                DwgOptions saveOptions = new DwgOptions();
                cadImage.Save(outputPath, saveOptions);
            }

            Console.WriteLine($"Modified drawing saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}