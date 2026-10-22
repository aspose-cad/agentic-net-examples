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
            Console.Error.WriteLine($"Input file \"{inputPath}\" not found.");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Locate the index of the first line entity to remove
                int indexToRemove = -1;
                var entitiesArray = cadImage.Entities.ToArray();

                for (int i = 0; i < entitiesArray.Length; i++)
                {
                    if (entitiesArray[i] is CadLine)
                    {
                        indexToRemove = i;
                        break;
                    }
                }

                // Remove the entity if found (by recreating the collection without it)
                if (indexToRemove >= 0)
                {
                    var updatedEntities = entitiesArray
                        .Where((e, idx) => idx != indexToRemove)
                        .ToArray();

                    // Reassign the filtered entities back to the image (using reflection as the setter is internal)
                    var entitiesField = typeof(CadImage).GetField("_entities", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (entitiesField != null)
                    {
                        entitiesField.SetValue(cadImage, updatedEntities);
                        Console.WriteLine($"Entity at index {indexToRemove} removed.");
                    }
                    else
                    {
                        Console.WriteLine("Unable to modify entities collection; removal not performed.");
                    }
                }
                else
                {
                    Console.WriteLine("No matching entity found to remove.");
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
                Console.WriteLine($"Modified drawing saved to \"{outputPath}\".");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}