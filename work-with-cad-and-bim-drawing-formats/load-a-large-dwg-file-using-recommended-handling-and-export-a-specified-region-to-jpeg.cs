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
            // Input DWG file path (replace with actual path)
            string inputPath = @"C:\CAD\large_drawing.dwg";
            // Output JPEG file path
            string outputPath = @"C:\CAD\region_output.jpg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Define the region to export (in drawing units)
            double regionX = 0.0;          // Lower-left X coordinate (not directly supported)
            double regionY = 0.0;          // Lower-left Y coordinate (not directly supported)
            double regionWidth = 100.0;   // Width of the region
            double regionHeight = 100.0;  // Height of the region

            // Load the DWG file using Aspose.CAD
            Image cadImage = Image.Load(inputPath);

            // NOTE: The current Aspose.CAD SDK does not provide a direct way to specify an export
            // region offset (regionX, regionY). The region can be approximated by setting the
            // PageWidth and PageHeight to the desired region size. Offsets would require additional
            // processing not available in this version of the SDK.

            var rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Centimenter,
                LineScale = 1.0f,
                PageWidth = (float)regionWidth,
                PageHeight = (float)regionHeight
                // AutomaticLayoutsScaling defaults to true; margins left at defaults
            };

            var jpegOptions = new JpegOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the specified region as JPEG (approximate)
            cadImage.Save(outputPath, jpegOptions);
            Console.WriteLine($"Region exported (approximate) to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}