using System;
using System.IO;
using System.Runtime.InteropServices;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Define input CAD file and output image paths
        string cadFilePath = "sample.dxf";
        string outputImagePath = "output.png";

        // Resolve full paths
        string fullCadPath = Path.GetFullPath(cadFilePath);
        string fullOutputPath = Path.GetFullPath(outputImagePath);

        // Ensure directories exist
        Directory.CreateDirectory(Path.GetDirectoryName(fullCadPath) ?? ".");
        Directory.CreateDirectory(Path.GetDirectoryName(fullOutputPath) ?? ".");

        // Choose comparison based on OS case‑sensitivity
        StringComparison pathComparison = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        // Example path comparison respecting OS rules
        bool pathsAreEqual = string.Equals(fullCadPath, fullOutputPath, pathComparison);
        Console.WriteLine($"Paths equal (OS‑aware): {pathsAreEqual}");

        // Ensure the CAD file exists – create a minimal placeholder if missing
        if (!File.Exists(fullCadPath))
        {
            // Minimal DXF content sufficient for Aspose.CAD to load
            string minimalDxf = "0\r\nSECTION\r\n2\r\nHEADER\r\n0\r\nENDSEC\r\n0\r\nEOF\r\n";
            File.WriteAllText(fullCadPath, minimalDxf);
            Console.WriteLine($"Placeholder CAD file created at: {fullCadPath}");
        }

        try
        {
            // Load the CAD drawing
            using (Image cadImage = Image.Load(fullCadPath))
            {
                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800f,
                    PageHeight = 600f,
                    UnitType = UnitType.Centimenter,
                    LineScale = 0.5f
                };

                // Configure PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as PNG
                cadImage.Save(fullOutputPath, pngOptions);
                Console.WriteLine($"CAD file exported to PNG at: {fullOutputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during CAD processing: {ex.Message}");
        }
    }
}