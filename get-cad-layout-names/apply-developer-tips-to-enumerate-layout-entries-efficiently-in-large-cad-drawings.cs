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
            // Define input file path
            string inputPath = "sample.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific properties
                CadImage cadSpecificImage = cadImage as CadImage;
                if (cadSpecificImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Enumerate layout names efficiently
                Console.WriteLine("CAD Layouts:");
                foreach (var layoutName in cadSpecificImage.Layouts.Keys)
                {
                    Console.WriteLine($"- {layoutName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}