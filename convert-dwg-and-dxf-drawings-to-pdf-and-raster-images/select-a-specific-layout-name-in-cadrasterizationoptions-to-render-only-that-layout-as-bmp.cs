using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.bmp";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure BMP save options with rasterization settings
                BmpOptions bmpOptions = new BmpOptions();

                // Set up rasterization options to render a specific layout
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Specify the layout name to render
                    Layouts = new string[] { "Layout1" }
                };

                // Assign rasterization options to the BMP options
                bmpOptions.VectorRasterizationOptions = rasterOptions;

                // Save the rendered layout as BMP
                cadImage.Save(outputPath, bmpOptions);
                Console.WriteLine($"Layout rendered and saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}