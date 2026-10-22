using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadLayoutIterationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source CAD file
            string inputPath = "sample.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD file using Aspose.CAD
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Retrieve the layout collection (read‑only dictionary) from the loaded image
                    IReadOnlyDictionary<string, CadLayout> layouts = cadImage.Layouts;

                    // Convert the keys to a list for indexed access
                    List<string> layoutKeys = new List<string>(layouts.Keys);

                    // Iterate through the layout collection using a standard for loop
                    for (int i = 0; i < layoutKeys.Count; i++)
                    {
                        string key = layoutKeys[i];
                        CadLayout layout = layouts[key];

                        // Output basic information about the layout
                        Console.WriteLine($"Layout #{i + 1}");
                        Console.WriteLine($"  Layout Name: {layout.LayoutName}");
                        Console.WriteLine($"  Block Table Record Handle: {layout.BlockTableRecordHandle}");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                // Print concise error information
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}