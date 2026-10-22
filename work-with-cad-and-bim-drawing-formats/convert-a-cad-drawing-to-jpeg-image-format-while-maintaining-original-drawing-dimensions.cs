using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.jpg";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization to keep original drawing dimensions
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.AutomaticLayoutsScaling = true;
                rasterOptions.PageWidth = cadImage.Width;
                rasterOptions.PageHeight = cadImage.Height;

                JpegOptions jpegOptions = new JpegOptions();
                jpegOptions.VectorRasterizationOptions = rasterOptions;

                cadImage.Save(outputPath, jpegOptions);
            }

            Console.WriteLine($"CAD drawing converted to JPEG: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}