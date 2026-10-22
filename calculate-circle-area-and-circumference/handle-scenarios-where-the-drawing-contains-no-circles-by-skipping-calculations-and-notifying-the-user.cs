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
        string sourcePath = "input.dwg";

        if (!File.Exists(sourcePath))
        {
            Console.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(sourcePath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("The loaded file is not a CAD image.");
                    return;
                }

                bool hasCircle = false;
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadCircle)
                    {
                        hasCircle = true;
                        break;
                    }
                }

                if (!hasCircle)
                {
                    Console.WriteLine("No circles were found in the drawing. Skipping calculations.");
                    return;
                }

                Console.WriteLine("Circles detected. Proceeding with calculations...");

                int circleCount = 0;
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadCircle)
                    {
                        circleCount++;
                    }
                }
                Console.WriteLine($"Number of circles: {circleCount}");

                string outputPath = "output.png";
                try
                {
                    PngOptions pngOptions = new PngOptions();
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Rasterized image saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to save rasterized image: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}
