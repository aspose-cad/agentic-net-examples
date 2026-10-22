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
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Remove all line entities from the drawing
                var filteredEntities = cadImage.Entities
                    .Where(e => !(e is CadLine))
                    .ToArray();

                // Assign the filtered collection back to the image
                // (Assumes CadImage.Entities has a setter that accepts an array)
                cadImage.Entities = filteredEntities;

                // Prepare PDF save options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the modified drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Successfully saved PDF to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}