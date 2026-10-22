using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Placeholder paths – replace with actual file locations.
        string inputFile = "sample.dwf";
        string outputFile = "sample.bmp";

        try
        {
            // Verify that the input DWF file exists.
            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"Input file not found: {inputFile}");
                return;
            }

            // Load the DWF drawing.
            using (Image cadImage = Image.Load(inputFile))
            {
                // Configure BMP export options.
                BmpOptions bmpOptions = new BmpOptions();

                // Save the drawing as BMP.
                cadImage.Save(outputFile, bmpOptions);
            }

            Console.WriteLine($"Conversion completed successfully. BMP saved to: {outputFile}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}