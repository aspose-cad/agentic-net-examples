using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Define input PLT file and output PNG file paths
            string inputPath = "sample.plt";
            string outputPath = "output.png";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the PLT drawing using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure PNG export options
                PngOptions pngOptions = new PngOptions();

                // Optional: control rasterization (page size, layout, etc.)
                // pngOptions.VectorRasterizationOptions = new CadRasterizationOptions
                // {
                //     PageWidth = 1600,
                //     PageHeight = 1600,
                //     Layouts = new[] { "Model" }
                // };

                // Save the drawing as PNG
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Conversion completed successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            // Print any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}