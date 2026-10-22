using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Input CAD file path (replace with actual path)
        string inputPath = "sample.dwg";

        // Output directory and file path
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "sample.pdf");

        try
        {
            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Retrieve original layout size using MinExtents and MaxExtents
                double originalWidth = 0;
                double originalHeight = 0;

                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    if (layout.MinExtents != null && layout.MaxExtents != null)
                    {
                        originalWidth = layout.MaxExtents.X - layout.MinExtents.X;
                        originalHeight = layout.MaxExtents.Y - layout.MinExtents.Y;
                        Console.WriteLine($"Original layout size: {originalWidth} x {originalHeight}");
                    }
                    else
                    {
                        Console.WriteLine("Layout extents are not defined.");
                    }
                    break; // Assuming single layout for simplicity
                }

                // Configure rasterization options for PDF export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Page size in pixels – using original size as a basis (rounded)
                    PageWidth = (int)Math.Ceiling(originalWidth),
                    PageHeight = (int)Math.Ceiling(originalHeight)
                };

                // Configure PDF save options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save as PDF
                cadImage.Save(outputPath, pdfOptions);

                // Compare exported page size with original layout size
                Console.WriteLine($"Exported PDF page size (set): {rasterOptions.PageWidth} x {rasterOptions.PageHeight}");
                if (rasterOptions.PageWidth == (int)Math.Ceiling(originalWidth) &&
                    rasterOptions.PageHeight == (int)Math.Ceiling(originalHeight))
                {
                    Console.WriteLine("Exported layout size matches the original.");
                }
                else
                {
                    Console.WriteLine("Exported layout size differs from the original.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}