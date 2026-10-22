using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "sample.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load a CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Inspect top‑level drawing entities before any operation
                Console.WriteLine("=== Entities before UpdateSize ===");
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    Console.WriteLine($"Entity Type: {entity.GetType().Name}, Layer: {entity.LayerName}");
                }

                // Select entities from a specific layer (e.g., "0")
                CadEntityBase[] selectedEntities = cadImage.Entities
                    .Where(e => string.Equals(e.LayerName, "0", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                // (Optional) Perform any manipulation on selected entities here.
                // For this example we simply keep the selection unchanged.

                // Call UpdateSize after assigning/selecting entities
                cadImage.UpdateSize();

                // Verify that layers of entities not in the selected array remain unchanged
                Console.WriteLine("\n=== Entities after UpdateSize ===");
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    Console.WriteLine($"Entity Type: {entity.GetType().Name}, Layer: {entity.LayerName}");
                }

                // Export the drawing to PDF
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600,
                        Layouts = new[] { "Model" }
                    }
                };

                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"\nPDF saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}