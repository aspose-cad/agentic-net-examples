using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Iterate through all top‑level entities in the drawing
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Read the Hyperlink field if it is present
                    string hyperlink = entity.Hyperlink;
                    if (!string.IsNullOrEmpty(hyperlink))
                    {
                        Console.WriteLine($"Entity Type: {entity.GetType().Name}, Hyperlink: {hyperlink}");
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