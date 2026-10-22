using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Input CAD file path (replace with actual file)
        string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Output TIFF file path
        string outputPath = "output.tiff";

        try
        {
            Image cadImage = Image.Load(inputPath);

            // Set rasterization options with explicit page dimensions
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,   // width in pixels
                PageHeight = 1200,  // height in pixels
                Layouts = new[] { "Model" }
            };

            // Configure TIFF save options
            TiffOptions tiffOptions = new TiffOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the CAD image as TIFF
            cadImage.Save(outputPath, tiffOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}