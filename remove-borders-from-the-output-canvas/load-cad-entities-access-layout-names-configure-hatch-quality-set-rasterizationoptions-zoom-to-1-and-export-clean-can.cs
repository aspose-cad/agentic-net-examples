using System;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Input CAD file path
                string inputPath = "input.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PNG file path
                string outputPath = "output.png";

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Display available layout names
                    Console.WriteLine("Available layouts:");
                    foreach (string layoutName in cadImage.Layouts.Keys)
                    {
                        Console.WriteLine(layoutName);
                    }

                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.Zoom = 1f; // Set zoom to 1 (original size)

                    // Create PNG export options and assign rasterization settings
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the CAD drawing to a clean PNG canvas
                    cadImage.Save(outputPath, pngOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}