using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadSatSabToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input and output directories (adjust as needed)
            string inputDirectory = "Input";
            string outputDirectory = "Output";

            try
            {
                // Verify that input directory exists
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

                // Gather all .sat and .sab files
                string[] satFiles = Directory.GetFiles(inputDirectory, "*.sat", SearchOption.TopDirectoryOnly);
                string[] sabFiles = Directory.GetFiles(inputDirectory, "*.sab", SearchOption.TopDirectoryOnly);

                // Combine both arrays
                string[] allFiles = new string[satFiles.Length + sabFiles.Length];
                satFiles.CopyTo(allFiles, 0);
                sabFiles.CopyTo(allFiles, satFiles.Length);

                // Process each file sequentially
                foreach (string sourceFilePath in allFiles)
                {
                    try
                    {
                        // Verify that the source file exists
                        if (!File.Exists(sourceFilePath))
                        {
                            Console.Error.WriteLine($"File not found: {sourceFilePath}");
                            continue;
                        }

                        // Load the CAD drawing
                        using (Image cadImage = Image.Load(sourceFilePath))
                        {
                            // Prepare PDF export options
                            PdfOptions pdfOptions = new PdfOptions();

                            // Determine output PDF path
                            string outputFileName = Path.GetFileNameWithoutExtension(sourceFilePath) + ".pdf";
                            string outputFilePath = Path.Combine(outputDirectory, outputFileName);

                            // Save as PDF
                            cadImage.Save(outputFilePath, pdfOptions);

                            Console.WriteLine($"Successfully exported '{sourceFilePath}' to PDF: '{outputFilePath}'");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing file '{sourceFilePath}': {ex.Message}");
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