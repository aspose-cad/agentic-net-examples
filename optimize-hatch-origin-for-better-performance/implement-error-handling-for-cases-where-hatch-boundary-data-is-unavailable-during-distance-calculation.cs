using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input CAD file path (replace with actual file path)
                string inputPath = "input.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Create output directory
                string outputDir = "Output";
                Directory.CreateDirectory(outputDir);

                // Load the CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cad = cadImage as CadImage;
                    if (cad == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Example: Inspect block definitions
                    foreach (CadBlockEntity block in cad.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block Name: {block.Name}, Entity Count: {block.Entities.Count}");
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            Console.WriteLine($"  Entity Type: {entity.GetType().Name}");
                        }
                    }

                    // Save as PNG
                    string pngPath = Path.Combine(outputDir, "output.png");
                    PngOptions pngOptions = new PngOptions();
                    cadImage.Save(pngPath, pngOptions);
                    Console.WriteLine($"PNG file saved to: {pngPath}");

                    // Save as PDF
                    string pdfPath = Path.Combine(outputDir, "output.pdf");
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(pdfPath, pdfOptions);
                    Console.WriteLine($"PDF file saved to: {pdfPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}