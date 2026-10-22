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
        const string inputPath = "input.dwg";   // placeholder path
        const string outputPath = "output.pdf"; // placeholder path

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cad = (CadImage)cadImage;

                // Inspect top‑level drawing entities
                foreach (CadEntityBase entity in cad.Entities)
                {
                    Console.WriteLine($"Entity Type: {entity.GetType().Name}, Layer: {entity.LayerName}");
                }

                // Inspect block definitions
                foreach (CadBlockEntity block in cad.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block Name: {block.Name}, Entity Count: {block.Entities.Count}");
                    foreach (CadEntityBase blockEntity in block.Entities)
                    {
                        Console.WriteLine($"  Block Entity Type: {blockEntity.GetType().Name}");
                    }
                }

                // Configure rasterization options to render only the visible portion
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    NoScaling = true,
                    PageWidth = 1600,
                    PageHeight = 1200
                };

                // Set PDF save options with the rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}