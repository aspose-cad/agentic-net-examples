using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;

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
                    Console.Error.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                // Extract layout identifiers and their corresponding names
                foreach (var kvp in cadImage.Layouts)
                {
                    string layoutId = kvp.Key;
                    CadLayout layout = kvp.Value;

                    // Use layout's string representation as the name (fallback if specific property is unavailable)
                    string layoutName = layout?.ToString() ?? "N/A";

                    Console.WriteLine($"Layout Id: {layoutId}, Name: {layoutName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}