using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string sourcePath = "sample.dwg";

        if (!File.Exists(sourcePath))
        {
            Console.WriteLine($"Source file '{sourcePath}' does not exist.");
            return;
        }

        try
        {
            using (Image image = Image.Load(sourcePath))
            {
                CadImage cadImage = (CadImage)image;

                // Save with automatic sizing (Width and Height omitted)
                string autoSizePath = "output_auto.png";
                var autoSizeOptions = new PngOptions();
                try
                {
                    cadImage.Save(autoSizePath, autoSizeOptions);
                    Console.WriteLine($"Automatic size image saved to '{autoSizePath}'.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to save automatic size image: {ex.Message}");
                }

                // Save with explicit dimensions (Width and Height set)
                string explicitSizePath = "output_fixed.png";
                var fixedSizeOptions = new PngOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600
                    }
                };
                try
                {
                    cadImage.Save(explicitSizePath, fixedSizeOptions);
                    Console.WriteLine($"Fixed size image saved to '{explicitSizePath}'.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to save fixed size image: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading CAD file: {ex.Message}");
        }
    }
}
