using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD file path (replace with actual path)
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"File not found: {inputPath}");
                return;
            }

            // Expected number of layouts (set to the value you expect)
            int expectedCount = 2;

            // Load the CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access layout information
                CadImage cadImg = cadImage as CadImage;
                if (cadImg == null)
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                    return;
                }

                // Retrieve the collection of layouts (read‑only dictionary)
                IReadOnlyDictionary<string, CadLayout> layoutDict = cadImg.Layouts;

                // Get the actual layout count
                int actualCount = layoutDict.Count;

                // Verify the count against the expected value
                if (actualCount == expectedCount)
                {
                    Console.WriteLine($"Layout count verification passed. Count: {actualCount}");
                }
                else
                {
                    Console.WriteLine($"Layout count verification failed. Expected: {expectedCount}, Actual: {actualCount}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}