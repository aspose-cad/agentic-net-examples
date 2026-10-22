using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadVectorRasterizationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            Image cadImage;
            try
            {
                cadImage = Image.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load CAD image: {ex.Message}");
                return;
            }

            using (cadImage)
            {
                var pngOptions = new PngOptions();
                var vectorOptions = new CadRasterizationOptions(); // Concrete implementation

                ConfigureVectorRasterizationOptions(vectorOptions, 2.5);
                pngOptions.VectorRasterizationOptions = vectorOptions;

                string outputPath = "output.png";
                try
                {
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Image saved successfully to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to save image: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Configures the provided VectorRasterizationOptions with the specified line scale.
        /// </summary>
        /// <param name="options">The VectorRasterizationOptions instance to configure.</param>
        /// <param name="lineScale">The desired line scale factor.</param>
        static void ConfigureVectorRasterizationOptions(VectorRasterizationOptions options, double lineScale)
        {
            options.LineScale = (float)lineScale;
        }
    }
}
