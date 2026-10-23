using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputDir = "Output";
        string outputPath = Path.Combine(outputDir, "output.dxf");

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Ascii
                };

                cadImage.Save(outputPath, dxfOptions);
            }

            Console.WriteLine($"DXF file saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}