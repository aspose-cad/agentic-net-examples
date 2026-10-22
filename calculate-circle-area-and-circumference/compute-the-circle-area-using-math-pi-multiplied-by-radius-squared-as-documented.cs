using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Placeholder paths – replace with actual file locations
        string inputPath = "input.dwg";
        string outputPath = "output.png";

        try
        {
            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file using Aspose.CAD
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific features
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to load CAD image as CadImage.");
                    return;
                }

                // Configure rasterization options (size, layout, etc.)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.PageWidth = 1600;   // Desired width in pixels
                rasterOptions.PageHeight = 1200;  // Desired height in pixels
                rasterOptions.Layouts = new string[] { "Model" }; // Optional layout name

                // Set PNG export options and attach rasterization settings
                PngOptions pngOptions = new PngOptions();
                pngOptions.VectorRasterizationOptions = rasterOptions;

                // Save the rendered image to the specified output path
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"File successfully saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Print concise error information without crashing
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
