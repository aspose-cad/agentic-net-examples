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
            // Input DWG file (template). Replace with a valid path if available.
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the existing CAD drawing.
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // NOTE: Aspose.CAD 26.9 does not provide a concrete entity class for a POINT.
                // CadEntityBase is abstract and its TypeName property is read‑only, so a POINT
                // entity cannot be instantiated directly. Therefore adding a POINT object
                // is not supported with the current SDK version.

                // If a future SDK version introduces a concrete point entity (e.g., CadPointEntity),
                // the code to add it would resemble:
                // var pointEntity = new CadPointEntity { /* set properties */ };
                // cadImage.Entities = cadImage.Entities.Concat(new[] { pointEntity }).ToArray();

                // Prepare output directory.
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, "output.dwg");

                // Save the (unchanged) drawing as DWG.
                cadImage.Save(outputPath, null);
                Console.WriteLine($"Drawing saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}