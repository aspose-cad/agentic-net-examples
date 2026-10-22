using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Placeholder input and output file paths
        string inputPath = "input.dwg";
        string outputPath = "output.png";

        try
        {
            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.PageWidth = 1600;   // Desired width in pixels
                rasterOptions.PageHeight = 1200;  // Desired height in pixels
                rasterOptions.Layouts = new string[] { "Model" }; // Render the default layout

                // Configure PNG export options
                PngOptions pngOptions = new PngOptions();
                pngOptions.VectorRasterizationOptions = rasterOptions;

                // Save the rendered image
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"CAD file successfully saved as PNG to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Output concise error information
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}