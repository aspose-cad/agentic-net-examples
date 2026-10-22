using System;
using System.IO;
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

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Set the default font for the drawing
                string newFontName = "Arial";
                cadImage.DefaultFont = newFontName;

                // Verify that text entities are present (they will use the default font)
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadText)
                    {
                        Console.WriteLine($"Text entity detected. Using default font: {cadImage.DefaultFont}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}