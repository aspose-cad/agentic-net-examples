using System;
using System.IO;
using System.Collections;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("Failed to cast the loaded image to CadImage.");
                    return;
                }

                if (cadImage.Layouts != null && cadImage.Layouts.Count > 0)
                {
                    Console.WriteLine("Layout names:");
                    foreach (DictionaryEntry entry in (IEnumerable)cadImage.Layouts)
                    {
                        Console.WriteLine(entry.Key?.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("No layouts found in the CAD file.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}