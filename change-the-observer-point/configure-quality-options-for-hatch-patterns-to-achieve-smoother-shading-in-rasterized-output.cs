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
            // Prepare input and output paths
            string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string inputFile = Path.Combine(dataDirectory, "sample.dwg");
            if (!File.Exists(inputFile))
            {
                Console.WriteLine("Input CAD file not found: " + inputFile);
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputFile))
            {
                // Cast to CadImage to access CAD-specific members if needed
                CadImage cad = cadImage as CadImage;
                if (cad == null)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" }
                };

                // Set PNG export options and attach rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rasterized image
                string outputFile = Path.Combine(outputDirectory, "output.png");
                cad.Save(outputFile, pngOptions);
                Console.WriteLine("Rasterized image saved to: " + outputFile);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}