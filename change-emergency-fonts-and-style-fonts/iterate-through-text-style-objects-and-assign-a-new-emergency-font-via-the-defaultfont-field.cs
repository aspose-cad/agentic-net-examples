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
            string inputPath = "input.dwg";
            string outputPath = "output.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.PageWidth = 1600;
                rasterOptions.PageHeight = 1200;
                rasterOptions.Layouts = new string[] { "Model" }; // optional layout selection

                // Configure PNG export options
                PngOptions pngOptions = new PngOptions();
                pngOptions.VectorRasterizationOptions = rasterOptions;

                // Save the CAD drawing as PNG
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"File saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
