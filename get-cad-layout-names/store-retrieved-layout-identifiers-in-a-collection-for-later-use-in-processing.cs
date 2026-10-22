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
        try
        {
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Store layout identifiers for later processing
                List<string> layoutIdentifiers = new List<string>();
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    layoutIdentifiers.Add(layout.LayoutName);
                }

                // Example usage: display retrieved layout identifiers
                Console.WriteLine("Retrieved layout identifiers:");
                foreach (string name in layoutIdentifiers)
                {
                    Console.WriteLine(name);
                }

                // Further processing can use the layoutIdentifiers collection here
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}