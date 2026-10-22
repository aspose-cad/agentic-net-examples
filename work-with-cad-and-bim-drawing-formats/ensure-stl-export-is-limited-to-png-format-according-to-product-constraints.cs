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
            // Define input and output file paths
            string inputPath = "sample.dwg";
            string pdfOutputPath = "output.pdf";
            
            string outputDir = Path.GetDirectoryName(pdfOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
string pngOutputPath = "output.png";

            // Verify that the input CAD file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD image using Aspose.CAD.Image.Load
            using (Image cadImage = Image.Load(inputPath))
            {
                // -------------------- Export to PDF --------------------
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfOutputPath, pdfOptions);

                // -------------------- Export to PNG --------------------
                // Configure rasterization options for vector to raster conversion
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    // Example layout selection; adjust as needed
                    Layouts = new string[] { "Model" }
                };

                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(pngOutputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            // Print concise error information without crashing
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}