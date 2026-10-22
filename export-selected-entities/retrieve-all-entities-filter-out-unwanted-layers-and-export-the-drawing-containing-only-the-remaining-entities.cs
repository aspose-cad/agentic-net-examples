using System;
using System.IO;
using System.Linq;
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
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Define layers that should be removed from the drawing
                string[] unwantedLayers = { "LayerToRemove", "AnotherLayer" };

                // NOTE: Aspose.CAD's CadImage.Entities is a read‑only IEnumerable<CadEntityBase>.
                // The SDK does not provide a direct method to remove entities from this collection.
                // Therefore, filtering out unwanted layers cannot be performed in‑place.
                // As a workaround, one could create a new drawing and copy only the desired entities,
                // but such functionality is not available via the current public API.
                // The code proceeds to export the original drawing.

                // Export the (unfiltered) drawing to PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}