using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string sourcePath = "input.dwg";
        string outputDirectory = "output";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        Directory.CreateDirectory(outputDirectory);

        try
        {
            using (Image cadImage = Image.Load(sourcePath))
            {
                CadImage cadImg = (CadImage)cadImage;

                ICollection<string> layoutNames = cadImg.Layouts.Keys.ToList();

                foreach (string layoutName in layoutNames)
                {
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new string[] { layoutName },
                        PageWidth = 1600f,
                        PageHeight = 1200f
                        // BackgroundColor omitted to avoid System.Drawing dependency
                    };

                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    string outputPath = Path.Combine(outputDirectory, layoutName + ".pdf");

                    cadImg.Save(outputPath, pdfOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}