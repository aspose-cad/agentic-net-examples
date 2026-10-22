using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadToPdfConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input DXF path (use first argument or a placeholder)
            string inputPath = args.Length > 0 ? args[0] : "sample.dxf";

            // Verify that the source DXF file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file \"{inputPath}\" does not exist.");
                return;
            }

            // Prepare output directory
            string outputDirectory = "output";
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error: Unable to create output directory \"{outputDirectory}\". {dirEx.Message}");
                return;
            }

            // Build output PDF file path
            string outputFileName = Path.GetFileNameWithoutExtension(inputPath) + ".pdf";
            string outputPath = Path.Combine(outputDirectory, outputFileName);

            try
            {
                // Load the DXF file using Aspose.CAD
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure PDF save options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the CAD image as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Conversion successful. PDF saved to \"{outputPath}\".");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
            }
        }
    }
}