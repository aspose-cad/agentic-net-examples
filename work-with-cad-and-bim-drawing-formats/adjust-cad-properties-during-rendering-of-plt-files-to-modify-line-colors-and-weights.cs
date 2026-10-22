using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadPltRender
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = "input.plt";
            string outputPath = "output.png";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Adjust line weight via LineScale
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        LineScale = 0.5f // Adjust line thickness
                    };

                    // Modify line colors by setting ColorId (indexed color) to red (index 1)
                    foreach (var entity in ((CadImage)cadImage).Entities)
                    {
                        var prop = entity.GetType().GetProperty("ColorId");
                        if (prop != null && prop.CanWrite)
                        {
                            prop.SetValue(entity, (short)1);
                        }
                    }

                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine("Rendering completed. Output saved to: " + outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}