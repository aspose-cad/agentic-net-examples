using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.Primitives;

class RenderIgesToPdf
{
    static void Main()
    {
        try
        {
            // Path to the source IGES file
            string sourcePath = "input.iges";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine("Source IGES file not found: " + sourcePath);
                return;
            }

            // Path for the resulting PDF file
            string outputPath = "output.pdf";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the IGES file
            Image cadImage = Image.Load(sourcePath);

            // Configure rasterization options: white background and anti‑aliasing enabled
            var rasterizationOptions = new CadRasterizationOptions
            {
                // Set the background color to white
                BackgroundColor = Aspose.CAD.Color.White,

                // Set page size based on the CAD image dimensions
                PageWidth = (float)cadImage.Width,
                PageHeight = (float)cadImage.Height,

                // Optional: set unit type and line scale if needed
                UnitType = UnitType.Centimenter,
                LineScale = 0.5f,

                // Enable anti‑aliasing via GraphicsOptions
                GraphicsOptions = new GraphicsOptions
                {
                    SmoothingMode = Aspose.CAD.SmoothingMode.AntiAlias,
                    TextRenderingHint = Aspose.CAD.TextRenderingHint.AntiAliasGridFit
                },

                // High quality rasterization
                Quality = new RasterizationQuality
                {
                    Text = RasterizationQualityValue.High,
                    Hatch = RasterizationQualityValue.High,
                    Arc = RasterizationQualityValue.High
                }
            };

            // Create PDF save options and attach the rasterization settings
            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterizationOptions
            };

            // Save the rendered image as a PDF file
            cadImage.Save(outputPath, pdfOptions);

            Console.WriteLine("IGES file has been rendered to PDF with anti‑aliasing and a white background.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}