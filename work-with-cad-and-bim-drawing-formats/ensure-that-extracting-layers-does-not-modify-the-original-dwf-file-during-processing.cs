using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        string sourcePath = "input.dwf";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(sourcePath))
            {
                if (image is CadImage cadImage)
                {
                    Console.WriteLine("Layers in the DWF file:");
                    foreach (CadLayerTable layerObj in cadImage.Layers)
                    {
                        if (layerObj is CadLayerTable layer)
                        {
                            Console.WriteLine("- " + layer.Name);
                        }
                    }
                }
                else
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}