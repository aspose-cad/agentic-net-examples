using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadLoadSaveExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input CAD file path (DWG, DXF, etc.)
                    string inputPath = @"C:\Input\sample.dwg";

            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PDF file path
                    string outputPath = @"C:\Output\sample.pdf";

                    // Ensure the output directory exists
                    string? outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Load the CAD image
                    using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                    {
                        // Create PDF save options
                        PdfOptions pdfOptions = new PdfOptions();

                        // Save the CAD image as PDF
                        cadImage.Save(outputPath, pdfOptions);
                    }

                    Console.WriteLine("CAD file has been successfully converted to PDF.");
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}