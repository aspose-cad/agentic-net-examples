using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Select layers: make only "Layer1" visible, hide others
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    if (layer.Name.Equals("Layer1", StringComparison.OrdinalIgnoreCase))
                    {
                        layer.PlotFlag = true; // visible
                    }
                    else
                    {
                        layer.PlotFlag = false; // hidden
                    }
                }

                // Rasterization options with desired size
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" }
                };

                // Export to PDF (vector output)
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                string pdfOutputPath = "output.pdf";
                EnsureDirectory(pdfOutputPath);
                cadImage.Save(pdfOutputPath, pdfOptions);

                // Export to PNG (raster image)
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                string pngOutputPath = "output.png";
                EnsureDirectory(pngOutputPath);
                cadImage.Save(pngOutputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    static void EnsureDirectory(string filePath)
    {
        string dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }
}