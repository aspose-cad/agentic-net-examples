using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.sat";
        const string outputPath = "output.sat";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                foreach (CadBlockEntity block in cadImage.BlockEntities.Values)
                {
                    Console.WriteLine($"Block name: {block.Name}");

                    if (block.Entities != null && block.Entities.Count > 0)
                    {
                        var firstEntity = block.Entities.FirstOrDefault();
                        if (firstEntity != null && block.Entities.Remove(firstEntity))
                        {
                            Console.WriteLine($"Removed first entity from block '{block.Name}'.");
                        }
                    }
                }

                cadImage.Save(outputPath);
                Console.WriteLine($"Modified drawing saved to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}