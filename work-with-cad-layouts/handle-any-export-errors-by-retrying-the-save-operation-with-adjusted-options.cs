using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwg";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded file is not a supported CAD format.");
                    return;
                }

                // Configure rasterization options (e.g., select a layout and layer)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = new string[] { "Model" },
                    Layers = new string[] { "0" },
                    PageWidth = 1600,
                    PageHeight = 1200
                    // BackgroundColor omitted to avoid System.Drawing usage
                };

                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                bool saved = false;
                int attempts = 0;
                while (!saved && attempts < 2)
                {
                    try
                    {
                        cadImage.Save(outputPath, pdfOptions);
                        saved = true;
                        Console.WriteLine($"File saved successfully to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        attempts++;
                        if (attempts == 1)
                        {
                            // Adjust options and retry
                            rasterOptions.ContentAsBitmap = true;
                            Console.Error.WriteLine($"Save failed ({ex.Message}). Retrying with bitmap rasterization.");
                        }
                        else
                        {
                            Console.Error.WriteLine($"Failed to save after retry: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}