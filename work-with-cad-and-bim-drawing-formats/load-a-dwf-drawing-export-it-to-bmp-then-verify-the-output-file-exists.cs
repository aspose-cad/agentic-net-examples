using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output paths
            string inputPath = "input.dwf";
            string outputDir = "Output";
            string outputPath = Path.Combine(outputDir, "output.bmp");

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the DWF drawing
            using (Image dwfImage = Image.Load(inputPath))
            {
                // Configure BMP export options
                BmpOptions bmpOptions = new BmpOptions();

                // Set rasterization options (required for vector formats)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600f,
                    PageHeight = 1200f,
                    Layouts = new[] { "Model" } // Use layout name directly
                };
                bmpOptions.VectorRasterizationOptions = rasterOptions;

                // Save as BMP
                dwfImage.Save(outputPath, bmpOptions);
            }

            // Verify the output file was created
            if (File.Exists(outputPath))
            {
                Console.WriteLine($"BMP file successfully created at: {outputPath}");
            }
            else
            {
                Console.Error.WriteLine("Failed to create the BMP output file.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}