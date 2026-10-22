using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            // Verify that the source CAD file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Record start time for audit logging
            DateTime startTime = DateTime.UtcNow;

            // Load the CAD file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Rasterization settings (e.g., page size)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 595f,   // A4 width in points
                    PageHeight = 842f   // A4 height in points
                    // BackgroundColor omitted to avoid System.Drawing dependency
                };
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the CAD image as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            // Record end time and calculate duration
            DateTime endTime = DateTime.UtcNow;
            TimeSpan duration = endTime - startTime;

            // Build audit log entry
            string logMessage = $"Export Audit:{Environment.NewLine}" +
                                $"Source File: {Path.GetFullPath(inputPath)}{Environment.NewLine}" +
                                $"Destination File: {Path.GetFullPath(outputPath)}{Environment.NewLine}" +
                                $"Export Format: PDF{Environment.NewLine}" +
                                $"Start Time (UTC): {startTime:O}{Environment.NewLine}" +
                                $"End Time (UTC): {endTime:O}{Environment.NewLine}" +
                                $"Duration (seconds): {duration.TotalSeconds:F2}{Environment.NewLine}" +
                                $"Output File Size (bytes): {new FileInfo(outputPath).Length}";

            // Output audit information
            Console.WriteLine(logMessage);
        }
        catch (Exception ex)
        {
            // Print concise error information without crashing
            Console.Error.WriteLine($"Error during CAD export: {ex.Message}");
        }
    }
}