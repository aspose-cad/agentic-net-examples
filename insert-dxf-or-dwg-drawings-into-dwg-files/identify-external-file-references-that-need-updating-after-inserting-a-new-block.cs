using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

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
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                    return;
                }

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadInsertObject insertObject)
                    {
                        string blockName = insertObject.Name ?? string.Empty;

                        if (blockName.EndsWith(".dwg", StringComparison.OrdinalIgnoreCase) ||
                            blockName.EndsWith(".dxf", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"External reference detected: {blockName}");
                            // Add code here to update the external reference path if required
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}