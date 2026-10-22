using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Input DWG/DXF file path
            string inputPath = "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Output PDF file path
            string outputPath = "output.pdf";

            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Inspect block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}, Entities count: {block.Entities.Count}");
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        Console.WriteLine($"  Entity Type: {entity.TypeName}");
                    }
                }

                // Inspect top‑level drawing entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    Console.WriteLine($"Top‑level Entity Type: {entity.TypeName}");
                    if (entity is CadInsertObject insert)
                    {
                        Console.WriteLine($"  Inserted Block Name: {insert.Name}");
                        Console.WriteLine($"  ScaleX: {insert.ScaleX}, ScaleY: {insert.ScaleY}, ScaleZ: {insert.ScaleZ}");
                    }
                }

                // Configure rasterization options for PDF export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600f,
                    PageHeight = 1200f,
                    Layouts = new[] { "Model" }
                };

                // Validate and set PenOptions (if needed)
                float lineWidth = 1.0f; // example line width
                if (lineWidth < 0)
                {
                    throw new ArgumentException("Pen line width cannot be negative.");
                }

                // Create PenOptions instance (no width property in this SDK version)
                PenOptions penOptions = new PenOptions();
                rasterOptions.PenOptions = penOptions;

                // Prepare PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine("Export completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}