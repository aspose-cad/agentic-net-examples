using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

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
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Extract and log layer names
                Console.WriteLine("Layers in the drawing:");
                foreach (CadLayerTable layerObj in cadImage.Layers)
                {
                    if (layerObj is CadLayerTable layer)
                    {
                        Console.WriteLine("- " + layer.Name);
                    }
                }

                // Export the drawing to PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Drawing exported to PDF successfully: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}