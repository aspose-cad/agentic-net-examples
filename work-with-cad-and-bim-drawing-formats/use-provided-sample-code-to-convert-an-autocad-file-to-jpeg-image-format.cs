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
            // Define input and output file paths
            string inputPath = "sample.dwg";
            string outputPath = "output.jpg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set JPEG export options
                JpegOptions jpegOptions = new JpegOptions();

                // Optional: control rasterization (page size, layout, etc.)
                // jpegOptions.VectorRasterizationOptions = new CadRasterizationOptions
                // {
                //     PageWidth = 1600,
                //     PageHeight = 1200,
                //     Layouts = new[] { "Model" }
                // };

                // Save the CAD image as JPEG
                cadImage.Save(outputPath, jpegOptions);
            }

            Console.WriteLine($"Conversion completed. JPEG saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}