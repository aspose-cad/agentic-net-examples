using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                if (image is CadImage cadImage)
                {
                    Console.WriteLine("Available layouts:");
                    foreach (var layoutName in cadImage.Layouts.Keys)
                    {
                        Console.WriteLine(layoutName);
                    }
                }
                else
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}