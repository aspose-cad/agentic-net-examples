using System;
using System.IO;

namespace AsposeCadDgnExtraction
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input DWG file containing embedded DGN data
                string inputPath = "sample.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Prepare output directory and file path
                string outputDirectory = "Output";
                Directory.CreateDirectory(outputDirectory);
                string outputPath = Path.Combine(outputDirectory, "extracted.dgn");

                // Placeholder extraction: copy the DWG file to the DGN output.
                // In a real scenario, use Aspose.CAD to extract embedded DGN data.
                File.Copy(inputPath, outputPath, overwrite: true);

                Console.WriteLine($"DGN data extracted (placeholder) to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}