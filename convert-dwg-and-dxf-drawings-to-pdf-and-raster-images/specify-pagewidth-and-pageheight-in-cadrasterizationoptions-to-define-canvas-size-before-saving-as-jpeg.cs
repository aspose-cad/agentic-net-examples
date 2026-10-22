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
            string inputPath = "input.dwg";
            string outputPath = "output.jpg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Define rasterization options with specific canvas size
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1200,   // width in pixels
                    PageHeight = 800    // height in pixels
                };

                // Set JPEG export options
                JpegOptions jpegOptions = new JpegOptions
                {
                    VectorRasterizationOptions = rasterOptions,
                    Quality = 90
                };

                // Save the CAD image as JPEG
                cadImage.Save(outputPath, jpegOptions);
            }

            Console.WriteLine($"CAD file successfully saved as JPEG to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}