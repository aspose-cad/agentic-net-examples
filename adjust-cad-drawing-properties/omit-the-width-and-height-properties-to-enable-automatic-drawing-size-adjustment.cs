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
        // Path to the input CAD file
        string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Inspect block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}");
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        string typeName = entity.GetType().Name;
                        Console.Write($"  Entity: {typeName}");

                        if (entity is CadLine line)
                        {
                            Console.WriteLine($"  Start({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) " +
                                              $"End({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                        }
                        else if (entity is CadLwPolyline polyline)
                        {
                            Console.WriteLine($"  Points count: {polyline.Coordinates.Count}");
                        }
                        else if (entity is CadInsertObject insert)
                        {
                            Console.WriteLine($"  Inserted Block: {insert.Name} Scale({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ})");
                        }
                        else
                        {
                            Console.WriteLine();
                        }
                    }
                }

                // Save the drawing to PNG without specifying Width/Height (automatic size)
                string outputPath = "output.png";
                PngOptions pngOptions = new PngOptions();

                try
                {
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Image saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to save image: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}
