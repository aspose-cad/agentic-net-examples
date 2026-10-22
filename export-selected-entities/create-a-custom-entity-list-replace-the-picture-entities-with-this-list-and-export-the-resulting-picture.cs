using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file '{inputPath}' not found.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                var customEntities = new List<CadEntityBase>();

                var line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 100, 0)
                };
                customEntities.Add(line);

                cadImage.Entities = customEntities.ToArray();

                var pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Exported to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}