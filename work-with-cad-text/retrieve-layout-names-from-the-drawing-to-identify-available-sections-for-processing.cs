using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AsposeCadLayoutNames
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD drawing file (DWG, DXF, etc.)
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the generic Image to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Prepare a list to hold layout names
                    List<string> layoutNames = new List<string>();

                    // Iterate through all layouts in the drawing and collect their names
                    foreach (CadLayout layout in cadImage.Layouts.Values)
                    {
                        layoutNames.Add(layout.LayoutName);
                    }

                    // Output the retrieved layout names
                    Console.WriteLine("Available Layouts:");
                    foreach (string name in layoutNames)
                    {
                        Console.WriteLine("- " + name);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}