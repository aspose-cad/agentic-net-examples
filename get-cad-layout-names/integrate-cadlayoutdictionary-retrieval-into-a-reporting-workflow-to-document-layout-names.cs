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
        string inputPath = "sample.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cadImg = cadImage as CadImage;
                if (cadImg == null)
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                    return;
                }

                // Retrieve layout dictionary (available via Layouts property)
                var layoutDict = cadImg.Layouts;
                if (layoutDict == null || layoutDict.Count == 0)
                {
                    Console.WriteLine("No layouts found in the CAD file.");
                }
                else
                {
                    Console.WriteLine("Layouts found in the CAD file:");
                    foreach (KeyValuePair<string, CadLayout> entry in layoutDict)
                    {
                        Console.WriteLine($"- {entry.Key}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}