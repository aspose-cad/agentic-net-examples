using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                Console.WriteLine($"Enumerating embedded DGN objects in '{inputPath}':");
                bool found = false;

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadInsertObject insertObj)
                    {
                        string blockName = insertObj.Name;
                        if (!string.IsNullOrEmpty(blockName) &&
                            blockName.EndsWith(".dgn", StringComparison.OrdinalIgnoreCase))
                        {
                            Cad3DPoint pt = insertObj.InsertionPoint;
                            Console.WriteLine($"- DGN reference block: {blockName} at ({pt.X}, {pt.Y}, {pt.Z})");
                            found = true;
                        }
                    }
                }

                if (!found)
                {
                    Console.WriteLine("No embedded DGN objects found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}