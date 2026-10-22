using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD file path (replace with actual path)
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Output directory and file path
            string outputDir = "output";
            string outputPath = Path.Combine(outputDir, "output.png");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cad = (CadImage)cadImage;

                // Locate the Model Space block entity
                CadBlockEntity modelBlock = null;
                foreach (CadBlockEntity block in cad.BlockEntities.ValuesTyped)
                {
                    if (block.Name == "*Model_Space")
                    {
                        modelBlock = block;
                        break;
                    }
                }

                if (modelBlock != null)
                {
                    // Create a new CadText entity using dynamic to avoid compile‑time member issues
                    dynamic textEntity = new CadText();
                    textEntity.TextString = "Hello Aspose.CAD";
                    textEntity.InsertionPoint = new Cad3DPoint(0.0, 0.0, 0.0);
                    textEntity.Height = 2.5;

                    // Ensure SoftOwner matches the Model Space block's handle (using dynamic for handle access)
                    dynamic blockDyn = modelBlock;
                    textEntity.SoftOwner = blockDyn.Handle;

                    // Add the new text entity to the Model Space block
                    blockDyn.Entities.Add(textEntity);
                }

                // Set rasterization options for PNG output
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" }
                };

                // Configure PNG save options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the modified CAD drawing as PNG
                cad.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}