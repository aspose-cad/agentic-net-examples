using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            using (Image loadedImage = Image.Load(inputPath))
            {
                CadImage cadImage = loadedImage as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("The loaded file is not a supported CAD image.");
                    return;
                }

                // Iterate through each layout in the CAD file
                foreach (var layoutEntry in cadImage.Layouts)
                {
                    CadLayout layout = layoutEntry.Value;
                    string layoutName = layout.LayoutName ?? layoutEntry.Key;

                    var min = layout.MinExtents;
                    var max = layout.MaxExtents;

                    double width = max.X - min.X;
                    double height = max.Y - min.Y;

                    Console.WriteLine($"Layout '{layoutName}': Width = {width}, Height = {height}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error loading CAD file: {ex.Message}");
        }
    }
}