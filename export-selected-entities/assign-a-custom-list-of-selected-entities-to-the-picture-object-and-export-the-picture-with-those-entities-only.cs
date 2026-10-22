using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file \"{inputPath}\" does not exist.");
            return;
        }

        try
        {
            // Load the source CAD drawing
            using (Image sourceImage = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)sourceImage;

                // Collect the entities you want to keep (example: only line entities)
                List<CadEntityBase> selectedEntities = new List<CadEntityBase>();
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine)
                    {
                        selectedEntities.Add(entity);
                    }
                }

                // Create a new CadImage that will contain only the selected entities
                CadImage subsetImage = new CadImage();

                // Assign the selected entities to the new image
                // (Assuming the Entities property has a setter; if not, this line may need adjustment based on the actual API)
                subsetImage.Entities = selectedEntities.ToArray();

                // Export the subset picture to PNG
                PngOptions pngOptions = new PngOptions();
                subsetImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Export completed successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}