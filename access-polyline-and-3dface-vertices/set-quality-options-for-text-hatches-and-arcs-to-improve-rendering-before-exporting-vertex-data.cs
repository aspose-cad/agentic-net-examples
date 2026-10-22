using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder paths – replace with actual file locations
            string inputPath = "sample.dwg";
            string pdfOutputPath = "output.pdf";
            
            string outputDir = Path.GetDirectoryName(pdfOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
string pngOutputPath = "output.png";

            try
            {
                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD file
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members if needed
                    CadImage cad = cadImage as CadImage;
                    if (cad == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Export to PDF
                    PdfOptions pdfOptions = new PdfOptions();
                    cad.Save(pdfOutputPath, pdfOptions);
                    Console.WriteLine($"PDF saved to: {pdfOutputPath}");

                    // Export to PNG with rasterization options
                    PngOptions pngOptions = new PngOptions();
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Example: set desired output size
                        PageWidth = 1600,
                        PageHeight = 1200,
                        // Optional: control layers or layouts here
                        // Layouts = new string[] { "Model" },
                        // Layers = new string[] { "0" }
                    };
                    pngOptions.VectorRasterizationOptions = rasterOptions;
                    cad.Save(pngOutputPath, pngOptions);
                    Console.WriteLine($"PNG saved to: {pngOutputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
