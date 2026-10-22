using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            string inputFile = "sample.dwg";
            string outputFile = "output.pdf";

            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"Input file not found: {inputFile}");
                return;
            }

            using (Image cadImage = Image.Load(inputFile))
            {
                CadImage cad = cadImage as CadImage;
                if (cad == null)
                {
                    Console.Error.WriteLine("Failed to load CAD image.");
                    return;
                }

                var layerNames = new HashSet<string>();
                foreach (var entity in cad.Entities)
                {
                    if (!string.IsNullOrEmpty(entity.LayerName))
                        layerNames.Add(entity.LayerName);
                }
                int layerCount = layerNames.Count;
                Console.WriteLine($"Number of layers in CAD file: {layerCount}");

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "0" } // Export the first layout (index 0) as string
                };

                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputFile, pdfOptions);
                Console.WriteLine($"PDF saved to: {outputFile}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}