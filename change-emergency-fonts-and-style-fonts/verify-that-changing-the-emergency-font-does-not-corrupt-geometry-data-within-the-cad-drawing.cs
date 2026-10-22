using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

namespace AsposeCadDemo
{
    class Program
    {
        static void Main()
        {
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    int entityCountBefore = cadImage.Entities.Count();

                    // Change the emergency (default) font
                    cadImage.DefaultFont = "Arial Unicode MS";

                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1600,
                        Layouts = new[] { "Model" }
                    };

                    var dwgOptions = new DwgOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    cadImage.Save(outputPath, dwgOptions);

                    int entityCountAfter = cadImage.Entities.Count();

                    Console.WriteLine($"Entity count before: {entityCountBefore}");
                    Console.WriteLine($"Entity count after : {entityCountAfter}");

                    if (entityCountBefore == entityCountAfter)
                    {
                        Console.WriteLine("Geometry data remains unchanged after emergency font modification.");
                    }
                    else
                    {
                        Console.WriteLine("Potential geometry data discrepancy detected after emergency font modification.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
