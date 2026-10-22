using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Input CAD file path
                string inputPath = "input.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PDF file path
                string outputPath = "output.pdf";

                // Hyperlink to validate
                string hyperlink = "www.example.com";

                // Validate hyperlink format (ensure protocol prefix)
                if (!hyperlink.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                    !hyperlink.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Hyperlink missing protocol. Adding http:// prefix.");
                    hyperlink = "http://" + hyperlink;
                }

                // Load CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cad = (CadImage)cadImage;

                    // Example: iterate top-level entities and output their type names
                    foreach (CadEntityBase entity in cad.Entities)
                    {
                        Console.WriteLine($"Entity type: {entity.TypeName}");
                    }

                    // Save the CAD drawing as PDF
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Saved PDF to '{outputPath}'. Hyperlink used: {hyperlink}");
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}