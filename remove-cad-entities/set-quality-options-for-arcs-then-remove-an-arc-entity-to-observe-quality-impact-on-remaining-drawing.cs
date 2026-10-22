using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file '{inputPath}' not found.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Configure rasterization options (quality settings)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example quality setting; ArcTolerance not available in this API version
                    LineScale = 0.01f
                };

                // Find the first arc entity
                CadEntityBase arcEntity = cadImage.Entities.OfType<CadArc>().FirstOrDefault();

                // Remove the arc entity if found
                if (arcEntity != null)
                {
                    cadImage.Entities = cadImage.Entities.Where(e => e != arcEntity).ToArray();
                }

                // Prepare PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the modified drawing
                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}