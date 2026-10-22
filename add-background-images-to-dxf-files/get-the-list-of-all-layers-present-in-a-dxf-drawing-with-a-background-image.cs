using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("The loaded file is not a CAD image.");
                    return;
                }

                Console.WriteLine("Layers present in the DXF drawing:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    Console.WriteLine(layer.Name);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}