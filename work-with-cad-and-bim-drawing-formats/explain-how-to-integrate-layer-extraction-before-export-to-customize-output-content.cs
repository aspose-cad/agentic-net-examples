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
            // Path to the source CAD file
            string inputPath = "input.dwg";

            // Verify that the source file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Path for the exported PDF
            string outputPath = Path.Combine(outputDir, "layer_filtered.pdf");

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadSpecificImage = cadImage as CadImage;
                if (cadSpecificImage == null)
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                    return;
                }

                // Configure rasterization options and select layers to export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Include only the desired layers (replace with actual layer names)
                    Layers = new string[] { "Layer1", "Layer2" },

                    // Optional: set page dimensions
                    PageWidth = 1200,
                    PageHeight = 800,

                    // Optional: specify layout(s) to use; "Model" is the default layout name
                    Layouts = new string[] { "Model" }
                };

                // Set up PDF export options and attach rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the filtered drawing to PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Export completed: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}