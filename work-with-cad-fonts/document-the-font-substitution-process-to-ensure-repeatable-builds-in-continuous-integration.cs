using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace FontSubstitutionExample
{
    class Program
    {
        static void Main()
        {
            // Define input and output paths (adjust as needed)
            string inputPath = "input.dxf";
            string outputPath = "output.pdf";

            // Verify that the input CAD file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD file using Aspose.CAD
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Specify a fallback font to be used when the original font is missing.
                    // This ensures repeatable builds in CI environments where the original font may not be installed.
                    // Provide the full path to a known font file (e.g., Arial.ttf) that is available on the build agents.
                    cadImage.DefaultFont = @"C:\Windows\Fonts\Arial.ttf";

                    // Optionally, you can inspect the layouts or entities here if needed.
                    // foreach (var layout in cadImage.Layouts.Values)
                    // {
                    //     Console.WriteLine($"Layout: {layout.Name}");
                    // }

                    // Save the CAD image to a PDF file (or any other supported format).
                    // The PdfOptions does not require rasterization options for font substitution.
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"CAD file saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}