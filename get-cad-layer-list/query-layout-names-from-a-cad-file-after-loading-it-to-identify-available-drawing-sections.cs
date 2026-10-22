using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

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
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                var layouts = cadImage.Layouts;
                if (layouts == null || layouts.Count == 0)
                {
                    Console.WriteLine("No layouts found in the CAD file.");
                    return;
                }

                Console.WriteLine("Available layout names:");
                foreach (var layoutName in layouts.Keys)
                {
                    Console.WriteLine($"- {layoutName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}