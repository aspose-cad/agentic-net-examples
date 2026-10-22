using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = args.Length > 0 ? args[0] : "input.dxf";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Source file not found: {inputPath}");
                return;
            }

            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            string outputPath = Path.Combine(outputDir, "modified_output.pdf");

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Modify layer assignment of all entities inside each block definition
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        entity.LayerName = "ModifiedLayer";
                    }
                }

                // Prepare PDF export options
                PdfOptions pdfOptions = new PdfOptions();
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Optional: set page size, layout, etc.
                    PageWidth = 800,
                    PageHeight = 600,
                    Layouts = new string[] { "Model" }
                };
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the modified CAD to PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"File saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}