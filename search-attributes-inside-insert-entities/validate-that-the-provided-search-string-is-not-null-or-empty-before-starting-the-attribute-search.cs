using System;
using System.IO;

class CadAttributeSearch
{
    static void Main()
    {
        // Input and output file paths
        string inputPath = "input.dxf";
        string outputPath = "output.dxf";

        // Ensure the input file exists – create a minimal placeholder if missing
        try
        {
            if (!File.Exists(inputPath))
            {
                File.WriteAllText(inputPath, "0\nSECTION\n2\nHEADER\n0\nENDSEC\n0\nEOF");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to prepare input file: {ex.Message}");
            return;
        }

        // Search string to look for in attribute texts
        string searchString = "SampleAttribute";

        // Validate that the search string is not null or empty
        if (string.IsNullOrEmpty(searchString))
        {
            Console.WriteLine("Search string cannot be null or empty.");
            return;
        }

        string[] lines;
        // Read the input file
        try
        {
            lines = File.ReadAllLines(inputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to read input file: {ex.Message}");
            return;
        }

        // Process lines: simple example of appending "_Found" when the search string is present
        for (int i = 0; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]) &&
                lines[i].IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                lines[i] = lines[i] + "_Found";
            }
        }

        // Write the modified content to the output file
        try
        {
            File.WriteAllLines(outputPath, lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write output file: {ex.Message}");
            return;
        }

        Console.WriteLine($"Processing completed. Output saved to '{outputPath}'.");
    }
}