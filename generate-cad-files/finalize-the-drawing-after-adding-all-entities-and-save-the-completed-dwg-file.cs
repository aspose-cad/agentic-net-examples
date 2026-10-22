using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Create a new line entity
                CadLine line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 100, 0),
                    LayerName = "0"
                };

                // Append the new line to existing entities
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { line }).ToArray();

                // Save the modified drawing
                cadImage.Save(outputPath);
            }

            Console.WriteLine($"Drawing saved successfully to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}