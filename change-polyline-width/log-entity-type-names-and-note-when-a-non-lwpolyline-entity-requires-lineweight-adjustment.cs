using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Input CAD file path
        string inputFilePath = "input.dwg";

        // Output directory for generated files
        string outputDir = "output";

        // Validate input file existence
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Input file not found: {inputFilePath}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image and cast to CadImage to access Entities
            CadImage cadImage = (CadImage)Image.Load(inputFilePath);

            // Iterate over all entities in the CAD drawing
            foreach (CadEntityBase entity in cadImage.Entities)
            {
                // Log the entity type name
                Console.WriteLine($"Entity Type: {entity.TypeName}");

                // If the entity is not a lightweight polyline and has a non‑zero line weight, note it
                if (!(entity is CadLwPolyline) && entity.LineWeight != 0)
                {
                    Console.WriteLine($"  -> Non‑LWPOLYLINE entity requires LineWeight adjustment. Current LineWeight: {entity.LineWeight}");
                }
            }

            // Example export to PDF (optional)
            string pdfPath = Path.Combine(outputDir, "output.pdf");
            PdfOptions pdfOptions = new PdfOptions();
            cadImage.Save(pdfPath, pdfOptions);
            Console.WriteLine($"PDF exported to: {pdfPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}