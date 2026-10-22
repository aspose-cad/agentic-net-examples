using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

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
                // Load the CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.UnitType = UnitType.Centimenter; // SDK spelling
                    rasterOptions.LineScale = 1.0f;
                    rasterOptions.Layouts = new string[] { "Model" }; // Specify layout name
                    rasterOptions.Margins = new Margins
                    {
                        Left = 10,
                        Right = 10,
                        Top = 10,
                        Bottom = 10
                    };

                    // Set up PNG export options
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the image to PNG format
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"CAD file successfully converted and saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}