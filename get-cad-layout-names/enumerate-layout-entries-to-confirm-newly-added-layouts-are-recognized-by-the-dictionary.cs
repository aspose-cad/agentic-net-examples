using System;
using System.IO;
using System.Collections.Generic;
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
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("The loaded file is not a CAD image.");
                    return;
                }

                // Enumerate layout entries to confirm they are recognized.
                IReadOnlyDictionary<string, CadLayout> layouts = cadImage.Layouts;
                if (layouts == null || layouts.Count == 0)
                {
                    Console.WriteLine("No layouts found in the CAD file.");
                }
                else
                {
                    Console.WriteLine("Layouts found:");
                    foreach (KeyValuePair<string, CadLayout> entry in layouts)
                    {
                        Console.WriteLine($"- Layout name: {entry.Key}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}