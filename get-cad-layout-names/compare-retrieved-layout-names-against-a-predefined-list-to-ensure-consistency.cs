using System;
using System.IO;
using System.Collections.Generic;
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

            // Predefined list of expected layout names
            List<string> expectedLayouts = new List<string> { "Model", "Layout1", "Layout2" };

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Retrieve actual layout names from the CAD file
                List<string> actualLayouts = new List<string>();
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    actualLayouts.Add(layout.LayoutName);
                }

                // Compare actual layout names against the predefined list
                bool allMatch = true;

                foreach (string expected in expectedLayouts)
                {
                    if (!actualLayouts.Contains(expected))
                    {
                        Console.WriteLine($"Missing expected layout: {expected}");
                        allMatch = false;
                    }
                }

                foreach (string actual in actualLayouts)
                {
                    if (!expectedLayouts.Contains(actual))
                    {
                        Console.WriteLine($"Unexpected layout found: {actual}");
                        allMatch = false;
                    }
                }

                if (allMatch)
                {
                    Console.WriteLine("All layout names match the predefined list.");
                }
                else
                {
                    Console.WriteLine("Layout name discrepancies detected.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}