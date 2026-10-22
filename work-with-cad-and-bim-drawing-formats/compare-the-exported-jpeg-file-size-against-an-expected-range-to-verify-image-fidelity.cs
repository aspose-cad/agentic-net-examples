using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Placeholder input CAD file path
        string inputPath = "input.dwg";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Prepare output directory and file path
        string outputDirectory = "Output";
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }
        string outputPath = Path.Combine(outputDirectory, "output.jpg");

        // Expected JPEG file size range (in bytes)
        long expectedMinSize = 50000L;   // adjust as needed
        long expectedMaxSize = 200000L;  // adjust as needed

        try
        {
            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options (optional, adjust dimensions as needed)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    // You can set additional options such as background color, layouts, etc.
                };

                // Configure JPEG export options
                JpegOptions jpegOptions = new JpegOptions
                {
                    Quality = 90,
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as JPEG
                cadImage.Save(outputPath, jpegOptions);
            }

            // Verify the size of the generated JPEG file
            FileInfo jpegInfo = new FileInfo(outputPath);
            long actualSize = jpegInfo.Length;

            if (actualSize >= expectedMinSize && actualSize <= expectedMaxSize)
            {
                Console.WriteLine($"JPEG file size {actualSize} bytes is within the expected range.");
            }
            else
            {
                Console.WriteLine($"JPEG file size {actualSize} bytes is outside the expected range ({expectedMinSize}-{expectedMaxSize} bytes).");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}