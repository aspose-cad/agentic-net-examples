using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace LineWeightVerification
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Validate input arguments
                if (args.Length == 0 || string.IsNullOrEmpty(args[0]))
                {
                    Console.Error.WriteLine("Error: Input CAD file path must be provided as the first argument.");
                    return;
                }

                string inputPath = args[0];
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Error: Input file does not exist: {inputPath}");
                    return;
                }

                // Prepare output directory
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load CAD image
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Error: Loaded file is not a valid CAD image.");
                        return;
                    }

                    // Adjust line weight for regular (non‑insert) top‑level entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Skip block insert references
                        if (entity is CadInsertObject)
                            continue;

                        // Set line weight to a specific value (e.g., 25 = 0.25 mm)
                        // The LineWeight property expects a short representing the weight index.
                        entity.LineWeight = 25;
                    }

                    // Optionally adjust line weight inside block definitions as well
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            if (entity is CadInsertObject)
                                continue;

                            entity.LineWeight = 25;
                        }
                    }

                    // Render the modified drawing to PNG
                    string outputPath = Path.Combine(outputDir, "LineWeightAdjusted.png");
                    PngOptions pngOptions = new PngOptions
                    {
                        // Use default rendering options; can be customized if needed
                    };
                    cadImage.Save(outputPath, pngOptions);

                    Console.WriteLine($"Successfully saved rendered image with adjusted line weights to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}