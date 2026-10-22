using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.dwg";

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
                // Cast to CadImage to access CAD object model
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Find the first line entity in the drawing
                CadLine lineEntity = null;
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine line)
                    {
                        lineEntity = line;
                        break;
                    }
                }

                if (lineEntity == null)
                {
                    Console.Error.WriteLine("No line entity found in the drawing.");
                    return;
                }

                // Update the start point of the selected line
                // New coordinates (example): X=10, Y=20, Z=0
                lineEntity.FirstPoint = new Cad3DPoint(10.0, 20.0, 0.0);

                // Save the modified drawing
                DwgOptions saveOptions = new DwgOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath);
                Console.WriteLine($"Line start point updated and saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}