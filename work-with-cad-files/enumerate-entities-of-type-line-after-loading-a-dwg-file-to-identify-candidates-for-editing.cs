using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadLineEnumeration
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string sourcePath = "input.dwg";

                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                using (Image image = Image.Load(sourcePath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    List<CadLine> lineEntities = new List<CadLine>();

                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity is CadLine line)
                        {
                            lineEntities.Add(line);
                        }
                    }

                    Console.WriteLine($"Found {lineEntities.Count} line entities:");
                    int index = 1;
                    foreach (CadLine line in lineEntities)
                    {
                        var start = line.FirstPoint;
                        var end = line.SecondPoint;
                        Console.WriteLine($"Line {index}: Start({start.X}, {start.Y}, {start.Z}) - End({end.X}, {end.Y}, {end.Z})");
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}