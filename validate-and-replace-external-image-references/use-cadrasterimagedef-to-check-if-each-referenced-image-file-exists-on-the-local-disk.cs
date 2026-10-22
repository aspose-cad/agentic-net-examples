using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadRasterCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                    return;
                }

                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Iterate over all objects to find raster image definitions
                    foreach (CadBaseObject baseObj in cadImage.Objects)
                    {
                        if (baseObj is CadRasterImageDef rasterDef)
                        {
                            string rasterFileName = rasterDef.FileName;
                            string rasterFullPath = Path.Combine(Path.GetDirectoryName(inputPath) ?? string.Empty, rasterFileName);

                            if (File.Exists(rasterFullPath))
                            {
                                Console.WriteLine($"Raster image file exists: {rasterFullPath}");
                            }
                            else
                            {
                                Console.WriteLine($"Raster image file missing: {rasterFullPath}");
                            }
                        }
                    }

                    // Save the CAD file (preserving editable objects)
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath, saveOptions);
                }

                Console.WriteLine($"CAD file saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}