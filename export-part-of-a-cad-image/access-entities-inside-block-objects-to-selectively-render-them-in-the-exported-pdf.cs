using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to work with the CAD object model
                CadImage cadImage = (CadImage)image;

                // Iterate through block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    // Example: process a block named "MyBlock"
                    if (block.Name == "MyBlock")
                    {
                        // Access entities inside the block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            // Output the type of each entity
                            Console.WriteLine($"Entity type in block '{block.Name}': {entity.GetType().Name}");
                            // Additional logic could filter or modify entities before rendering
                        }
                    }
                }

                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600
                    // Additional rasterization settings can be set here
                };
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the CAD drawing to PDF
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}