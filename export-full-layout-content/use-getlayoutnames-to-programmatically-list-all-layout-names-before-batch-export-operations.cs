using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            string sourcePath = "input.dwg";
            string outputFolder = "output";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            Directory.CreateDirectory(outputFolder);

            using (Image cadImage = Image.Load(sourcePath))
            {
                var cad = (CadImage)cadImage;

                var layoutNames = new System.Collections.Generic.List<string>();
                foreach (var kvp in cad.Layouts)
                {
                    layoutNames.Add(kvp.Key);
                }

                foreach (string layoutName in layoutNames)
                {
                    var rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new[] { layoutName },
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    string outputPath = Path.Combine(outputFolder, layoutName + ".pdf");
                    cadImage.Save(outputPath, pdfOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}