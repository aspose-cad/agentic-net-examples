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
            string outputOriginal = "output_original.png";
            string outputModified = "output_modified.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Render original appearance
                PngOptions originalOptions = new PngOptions();
                CadRasterizationOptions originalRaster = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new string[] { "Model" }
                };
                originalOptions.VectorRasterizationOptions = originalRaster;
                cadImage.Save(outputOriginal, originalOptions);

                // Change the emergency/default font
                string originalFont = cadImage.DefaultFont;
                cadImage.DefaultFont = "Arial";

                // Render appearance after font change
                PngOptions modifiedOptions = new PngOptions();
                CadRasterizationOptions modifiedRaster = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new string[] { "Model" }
                };
                modifiedOptions.VectorRasterizationOptions = modifiedRaster;
                cadImage.Save(outputModified, modifiedOptions);

                // Optionally restore original font
                cadImage.DefaultFont = originalFont;
            }

            Console.WriteLine("Rendering completed. Check the output images.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}