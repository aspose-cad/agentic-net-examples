using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Source file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            string outputPath = Path.Combine(outputDir, "output.dwg");

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Iterate top‑level entities and change line colors to red (ColorId = 1)
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine line)
                    {
                        line.ColorId = 1;
                    }
                }

                // List layers with their visibility flags
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isFrozen = (layer.Flags & 1) != 0;
                    bool isOff = layer.ColorId < 0;
                    bool isLocked = (layer.Flags & 0x10) != 0; // bit 4
                    Console.WriteLine($"Layer: {layer.Name}, Frozen: {isFrozen}, Off: {isOff}, Locked: {isLocked}");
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
                Console.WriteLine($"Saved modified drawing to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}