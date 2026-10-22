using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadPenOptionsDemo
{
    class Program
    {
        static void Main(string[] args)
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

                // Path for the rendered output image
                string outputPath = "rendered_output.png";

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Inspect block entities and log their types
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block: {block.Name}");
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            Console.WriteLine($"  Entity Type: {entity.TypeName}");
                        }
                    }

                    // Configure rasterization options (PenOptions omitted due to unavailable property)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    // Prepare PNG save options with the rasterization settings
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Render and save the CAD drawing to PNG
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine("Rendering completed. Output saved to " + outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}