using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.IO;

namespace CadLayoutEnumeration
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder input CAD file path
                string inputPath = "input.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the generic Image to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Obtain the layout dictionary (read‑only)
                    IReadOnlyDictionary<string, CadLayout> layoutDict = cadImage.Layouts;

                    // Enumerate each entry to obtain layout identifiers
                    foreach (KeyValuePair<string, CadLayout> entry in layoutDict)
                    {
                        Console.WriteLine($"Layout Identifier: {entry.Key}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}