using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD;

class Program
{
    static void Main()
    {
        try
        {
            // Define input CAD file path (placeholder)
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Extract all strings from the CAD drawing
                string[] allStrings = cadImage.GetStrings();

                // Define keywords to filter
                string[] keywords = new string[] { "Door", "Window" };

                // Filter strings containing any of the keywords
                List<string> filteredStrings = new List<string>();
                foreach (string str in allStrings)
                {
                    foreach (string keyword in keywords)
                    {
                        if (str != null && str.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        {
                            filteredStrings.Add(str);
                            break;
                        }
                    }
                }

                // Prepare output directory
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Write filtered strings to a text file
                string outputPath = Path.Combine(outputDir, "filtered_strings.txt");
                using (StreamWriter writer = new StreamWriter(outputPath, false))
                {
                    foreach (string line in filteredStrings)
                    {
                        writer.WriteLine(line);
                    }
                }

                Console.WriteLine($"Filtered strings written to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}