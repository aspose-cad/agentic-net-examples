using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("The loaded file is not a supported CAD image.");
                    return;
                }

                dynamic layouts = cadImage.Layouts;
                var layoutNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                bool duplicateFound = false;

                foreach (dynamic layout in layouts)
                {
                    string name = layout.Name as string ?? layout.Name?.ToString() ?? string.Empty;
                    if (!layoutNames.Add(name))
                    {
                        duplicateFound = true;
                        Console.WriteLine($"Duplicate layout name found: {name}");
                    }
                }

                if (!duplicateFound)
                {
                    Console.WriteLine("No duplicate layout names found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}