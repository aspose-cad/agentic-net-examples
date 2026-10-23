using System.Linq;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Dwf;

class Program
{
    static void Main()
    {
        // Paths (replace with actual locations as needed)
        string sourcePath = "sample.dwf";
        string outputPath = "output.pdf";

        // Verify source file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the DWF drawing
            Image image = Image.Load(sourcePath);
            DwfImage dwf = image as DwfImage;
            if (dwf == null)
            {
                Console.Error.WriteLine("Failed to load DWF image.");
                return;
            }

            // Extract and log layer names
            var layerNames = dwf.Layers.GetLayersNames();
            Console.WriteLine("Layers in the drawing:");
            foreach (var name in layerNames)
            {
                Console.WriteLine("- " + name);
            }

            // Prepare PDF export options
            var rasterOptions = new CadRasterizationOptions
            {
                PageWidth = dwf.Width,
                PageHeight = dwf.Height,
                Layers = layerNames.ToArray()
            };

            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Export the drawing to PDF
            dwf.Save(outputPath, pdfOptions);
            Console.WriteLine($"Drawing exported to PDF successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}