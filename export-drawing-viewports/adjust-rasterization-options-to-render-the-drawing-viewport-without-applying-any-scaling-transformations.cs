using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage if CAD‑specific members are needed
                CadImage cad = (CadImage)cadImage;

                // Configure rasterization options to render the viewport without scaling
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Use default unit type and line scale (no scaling)
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f,
                    // Let the renderer determine page size from the drawing's natural extents
                    PageWidth = 0,
                    PageHeight = 0,
                    // Render the ModelSpace layout
                    Layouts = new[] { "ModelSpace" }
                };

                // Set PNG export options and assign the rasterization options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered image
                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}