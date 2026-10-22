using Aspose.CAD.FileFormats.Cad;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;

namespace CadStringFrequency
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input CAD file path (placeholder)
                string inputFilePath = "input.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Load the CAD image as CadImage
                using (CadImage cadImage = (CadImage)Image.Load(inputFilePath))
                {
                    // Retrieve all text strings from the drawing
                    IList<string> textStrings = cadImage.GetStrings();

                    // Count occurrences of each unique string
                    Dictionary<string, int> frequencyMap = new Dictionary<string, int>(StringComparer.Ordinal);
                    foreach (string txt in textStrings)
                    {
                        if (frequencyMap.ContainsKey(txt))
                        {
                            frequencyMap[txt] += 1;
                        }
                        else
                        {
                            frequencyMap[txt] = 1;
                        }
                    }

                    // Output the frequency analysis
                    Console.WriteLine("String Frequency Analysis:");
                    foreach (KeyValuePair<string, int> entry in frequencyMap)
                    {
                        Console.WriteLine($"\"{entry.Key}\" : {entry.Value}");
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