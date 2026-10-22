using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file (replace with actual path)
            string sourcePath = "input.dwg";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            // Load the CAD file
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = (CadImage)image;

                // Iterate through each layout in the CAD file
                foreach (var layoutEntry in cadImage.Layouts)
                {
                    CadLayout layout = layoutEntry.Value;
                    string layoutName = layout.LayoutName ?? "Unnamed";

                    // Sanitize layout name for file system usage
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        layoutName = layoutName.Replace(c, '_');
                    }

                    // Configure rasterization options for the current layout
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new[] { layoutName },
                        PageWidth = 800,
                        PageHeight = 600
                    };

                    // Set up PDF save options using the rasterization options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Create a dynamic file name based on the layout name
                    string outputFilePath = Path.Combine(outputDir, $"output_{layoutName}.pdf");

                    // Save the current layout as a PDF file
                    cadImage.Save(outputFilePath, pdfOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}