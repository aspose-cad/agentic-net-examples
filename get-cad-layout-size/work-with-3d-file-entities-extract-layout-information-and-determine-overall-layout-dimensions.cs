using System;
using System.Collections.Generic;
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
            Console.Error.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Retrieve layout dictionary (key: layout name, value: CadLayout object)
                IReadOnlyDictionary<string, CadLayout> layouts = cadImage.Layouts;

                double overallMinX = double.MaxValue;
                double overallMinY = double.MaxValue;
                double overallMaxX = double.MinValue;
                double overallMaxY = double.MinValue;

                foreach (KeyValuePair<string, CadLayout> kvp in layouts)
                {
                    string layoutName = kvp.Key;
                    CadLayout layout = kvp.Value;

                    Console.WriteLine($"Layout Name: {layoutName}");

                    double minX = layout.MinExtents.X;
                    double minY = layout.MinExtents.Y;
                    double maxX = layout.MaxExtents.X;
                    double maxY = layout.MaxExtents.Y;

                    Console.WriteLine($"  Extents: MinX={minX}, MinY={minY}, MaxX={maxX}, MaxY={maxY}");

                    if (minX < overallMinX) overallMinX = minX;
                    if (minY < overallMinY) overallMinY = minY;
                    if (maxX > overallMaxX) overallMaxX = maxX;
                    if (maxY > overallMaxY) overallMaxY = maxY;
                }

                double overallWidth = overallMaxX - overallMinX;
                double overallHeight = overallMaxY - overallMinY;

                Console.WriteLine($"Overall drawing dimensions: Width = {overallWidth}, Height = {overallHeight}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}