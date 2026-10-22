using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadHyperlinkExample
{
    class Program
    {
        static void Main(string[] args)
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
                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate over top‑level entities and set a hyperlink on a specific entity
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Example condition: modify entities on layer "0"
                        if (entity.LayerName == "0")
                        {
                            // Set the hyperlink (replace with desired URL)
                            entity.Hyperlink = "https://example.com";
                        }
                    }

                    // Save the modified drawing as PDF
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPath, pdfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}