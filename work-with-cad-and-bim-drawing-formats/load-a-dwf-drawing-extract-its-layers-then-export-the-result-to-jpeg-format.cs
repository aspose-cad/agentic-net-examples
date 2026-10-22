using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

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
                CadImage cadImage = (CadImage)image;

                // Extract and display layer information
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isOff = layer.ColorId < 0;
                    bool isFrozen = (layer.Flags & 1) != 0;
                    bool isLocked = ((layer.Flags >> 4) & 1) != 0;

                    Console.WriteLine($"Layer: {layer.Name}, ColorId: {layer.ColorId}, Off: {isOff}, Frozen: {isFrozen}, Locked: {isLocked}");
                }

                // Set rasterization options for JPEG export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Use default page size and layout; adjust if needed
                    // Layouts = null,
                    // Layers = null
                };

                JpegOptions jpegOptions = new JpegOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as JPEG
                image.Save(outputPath, jpegOptions);
                Console.WriteLine($"Exported JPEG saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}