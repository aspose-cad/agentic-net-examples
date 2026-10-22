using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwf";
            string outputPath = "output.bmp";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                BmpOptions bmpOptions = new BmpOptions();
                // Default BMP options are used

                cadImage.Save(outputPath, bmpOptions);
            }

            Console.WriteLine($"Successfully exported to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}