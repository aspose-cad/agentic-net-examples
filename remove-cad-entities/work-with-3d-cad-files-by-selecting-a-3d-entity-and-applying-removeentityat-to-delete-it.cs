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
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Find index of first 3D entity (Cad3DFace)
                int indexToRemove = -1;
                var entities = cadImage.Entities.ToArray();
                for (int i = 0; i < entities.Length; i++)
                {
                    if (entities[i] is Cad3DFace)
                    {
                        indexToRemove = i;
                        break;
                    }
                }

                if (indexToRemove >= 0)
                {
                    // Create a new entity collection without the selected entity
                    var updatedEntities = entities
                        .Where((e, idx) => idx != indexToRemove)
                        .ToArray();

                    // Assign the filtered collection back to the image
                    cadImage.Entities = updatedEntities;

                    Console.WriteLine($"Removed 3D entity at index {indexToRemove}.");
                }
                else
                {
                    Console.WriteLine("No 3D entity found to remove.");
                }

                // Save the modified CAD file
                cadImage.Save(outputPath);
                Console.WriteLine($"Saved modified file to {outputPath}.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}