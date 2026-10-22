using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Define which entity types should be retained
                var allowedEntityTypes = new HashSet<string>
                {
                    "Line",
                    "Circle"
                };

                // Keep only the allowed entities
                var filteredEntities = cadImage.Entities
                    .Where(e => allowedEntityTypes.Contains(e.TypeName.ToString()))
                    .ToArray();

                // Replace the original entities collection with the filtered set
                cadImage.Entities = filteredEntities;

                // Export the modified drawing to PDF
                var pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Export completed successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}