using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Path for the modified CAD file
            string outputPath = "output/output.dwg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load the CAD image.");
                    return;
                }

                // Create a new CadText entity using property initializers
                Cad3DPoint insertionPoint = new Cad3DPoint(0, 0, 0);
                double textHeight = 2.0;

                CadText newText = new CadText
                {
                    DefaultValue = "Hello World",
                    FirstAlignment = insertionPoint,
                    TextHeight = textHeight,
                    TextRotation = 0.0,
                    SoftOwner = cadImage.Layouts.Values.FirstOrDefault()?.BlockTableRecordHandle ?? string.Empty
                };

                // Add the text entity to the drawing's top‑level entities collection
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { newText }).ToArray();

                // Save the modified drawing
                cadImage.Save(outputPath);
            }

            Console.WriteLine("Modified CAD file saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}