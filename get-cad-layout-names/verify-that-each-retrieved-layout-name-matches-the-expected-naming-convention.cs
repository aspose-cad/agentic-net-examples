using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                // Retrieve layout names from the dictionary keys
                IList<string> layoutNames = new List<string>(cadImage.Layouts.Keys);
                if (layoutNames == null || layoutNames.Count == 0)
                {
                    Console.WriteLine("No layouts found in the CAD file.");
                }
                else
                {
                    foreach (string layoutName in layoutNames)
                    {
                        // Expected naming convention: layout name starts with "Layout"
                        if (layoutName.StartsWith("Layout", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Layout \"{layoutName}\" matches the naming convention.");
                        }
                        else
                        {
                            Console.WriteLine($"Layout \"{layoutName}\" does NOT match the naming convention.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}