using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Select only line entities
                List<CadEntityBase> selectedEntities = new List<CadEntityBase>();
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine)
                    {
                        selectedEntities.Add(entity);
                    }
                }

                // Replace the image's entities with the selected ones (if any)
                if (selectedEntities.Count > 0)
                {
                    cadImage.Entities = selectedEntities.ToArray();
                }

                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600
                    }
                };

                // Export to PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Export completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}