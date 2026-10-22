using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string cadPath = "sample.dwg";
        string pngPath = "output.png";

        if (!File.Exists(cadPath))
        {
            Console.WriteLine($"CAD file not found: {cadPath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(cadPath))
            {
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800 // Height is omitted; it will be calculated automatically
                    // Unit can be set if needed, e.g., Unit = LengthUnit.Pixel
                };

                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                try
                {
                    cadImage.Save(pngPath, pngOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving PNG: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading CAD file: {ex.Message}");
            return;
        }

        if (!File.Exists(pngPath))
        {
            Console.WriteLine($"Exported PNG not found: {pngPath}");
            return;
        }

        try
        {
            using (Image exportedImage = Image.Load(pngPath))
            {
                int exportedWidth = exportedImage.Width;
                int exportedHeight = exportedImage.Height;

                Console.WriteLine($"Exported image dimensions: Width = {exportedWidth}, Height = {exportedHeight}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading exported PNG: {ex.Message}");
        }
    }
}
