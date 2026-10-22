using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExportAllLayoutContent
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            try
            {
                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Configure rasterization options for CAD export
                CadRasterizationOptions cadRasterizationOptions = new CadRasterizationOptions();
                // Enable exporting of all layout content regardless of plot area boundaries
                cadRasterizationOptions.ExportAllLayoutContent = true;

                // Create PDF save options and assign the rasterization settings
                PdfOptions pdfOptions = new PdfOptions();
                pdfOptions.VectorRasterizationOptions = cadRasterizationOptions;

                // Load the CAD file and save it using the configured options
                using (Image cadImage = Image.Load(inputPath))
                {
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"File successfully saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}