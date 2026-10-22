using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace FontSubstitutionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Paths – replace with actual locations as needed
            string inputPath = "input.dxf";
            string outputPath = "output.png";
            string customFontFolder = "CustomFonts";

            try
            {
                // Validate input file and custom font folder
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                if (!Directory.Exists(customFontFolder))
                {
                    Console.Error.WriteLine($"Custom font folder not found: {customFontFolder}");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD‑specific members
                    CadImage cadImage = (CadImage)image;

                    // Specify a fallback font name to be used when a required font is missing
                    cadImage.DefaultFont = "Arial";

                    // Note: Aspose.CAD does not expose a direct API to add a custom font folder.
                    // In a real scenario, you would ensure the fonts in 'customFontFolder' are installed
                    // on the system or accessible via the operating system's font configuration.

                    // Prepare rasterization options for PNG export
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Use layout names for rendering; select all available layouts
                        Layouts = cadImage.Layouts.Keys.ToArray(),
                        PageWidth = 1600,
                        PageHeight = 1200,
                        DrawType = CadDrawTypeMode.UseObjectColor
                    };

                    // Set up PNG export options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the rendered image
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"Export completed successfully: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}