using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("Failed to load the DWF file as a CAD image.");
                    return;
                }

                var layers = cadImage.Layers;
                if (layers == null)
                {
                    Console.WriteLine("No layers found in the drawing.");
                    return;
                }

                bool anyLayer = false;
                Console.WriteLine("Layer information:");
                foreach (var obj in layers)
                {
                    if (obj is CadLayerTable layer)
                    {
                        anyLayer = true;
                        string name = layer.Name ?? "<Unnamed>";
                        short colorId = layer.ColorId;
                        int flags = layer.Flags;

                        bool isOff = colorId < 0;
                        bool isFrozen = (flags & 1) != 0;
                        bool isVisible = !(isOff || isFrozen);

                        Console.WriteLine($"- Name: {name}, ColorId: {colorId}, Visible: {isVisible}");
                    }
                }

                if (!anyLayer)
                {
                    Console.WriteLine("No layer entries found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing the DWF file: {ex.Message}");
        }
    }
}