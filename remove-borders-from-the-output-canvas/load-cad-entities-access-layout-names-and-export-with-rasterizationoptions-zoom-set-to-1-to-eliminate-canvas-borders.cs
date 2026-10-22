using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Path to the source CAD file
        string sourcePath = "input.dwg";
        // Path to the output raster image
        string outputPath = "output.png";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            // Load the CAD file
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Retrieve layout names from the CAD drawing
                string[] layoutNames = new string[cadImage.Layouts.Count];
                int idx = 0;
                foreach (var layout in cadImage.Layouts.Values)
                {
                    // CadLayout does not expose a 'Name' property; use 'LayoutName' instead
                    layoutNames[idx++] = layout.LayoutName;
                }

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = layoutNames, // render the specified layouts
                    Zoom = 1 // eliminate canvas borders
                };

                // Set up PNG export options
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