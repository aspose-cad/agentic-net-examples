using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD files of various formats
            string[] inputFiles = new string[]
            {
                "sample1.dwg",
                "sample2.dxf",
                "sample3.dgn",
                "sample4.stl",
                "sample5.obj"
            };

            // Output directory for PDFs
            string outputDir = "output";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // PDF save options (common for all conversions)
            PdfOptions pdfOptions = new PdfOptions();

            foreach (string inputPath in inputFiles)
            {
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    continue;
                }

                // Load the CAD file
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Determine output PDF path
                    string outputFileName = Path.GetFileNameWithoutExtension(inputPath) + ".pdf";
                    string outputPath = Path.Combine(outputDir, outputFileName);

                    // Save as PDF
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"Converted '{inputPath}' to PDF: '{outputPath}'");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}