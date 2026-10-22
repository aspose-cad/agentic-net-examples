using System;
using System.IO;
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
            using (Image cadImage = Image.Load(inputPath))
            {
                if (cadImage is CadImage cad)
                {
                    var layouts = cad.Layouts;
                    if (layouts == null || layouts.Count == 0)
                    {
                        Console.WriteLine("No layouts found in the CAD file.");
                    }
                    else
                    {
                        // Iterate over layout dictionary entries
                        foreach (var kvp in layouts)
                        {
                            // kvp.Key is the layout name
                            Console.WriteLine($"Layout: {kvp.Key}");
                        }
                    }
                }
                else
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}