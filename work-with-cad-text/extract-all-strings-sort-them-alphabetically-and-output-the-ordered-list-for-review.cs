using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AsposeCadStringExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder input CAD file path
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Collect all text strings from the drawing
                    List<string> extractedStrings = new List<string>();

                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity is CadText cadText)
                        {
                            if (!string.IsNullOrEmpty(cadText.DefaultValue))
                            {
                                extractedStrings.Add(cadText.DefaultValue);
                            }
                        }
                        else if (entity is CadMText cadMText)
                        {
                            if (!string.IsNullOrEmpty(cadMText.Text))
                            {
                                extractedStrings.Add(cadMText.Text);
                            }
                        }
                    }

                    // Sort the strings alphabetically
                    extractedStrings.Sort(StringComparer.Ordinal);

                    // Output the sorted list
                    Console.WriteLine("Extracted strings (sorted):");
                    foreach (string str in extractedStrings)
                    {
                        Console.WriteLine(str);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}