using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace BatchConvertDWFxToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output directories (replace with actual paths as needed)
                string inputDirectory = @"C:\InputDWFx";
                string outputDirectory = @"C:\OutputPdf";

                // Verify input directory exists
                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Get all DWFx files in the input directory
                string[] dwfxFiles = Directory.GetFiles(inputDirectory, "*.dwfx", SearchOption.TopDirectoryOnly);
                if (dwfxFiles.Length == 0)
                {
                    Console.WriteLine("No DWFx files found to convert.");
                    return;
                }

                // Prepare PDF save options (default settings)
                PdfOptions pdfOptions = new PdfOptions();

                foreach (string dwfxFilePath in dwfxFiles)
                {
                    try
                    {
                        // Load the DWFx file
                        using (Image dwfxImage = Image.Load(dwfxFilePath))
                        {
                            // Build output PDF file path
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(dwfxFilePath);
                            string pdfFilePath = Path.Combine(outputDirectory, fileNameWithoutExtension + ".pdf");

                            // Save as PDF using default options
                            dwfxImage.Save(pdfFilePath, pdfOptions);

                            Console.WriteLine($"Converted: {dwfxFilePath} -> {pdfFilePath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error converting file '{dwfxFilePath}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}