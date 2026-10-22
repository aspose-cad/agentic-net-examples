using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                // Path to the source DWF file
                string inputPath = "input.dwf";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path where the JPEG image will be saved
                string outputPath = "output.jpg";

                // Load the DWF drawing. Aspose.CAD automatically detects the format.
                using (Image dwfImage = Image.Load(inputPath))
                {
                    // Create JPEG save options. No properties are set, so default
                    // rasterization (vector to raster) and JPEG quality are used.
                    JpegOptions jpegOptions = new JpegOptions();

                    // Save the drawing as a JPEG image. The conversion process rasterizes
                    // the vector content of the DWF using default settings (e.g., 300 DPI,
                    // default background color) and encodes the raster data as JPEG.
                    dwfImage.Save(outputPath, jpegOptions);
                }

                Console.WriteLine("Conversion completed: DWF -> JPEG");
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}