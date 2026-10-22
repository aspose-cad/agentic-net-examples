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
                // Define input CAD file path
                string inputPath = "input.dwg";

                // Define output directory and file path
                string outputDirectory = "output";
                string outputPath = Path.Combine(outputDirectory, "output.png");

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure the output directory exists
                Directory.CreateDirectory(outputDirectory);

                // Load the CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage if further CAD-specific processing is required
                    CadImage cad = cadImage as CadImage;

                    // Configure rasterization options with zero margins to remove borders
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.UnitType = UnitType.Centimenter;
                    rasterOptions.Zoom = 1;
                    rasterOptions.LineScale = 1;
                    rasterOptions.Margins = new Margins()
                    {
                        Left = 0,
                        Right = 0,
                        Top = 0,
                        Bottom = 0
                    };

                    // Set PNG save options and assign the rasterization settings
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the image without borders
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"Image saved successfully to: {outputPath}");
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}