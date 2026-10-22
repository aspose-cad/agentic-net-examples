using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputFilePath = "input.dwg";
        string outputFilePath = "output.png";

        if (!File.Exists(inputFilePath))
        {
            Console.Error.WriteLine($"Input file not found: {inputFilePath}");
            return;
        }

        try
        {
            using (Image loadedImage = Image.Load(inputFilePath))
            {
                CadImage cadImage = loadedImage as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.PageWidth = 1600;
                rasterOptions.PageHeight = 1200;
                // Example: render only the "Model" layout if present
                // rasterOptions.Layouts = new string[] { "Model" };

                PngOptions pngOptions = new PngOptions();
                pngOptions.VectorRasterizationOptions = rasterOptions;

                cadImage.Save(outputFilePath, pngOptions);
                Console.WriteLine($"CAD file saved as PNG to: {outputFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}