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
            // Input CAD file (DWG, DXF, etc.)
            string inputPath = "input.dwg";
            // Output DXF file
            string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options to preserve layout settings
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                // Preserve specific layouts; adjust names as needed for the source file
                rasterOptions.Layouts = new string[] { "Model", "Layout1" };

                // Set up DXF export options
                DxfOptions dxfOptions = new DxfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export to DXF
                cadImage.Save(outputPath, dxfOptions);
            }

            Console.WriteLine($"CAD drawing exported to DXF: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}