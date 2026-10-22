using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            string outputPath = "output.png";
            string layoutName = "Layout1";
            string layerName = "Layer0";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Configure rasterization options with specific layout and layer
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.Layouts = new string[] { layoutName };
                    rasterOptions.Layers = new string[] { layerName };
                    rasterOptions.PageWidth = 1600;
                    rasterOptions.PageHeight = 1200;
                    rasterOptions.DrawType = CadDrawTypeMode.UseObjectColor;

                    // Set PNG export options
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the selected layout and layer to PNG
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"File saved successfully to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}