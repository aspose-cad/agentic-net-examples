using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.png";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD file using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization options (e.g., select a layout)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Specify the layout(s) to render; adjust as needed
                        Layouts = new string[] { "Model" },

                        // Optional: set the desired output size in pixels
                        // Width = 1600,
                        // Height = 1200,

                        // Optional: set background color, etc.
                        // BackgroundColor = System.Drawing.Color.White
                    };

                    // Set up PNG export options and attach rasterization settings
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the rendered layout to a PNG file
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"CAD file successfully exported to PNG: {outputPath}");
            }
            catch (Exception ex)
            {
                // Print concise error information without crashing
                Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}