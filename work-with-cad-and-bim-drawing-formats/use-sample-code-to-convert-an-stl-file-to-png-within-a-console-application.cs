using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.stl";
        string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file \"{inputPath}\" does not exist.");
            return;
        }

        try
        {
            // Ensure the output directory exists
            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir))
                Directory.CreateDirectory(outDir);

            // Load the STL file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1000,
                    PageHeight = 1000
                    // Note: Layout property is not available in the current API
                };

                // Set PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the image as PNG
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Successfully converted \"{inputPath}\" to \"{outputPath}\".");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}