using System;
using System.IO;
using System.Diagnostics;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadBatchConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder input and output directories
                string inputDirectory = @"C:\CadInput";
                string outputDirectory = @"C:\CadOutput";

                // Verify directories exist
                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                    return;
                }

                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Get all CF2 files in the input directory
                string[] cf2Files = Directory.GetFiles(inputDirectory, "*.cf2", SearchOption.TopDirectoryOnly);
                if (cf2Files.Length == 0)
                {
                    Console.WriteLine("No CF2 files found for conversion.");
                    return;
                }

                // Prepare PDF save options
                PdfOptions pdfOptions = new PdfOptions();

                // Process each file and measure memory usage
                Process currentProcess = Process.GetCurrentProcess();

                foreach (string cf2FilePath in cf2Files)
                {
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(cf2FilePath);
                    string pdfOutputPath = Path.Combine(outputDirectory, fileNameWithoutExt + ".pdf");

                    // Memory before loading
                    long memoryBeforeLoad = currentProcess.PrivateMemorySize64;

                    using (Image cadImage = Image.Load(cf2FilePath))
                    {
                        // Memory after loading
                        long memoryAfterLoad = currentProcess.PrivateMemorySize64;

                        // Save as PDF
                        cadImage.Save(pdfOutputPath, pdfOptions);

                        // Memory after saving
                        long memoryAfterSave = currentProcess.PrivateMemorySize64;

                        Console.WriteLine($"Converted: {Path.GetFileName(cf2FilePath)} -> {Path.GetFileName(pdfOutputPath)}");
                        Console.WriteLine($"Memory (bytes) - Before Load: {memoryBeforeLoad}, After Load: {memoryAfterLoad}, After Save: {memoryAfterSave}");
                    }

                    // Force garbage collection to get more accurate next measurement
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                Console.WriteLine("Batch conversion completed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}