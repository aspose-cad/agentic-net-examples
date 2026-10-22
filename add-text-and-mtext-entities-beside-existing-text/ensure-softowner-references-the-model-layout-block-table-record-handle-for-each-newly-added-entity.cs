using System;
using System.Collections;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using System.IO;

class Program
{
    static void Main()
    {
        // Path to the source CAD file
        string inputPath = "input.dwg";

        
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }
// Load the CAD file
        using (Image image = Image.Load(inputPath))
        {
            CadImage cadImage = image as CadImage;
            if (cadImage == null)
            {
                Console.WriteLine("Failed to load CAD image.");
                return;
            }

            // Iterate through block definitions
            foreach (DictionaryEntry entry in cadImage.BlockEntities)
            {
                CadBlockEntity block = entry.Value as CadBlockEntity;
                if (block == null)
                {
                    continue;
                }

                Console.WriteLine($"Block Name: {entry.Key}");

                // Iterate entities inside the block
                foreach (CadEntityBase entity in block.Entities)
                {
                    CadLine line = entity as CadLine;
                    if (line != null)
                    {
                        Console.WriteLine($"Line from ({line.FirstPoint.X}, {line.FirstPoint.Y}) to ({line.SecondPoint.X}, {line.SecondPoint.Y})");
                    }
                }
            }

            // Save the CAD image to PNG with error handling
            string outputPath = "output.png";
            PngOptions pngOptions = new PngOptions();

            try
            {
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image saved successfully to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image: {ex.Message}");
            }
        }
    }
}