using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace AsposeCadFontSubstitution
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path (DWG, DXF, etc.)
            string inputPath = @"C:\CAD\sample.dwg";
            // Output PDF file path
            string outputPath = @"C:\CAD\sample_output.pdf";
            // Custom font file path to use when the font is not installed
            string customFontPath = @"C:\Fonts\MyCustomFont.ttf";
            // Name of the font that the CAD file expects
            string expectedFontName = "MyCustomFont";

            try
            {
                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                    return;
                }

                // Verify custom font file exists
                if (!File.Exists(customFontPath))
                {
                    Console.Error.WriteLine($"Custom font file does not exist: {customFontPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // NOTE: Checking installed system fonts typically requires System.Drawing.Text,
                    // which is prohibited in this console-only sample. Therefore, we assume the
                    // font is not installed and perform substitution unconditionally.
                    cadImage.DefaultFont = customFontPath;
                    Console.WriteLine($"Font \"{expectedFontName}\" substitution applied using custom font at: {customFontPath}");

                    // Save the CAD image to PDF
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"CAD file saved successfully to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}