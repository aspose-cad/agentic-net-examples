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
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Collect desired entities (e.g., lines and lightweight polylines)
                List<CadEntityBase> desiredEntities = new List<CadEntityBase>();

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine || entity is CadLwPolyline)
                    {
                        desiredEntities.Add(entity);
                    }
                }

                // Replace the drawing's entities with only the desired ones
                cadImage.Entities = desiredEntities.ToArray();

                // Save the modified drawing to PDF
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