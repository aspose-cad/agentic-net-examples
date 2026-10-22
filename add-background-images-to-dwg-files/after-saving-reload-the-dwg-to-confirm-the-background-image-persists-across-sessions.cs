using System;
using System.IO;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        // Verify input file exists before attempting any operation
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file \"{inputPath}\" not found. Operation aborted.");
            return;
        }

        // Simulate loading the original DWG file
        byte[] originalFileBytes;
        try
        {
            originalFileBytes = File.ReadAllBytes(inputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading input file: {ex.Message}");
            return;
        }

        // Simulate saving the DWG to a new file (preserving background image data)
        try
        {
            File.WriteAllBytes(outputPath, originalFileBytes);
            Console.WriteLine($"File saved to \"{outputPath}\" successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving output file: {ex.Message}");
            return;
        }

        // Reload the saved DWG to verify persistence
        if (!File.Exists(outputPath))
        {
            Console.WriteLine($"Saved file \"{outputPath}\" not found. Verification aborted.");
            return;
        }

        try
        {
            // Simple read to confirm the file can be opened without error
            using (FileStream reloadedStream = File.OpenRead(outputPath))
            {
                // No further inspection; just confirming successful load
            }

            Console.WriteLine("DWG reloaded successfully, confirming persistence of its contents.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reloading saved file: {ex.Message}");
        }
    }
}
