using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder paths for input STEP file and output file with unsupported format
            string inputPath = "input.stp";
            string outputPath = "output.unsupported";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the STEP file using Aspose.CAD
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Attempt to save to an unsupported format (extension not recognized by Aspose.CAD)
                    // This should raise an exception.
                    cadImage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                // Expected error handling for unsupported export format
                Console.Error.WriteLine("Error during export:");
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}