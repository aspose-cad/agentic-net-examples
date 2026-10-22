using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Initialize a list of file paths that may contain duplicates
        List<string> filePaths = new List<string>
        {
            @"C:\Drawings\design1.dwg",
            @"C:\Drawings\design2.dwg",
            @"C:\Drawings\design1.dwg",
            @"C:\Drawings\design3.dwg",
            @"C:\Drawings\design2.dwg"
        };

        // Create a list to hold distinct paths while preserving the original order
        List<string> distinctPaths = new List<string>();
        // HashSet tracks which paths have already been encountered (case‑insensitive)
        HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Iterate through the original list and add only unseen paths to the distinct list
        foreach (string path in filePaths)
        {
            if (seen.Add(path))
            {
                distinctPaths.Add(path);
            }
        }

        // Output the resulting list of unique file paths
        foreach (string path in distinctPaths)
        {
            Console.WriteLine(path);
        }
    }
}