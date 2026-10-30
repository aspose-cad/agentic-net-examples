using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.dxf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
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
            // Load the CAD drawing
            Image cadImage = Image.Load(inputPath);

            // Set up rasterization options required by DxfOptions
            var rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 800,
                PageHeight = 600,
                UnitType = UnitType.Centimenter,
                LineScale = 1.0f
            };

            // Configure DxfOptions for binary DXF output
            var dxfOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Binary,
                VectorRasterizationOptions = rasterOptions
            };

            // Save the drawing as binary DXF
            cadImage.Save(outputPath, dxfOptions);
            Console.WriteLine($"DXF file saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during processing: {ex.Message}");
        }
    }
}