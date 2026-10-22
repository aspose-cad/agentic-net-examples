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
            // Path to the source CAD drawing
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Begin hyperlink editing
                // NOTE: Replace the following placeholder with actual hyperlink manipulation code
                // if the CadImage type provides a Hyperlinks collection or similar API.
                // Example (hypothetical):
                // var cadSpecific = cadImage as CadImage;
                // if (cadSpecific != null && cadSpecific.Hyperlinks != null)
                // {
                //     foreach (var link in cadSpecific.Hyperlinks)
                //     {
                //         // Edit hyperlink properties here
                //     }
                // }

                // Example of saving the edited drawing to a PNG file
                string outputPath = "output.png";
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions()
                };
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Drawing saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}