using System;
using System.Collections;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";
        const string blockNameToRemove = "MyBlock";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // BlockEntities holds the block definitions dictionary
                IDictionary blockDictionary = cadImage.BlockEntities as IDictionary;
                if (blockDictionary == null)
                {
                    Console.WriteLine("Unable to access block dictionary.");
                    return;
                }

                if (blockDictionary.Contains(blockNameToRemove))
                {
                    blockDictionary.Remove(blockNameToRemove);
                    Console.WriteLine($"Block '{blockNameToRemove}' has been removed.");
                }
                else
                {
                    Console.WriteLine($"Block '{blockNameToRemove}' was not found in the drawing.");
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