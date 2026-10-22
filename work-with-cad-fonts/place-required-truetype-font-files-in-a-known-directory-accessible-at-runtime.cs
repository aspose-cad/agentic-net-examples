using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadFontExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Paths (adjust as needed)
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";
            string fontsDirectory = @"C:\TrueTypeFonts";

            try
            {
                // Validate input CAD file
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Validate fonts directory
                if (!Directory.Exists(fontsDirectory))
                {
                    Console.Error.WriteLine($"Fonts directory not found: {fontsDirectory}");
                    return;
                }

                // Load the CAD image
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD‑specific members
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Set the default TrueType font path (Aspose.CAD uses this when a font is missing)
                    // You can point to a specific font file or a directory; here we provide the directory.
                    cadImage.DefaultFont = fontsDirectory;

                    // Save the CAD file preserving editable objects (convert mode keeps geometry as lines)
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath, saveOptions);
                }

                Console.WriteLine($"CAD file saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}