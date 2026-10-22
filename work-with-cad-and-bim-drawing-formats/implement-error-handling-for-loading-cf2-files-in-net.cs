using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadCf2LoadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the CF2 file (replace with actual path)
            string inputPath = "sample.cf2";

            // Verify that the file exists before attempting to load
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: The file \"{inputPath}\" does not exist.");
                return;
            }

            // Prepare output directory
            string outputDir = Path.GetDirectoryName("output.pdf");
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the CF2 file using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Output basic information about the loaded image
                    Console.WriteLine($"Successfully loaded CF2 file: {inputPath}");
                    Console.WriteLine($"Image size: {cadImage.Width} x {cadImage.Height}");

                    // Optional: Export the loaded CAD to PDF to verify successful load
                    string outputPath = "output.pdf";
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = cadImage.Width,
                            PageHeight = cadImage.Height,
                            Layouts = new string[] { } // Export all layouts
                        }
                    };

                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"Exported PDF saved to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during loading or saving
                Console.Error.WriteLine($"An error occurred while processing the CF2 file: {ex.Message}");
            }
        }
    }
}