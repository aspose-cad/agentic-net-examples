using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDirectory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cadImg = (CadImage)cadImage;

                foreach (var block in cadImg.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block definition found: {block.Name}");
                }

                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };

                cadImg.Save(outputPath, dxfOptions);
                Console.WriteLine($"DXF file saved successfully to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}