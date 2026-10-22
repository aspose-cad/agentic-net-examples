using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Define input and output file paths
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

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Prepare PNG export options
                PngOptions pngOptions = new PngOptions();

                // Optional: configure rasterization options (e.g., layout, layers, resolution)
                // pngOptions.VectorRasterizationOptions = new CadRasterizationOptions
                // {
                //     Layouts = new string[] { "Model" },
                //     PageWidth = 1600,
                //     PageHeight = 1200,
                //     DrawType = Aspose.CAD.ImageOptions.CadDrawTypeMode.UseObjectColor
                // };

                // Save the drawing as PNG
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"File successfully saved to {outputPath}");
        }
        catch (Exception ex)
        {
            // Print concise error information
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}