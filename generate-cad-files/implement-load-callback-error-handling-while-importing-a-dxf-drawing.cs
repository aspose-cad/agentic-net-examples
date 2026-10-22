using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System.IO;

class Program
{
    static void Main()
    {
        string inputPath = "input.dxf";

        
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }
try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("The loaded file is not a CAD image.");
                    return;
                }

                // Example: count line entities in the drawing
                int lineCount = 0;
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine)
                    {
                        lineCount++;
                    }
                }
                Console.WriteLine($"Number of line entities: {lineCount}");

                // Save the drawing to PDF as a demonstration of successful load
                string outputPath = "output.pdf";
                PdfOptions pdfOptions = new PdfOptions();
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1600,
                    Layouts = new[] { "Model" }
                };
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"CAD drawing saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading DXF file: {ex.Message}");
        }
    }
}