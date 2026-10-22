using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Input DWG file path
                string inputPath = "sample.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output JPEG file path
                string outputPath = "output.jpg";

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific properties
                    CadImage cadImage = image as CadImage;
                    if (cadImage != null)
                    {
                        // Substitute missing fonts with a known font (e.g., Arial)
                        cadImage.DefaultFont = "Arial";

                        // Set JPEG export options
                        JpegOptions jpegOptions = new JpegOptions();
                        jpegOptions.Quality = 90;

                        // Save the CAD drawing as a JPEG image
                        cadImage.Save(outputPath, jpegOptions);
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}