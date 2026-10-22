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
            try
            {
                // Define input and output file paths
                string inputPath = "input.dwf";
                string outputPath = "output.bmp";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    throw new FileNotFoundException($"Input file not found: {inputPath}");
                }

                // Load the DWF drawing
                using (Image dwfImage = Image.Load(inputPath))
                {
                    // Set BMP save options
                    BmpOptions bmpOptions = new BmpOptions();

                    // Save the drawing as BMP
                    dwfImage.Save(outputPath, bmpOptions);
                }

                Console.WriteLine("Conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}