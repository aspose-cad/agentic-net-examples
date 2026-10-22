using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
                // List of CAD files to process
                List<string> cadFiles = new List<string>
                {
                    "sample1.dwg",
                    "sample2.dxf"
                };

                // Ensure the output directory exists
                string outputDirectory = "OutputImages";
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Iterate over each CAD file
                foreach (string cadFilePath in cadFiles)
                {
                    // Check that the input file exists
                    if (!File.Exists(cadFilePath))
                    {
                        Console.WriteLine($"Input file not found: {cadFilePath}");
                        continue;
                    }

                    // Load the CAD image within a using block to ensure proper disposal
                    using (Image cadImage = Image.Load(cadFilePath))
                    {
                        // Configure PNG export options
                        PngOptions pngOptions = new PngOptions();

                        // Determine the output file path
                        string outputFileName = Path.GetFileNameWithoutExtension(cadFilePath) + ".png";
                        string outputPath = Path.Combine(outputDirectory, outputFileName);

                        // Save the CAD image as PNG
                        cadImage.Save(outputPath, pngOptions);
                        Console.WriteLine($"Saved PNG: {outputPath}");
                    } // cadImage is disposed here; no further usage
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}