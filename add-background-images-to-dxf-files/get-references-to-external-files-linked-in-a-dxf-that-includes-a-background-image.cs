using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Path to the DXF file that may contain external image references
        string sourcePath = "input.dxf";

        // Verify that the file exists before attempting to read it
        if (!File.Exists(sourcePath))
        {
            Console.WriteLine($"File not found: {sourcePath}");
            return;
        }

        try
        {
            // Read all lines from the DXF file
            string[] lines = File.ReadAllLines(sourcePath);
            var externalImages = new List<string>();

            bool insideImageEntity = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                // Detect the start of an IMAGE entity
                if (string.Equals(line, "IMAGE", StringComparison.OrdinalIgnoreCase))
                {
                    insideImageEntity = true;
                    continue;
                }

                if (insideImageEntity)
                {
                    // In DXF, group code 1 holds the image file path for IMAGE entities
                    if (line == "1" && i + 1 < lines.Length)
                    {
                        string filePath = lines[i + 1].Trim();
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            externalImages.Add(filePath);
                        }
                        i++; // Skip the file path line we just processed
                        continue;
                    }

                    // End of the IMAGE entity is typically marked by a new entity type or a section delimiter
                    // We'll reset the flag when we encounter another known entity type (e.g., "LINE", "CIRCLE", etc.)
                    // or when we reach the end of the file.
                    if (!string.IsNullOrEmpty(line) && !char.IsDigit(line[0]))
                    {
                        // Assume a new entity starts; exit IMAGE context
                        insideImageEntity = false;
                    }
                }
            }

            if (externalImages.Count == 0)
            {
                Console.WriteLine("No external image references found.");
            }
            else
            {
                foreach (var img in externalImages)
                {
                    Console.WriteLine($"External image reference found: {img}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while processing the DXF file: {ex.Message}");
        }
    }
}
