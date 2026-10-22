using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace BatchDwfToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output directories (replace with actual paths as needed)
                string inputDirectory = @"C:\InputDwfFiles";
                string outputDirectory = @"C:\OutputPdfFiles";

                // Verify that the input directory exists
                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                    return;
                }

                // Ensure the output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Get all DWF files in the input directory
                string[] dwfFiles = Directory.GetFiles(inputDirectory, "*.dwf", SearchOption.TopDirectoryOnly);
                if (dwfFiles.Length == 0)
                {
                    Console.WriteLine("No DWF files found to convert.");
                    return;
                }

                // Prepare PDF save options (default settings)
                PdfOptions pdfOptions = new PdfOptions();

                foreach (string dwfFilePath in dwfFiles)
                {
                    // Load the DWF file
                    using (Image cadImage = Image.Load(dwfFilePath))
                    {
                        // Determine output PDF file path
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(dwfFilePath);
                        string pdfFilePath = Path.Combine(outputDirectory, fileNameWithoutExt + ".pdf");

                        // Save as PDF
                        cadImage.Save(pdfFilePath, pdfOptions);
                        Console.WriteLine($"Converted: {dwfFilePath} -> {pdfFilePath}");
                    }
                }

                Console.WriteLine("Batch conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}