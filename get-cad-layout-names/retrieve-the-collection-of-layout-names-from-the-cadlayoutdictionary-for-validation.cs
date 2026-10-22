using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the CAD file (DWG, DXF, etc.)
            string inputPath = "sample.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file using Aspose.CAD
            using (Image image = Image.Load(inputPath))
            {
                // Cast the generic Image to CadImage to access CAD-specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("The loaded file is not a supported CAD image.");
                    return;
                }

                // Retrieve the layout dictionary from the CAD image
                IReadOnlyDictionary<string, CadLayout> layoutDictionary = cadImage.Layouts;

                // Validate and display layout names
                if (layoutDictionary == null || layoutDictionary.Count == 0)
                {
                    Console.WriteLine("No layouts were found in the drawing.");
                }
                else
                {
                    Console.WriteLine("Layouts found in the drawing:");
                    foreach (KeyValuePair<string, CadLayout> kvp in layoutDictionary)
                    {
                        Console.WriteLine($"- {kvp.Key}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Print concise error information without crashing
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}