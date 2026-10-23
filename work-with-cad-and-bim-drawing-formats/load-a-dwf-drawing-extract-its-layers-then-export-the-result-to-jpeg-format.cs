using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Dwf;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwf";
        string outputPath = "output.jpg";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (Image image = Image.Load(inputPath))
            {
                DwfImage dwf = (DwfImage)image;

                // Extract and display layer names
                string[] layerNames = dwf.Layers.GetLayersNames().ToArray();
                Console.WriteLine("Layers in DWF:");
                foreach (string name in layerNames)
                {
                    Console.WriteLine($"- {name}");
                }

                // Set rasterization options for JPEG export, including all layers
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layers = layerNames
                };

                JpegOptions jpegOptions = new JpegOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as JPEG
                dwf.Save(outputPath, jpegOptions);
                Console.WriteLine($"Exported JPEG saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}