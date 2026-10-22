using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Prepare input and output paths
        string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
        try
        {
            Directory.CreateDirectory(outputDir);
        }
        catch (Exception dirEx)
        {
            Console.WriteLine("Failed to create output directory: " + dirEx.Message);
            return;
        }

        string inputPath = Path.Combine(dataDir, "sample.dwg"); // replace with your CAD file
        string outputPath = Path.Combine(outputDir, "result.pdf");

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Set rasterization options: select layout and define page size (as a substitute for margins)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = new[] { "Layout1" } // name of the layout to export
                };

                // Example of setting a larger page size to simulate margins (optional)
                // rasterOptions.PageSize = new SizeF(850, 1100); // adjust as needed

                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the result as PDF
                try
                {
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine("PDF exported successfully to: " + outputPath);
                }
                catch (Exception saveEx)
                {
                    Console.WriteLine("Error saving PDF: " + saveEx.Message);
                }
            }
        }
        catch (Exception loadEx)
        {
            Console.WriteLine("Error loading CAD file: " + loadEx.Message);
        }
    }
}
