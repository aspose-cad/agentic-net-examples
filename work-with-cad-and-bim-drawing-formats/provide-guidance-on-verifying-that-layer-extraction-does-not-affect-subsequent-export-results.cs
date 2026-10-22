using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadLayerExportVerification
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            string layerExportPath = "layer_output.png";
            string fullExportPath = "full_output.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Export only a specific layer (e.g., "Layer1")
                    CadRasterizationOptions layerRasterOptions = new CadRasterizationOptions();
                    layerRasterOptions.Layers = new string[] { "Layer1" };
                    layerRasterOptions.PageWidth = 1600;
                    layerRasterOptions.PageHeight = 1200;
                    // Optional: specify layout if needed
                    // layerRasterOptions.Layouts = new string[] { "Model" };

                    PngOptions layerPngOptions = new PngOptions();
                    layerPngOptions.VectorRasterizationOptions = layerRasterOptions;

                    cadImage.Save(layerExportPath, layerPngOptions);
                    Console.WriteLine($"Layer-specific export saved to: {layerExportPath}");

                    // Export the full drawing (all layers)
                    CadRasterizationOptions fullRasterOptions = new CadRasterizationOptions();
                    fullRasterOptions.PageWidth = 1600;
                    fullRasterOptions.PageHeight = 1200;
                    // Ensure no layer filter is applied
                    fullRasterOptions.Layers = null;

                    PngOptions fullPngOptions = new PngOptions();
                    fullPngOptions.VectorRasterizationOptions = fullRasterOptions;

                    cadImage.Save(fullExportPath, fullPngOptions);
                    Console.WriteLine($"Full drawing export saved to: {fullExportPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}