using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Paths
        string inputPath = "sample.dwg";
        string outputDir = "Output";
        string jpegPath = Path.Combine(outputDir, "sample.jpg");

        // Validate input file
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        Directory.CreateDirectory(outputDir);

        try
        {
            // Load CAD drawing
            using (Image cadImg = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = (CadImage)cadImg;

                // Convert to JPEG
                JpegOptions jpegOptions = new JpegOptions();
                cadImage.Save(jpegPath, jpegOptions);
                Console.WriteLine($"CAD drawing saved as JPEG to: {jpegPath}");

                // Verify that JPEG colors match the original CAD palette
                // NOTE: The current Aspose.CAD API does not expose a ColorTable or palette information for CadImage,
                // and JPEG images loaded via Aspose.CAD do not provide palette data either.
                // Therefore, programmatic verification of color palette equivalence is not possible.
                Console.WriteLine("Verification note: CAD palette information is not available via the current Aspose.CAD API; " +
                                  "automatic color comparison cannot be performed. Please verify colors manually if required.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}