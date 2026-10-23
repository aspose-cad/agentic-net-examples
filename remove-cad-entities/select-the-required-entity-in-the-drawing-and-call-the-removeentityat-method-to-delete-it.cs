using System;
using System.Collections.Generic;
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

                // Get mutable list of entities
                List<CadEntityBase> entities = cadImage.Entities.ToList();

                // Locate the index of the first line entity to remove
                int indexToRemove = entities.FindIndex(e => e is CadLine);

                if (indexToRemove >= 0)
                {
                    entities.RemoveAt(indexToRemove);
                    // Reassign the updated collection preserving other entities
                    cadImage.Entities = entities.ToArray();
                    Console.WriteLine($"Entity at index {indexToRemove} removed.");
                }
                else
                {
                    Console.WriteLine("No matching entity found to remove.");
                }

                // Ensure output directory exists
                string? outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
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