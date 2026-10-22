using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder paths – replace with actual file locations
            string inputPath = "input.dwg";
            string outputPath = "output.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded file is not a supported CAD image.");
                    return;
                }

                // NOTE: Adding new layouts is not supported directly via the public API.
                // This sample focuses on retrieving layout names after any modifications
                // that might have been performed elsewhere.

                // Retrieve layout names
                string[] layoutNames = cadImage.Layouts?.Keys?.ToArray() ?? Array.Empty<string>();

                Console.WriteLine("Available layouts:");
                foreach (string name in layoutNames)
                {
                    Console.WriteLine(name);
                }

                // Prepare rasterization options (render the first layout if any)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = layoutNames.Length > 0 ? new[] { layoutNames[0] } : Array.Empty<string>(),
                    PageWidth = 1600,
                    PageHeight = 1200
                };

                // Set PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as a PNG image
                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}