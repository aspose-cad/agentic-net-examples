using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "layout.png";
        string layoutName = "Layout1";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (Image loadedImage = Image.Load(inputPath))
            {
                CadImage cadImage = loadedImage as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Verify the requested layout exists
                if (!cadImage.Layouts.ContainsKey(layoutName))
                {
                    Console.Error.WriteLine($"Layout \"{layoutName}\" not found in the CAD file.");
                    return;
                }

                // Prepare rasterization options to render only the specified layout
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = new string[] { layoutName }
                };

                // Prepare PNG options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered layout as PNG
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Layout \"{layoutName}\" exported successfully to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}