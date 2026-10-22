using System;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                // Path to the CAD file to be processed
                string inputPath = "input.dwg";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD file using Aspose.CAD
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the loaded image to CadImage to access CAD-specific members
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("The file could not be loaded as a CAD image.");
                        return;
                    }

                    // Iterate through all top‑level drawing entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Identify circle entities using pattern matching
                        if (entity is CadCircle circle)
                        {
                            // Output circle details: layer, center coordinates, and radius
                            Console.WriteLine(
                                $"Circle found on layer '{circle.LayerName}': " +
                                $"Center = ({circle.CenterPoint.X}, {circle.CenterPoint.Y}), " +
                                $"Radius = {circle.Radius}");
                        }
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}