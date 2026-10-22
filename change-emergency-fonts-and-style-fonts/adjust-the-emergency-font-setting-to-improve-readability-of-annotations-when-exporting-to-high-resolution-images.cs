using Aspose.CAD;
using System;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Load the CAD file
                string inputPath = "input.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        CadImage cadImage = (CadImage)Aspose.CAD.Image.Load(inputPath);

                // Set the emergency (fallback) font to improve annotation readability
                cadImage.DefaultFont = "Arial";

                // Configure rasterization options for high‑resolution export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Define page size to achieve a high DPI (e.g., 300 DPI)
                    // Assuming drawing units are inches, set width/height accordingly
                    PageWidth = 8.5f * 300f,   // 8.5 inches * 300 DPI
                    PageHeight = 11f * 300f,  // 11 inches * 300 DPI
                    LineScale = 1.0f,
                    AutomaticLayoutsScaling = true
                };

                // Set PNG export options using the rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the high‑resolution PNG image
                string outputPath = "output.png";
                cadImage.Save(outputPath, pngOptions);
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}