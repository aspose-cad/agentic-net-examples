using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using System.IO;

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
        // Path for the resulting PDF file
                string outputPath = "output.pdf";

                // Load the CAD drawing
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Iterate through block entities to locate external references (XREFs)
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    // Heuristic: a non‑zero base point often indicates an external reference block
                    if (block.BasePoint != null &&
                        (block.BasePoint.X != 0 || block.BasePoint.Y != 0 || block.BasePoint.Z != 0))
                    {
                        Console.WriteLine("External reference block detected.");
                        int entityCount = block.Entities != null ? block.Entities.Count : 0;
                        Console.WriteLine($"Contains {entityCount} entities.");
                    }
                }

                // Save the CAD image to PDF format
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}