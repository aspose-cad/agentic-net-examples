using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadConversionExample
{
    class Program
    {
        static void Main(string[] args)
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
                    cadImage.Save(outputPath, bmpOptions);
                }

                Console.WriteLine($"Conversion completed successfully. Output saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
            }
        }
    }
}