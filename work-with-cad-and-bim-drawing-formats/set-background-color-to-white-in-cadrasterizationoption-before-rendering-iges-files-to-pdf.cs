using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Path to the source IGES file
        string sourcePath = "input.igs";

        // Path to the output PDF file
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
            // Load the IGES file as a CadImage
            using (Image cadImage = Image.Load(sourcePath))
            {
                // Configure rasterization options with a white background
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Set the background color to white
                    BackgroundColor = Color.White,

                    // Set page dimensions based on the loaded image size
                    PageWidth = cadImage.Width,
                    PageHeight = cadImage.Height,

                    // Preserve object colors when rendering
                    DrawType = CadDrawTypeMode.UseObjectColor
                };

                // Create PDF options and assign the rasterization options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD drawing to PDF using the configured options
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}