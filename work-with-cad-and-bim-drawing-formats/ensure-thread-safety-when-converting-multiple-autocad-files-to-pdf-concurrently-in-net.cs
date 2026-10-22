using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Define input CAD files (placeholders)
            string[] inputFiles = new string[]
            {
                "input1.dwg",
                "input2.dxf",
                "input3.dwg"
            };

            // Define output directory
            string outputDirectory = "output";

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Filter only existing files
            List<string> existingFiles = new List<string>();
            foreach (string filePath in inputFiles)
            {
                if (File.Exists(filePath))
                {
                    existingFiles.Add(filePath);
                }
                else
                {
                    Console.Error.WriteLine($"File not found: {filePath}");
                }
            }

            // Convert each CAD file to PDF concurrently
            Parallel.ForEach(existingFiles, (filePath) =>
            {
                try
                {
                    // Load CAD image
                    using (Image cadImage = Image.Load(filePath))
                    {
                        // Prepare PDF export options
                        PdfOptions pdfOptions = new PdfOptions();

                        // Configure rasterization options for vector output
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            // Optional: set page size or other rasterization settings here
                            // For example, set a default page size
                            PageWidth = 842,   // A4 width in points (approx 8.27 inches)
                            PageHeight = 595   // A4 height in points (approx 11.69 inches)
                        };

                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Determine output PDF path
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
                        string outputPath = Path.Combine(outputDirectory, fileNameWithoutExt + ".pdf");

                        // Save as PDF
                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"Converted '{filePath}' to PDF successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{filePath}': {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}