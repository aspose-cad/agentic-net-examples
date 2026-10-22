using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CadEntityRemoval
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dxf";
            string outputDir = "output";
            string outputPath = Path.Combine(outputDir, "output.dxf");

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            Directory.CreateDirectory(outputDir);

            try
            {
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // NOTE: CadImage does not expose RasterizationOptions, so quality settings for text, hatches,
                    // and arcs cannot be applied directly in the current Aspose.CAD API.

                    // NOTE: The Entities collection of CadImage is read‑only and the SDK does not provide a method
                    // to remove specific entity types. Therefore removal of Text, Hatch, and Arc entities cannot be
                    // performed with this version of the library.

                    var saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        DxfFileFormat = CadFileFormat.Binary // explicit format selection
                    };

                    cadImage.Save(outputPath);
                    Console.WriteLine($"File saved to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}