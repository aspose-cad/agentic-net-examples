using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadExportWithErrorCallback
{
    // Custom error handler that logs export failures to a file
    public class FileLoggingErrorHandler
    {
        private readonly string _logFilePath;

        public FileLoggingErrorHandler(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void OnError(string message, Exception exception)
        {
            string logEntry = $"{DateTime.Now:u} - Error: {message} - Exception: {exception}";
            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder input CAD file path
            string inputFilePath = "input.dwg";

            // Verify input file exists
            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            // Prepare output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);

            // Define output PDF file path
            string outputPdfPath = Path.Combine(outputDir, "exported.pdf");

            // Define log file path for error callback
            string logFilePath = Path.Combine(outputDir, "export_errors.log");
            var errorHandler = new FileLoggingErrorHandler(logFilePath);

            try
            {
                // Load the CAD image
                Image cadImage = Image.Load(inputFilePath);

                // Configure PDF export options
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions()
                };

                // Save the CAD drawing as PDF with error handling
                try
                {
                    cadImage.Save(outputPdfPath, pdfOptions);
                    Console.WriteLine("Export completed successfully.");
                }
                catch (Exception ex)
                {
                    errorHandler.OnError("Export failed", ex);
                    Console.Error.WriteLine($"Export failed: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                errorHandler.OnError("Unexpected error during processing", ex);
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}