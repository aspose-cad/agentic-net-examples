using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Validate input argument
            if (args == null || args.Length == 0 || string.IsNullOrEmpty(args[0]))
            {
                Console.Error.WriteLine("Please provide the path to a CAD file as the first argument.");
                return;
            }

            string inputPath = args[0];
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = Path.Combine(Path.GetDirectoryName(inputPath) ?? "", "Output");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load CAD image
            using (Image img = Image.Load(inputPath))
            {
                // Cast to CadImage to access objects
                if (img is not CadImage cadImage)
                {
                    Console.Error.WriteLine("The provided file is not a supported CAD image.");
                    return;
                }

                // Detect missing external image references
                bool missingReference = false;
                foreach (var rasterDef in cadImage.Objects.OfType<CadRasterImageDef>())
                {
                    string externalPath = rasterDef.FileName;
                    if (!string.IsNullOrEmpty(externalPath) && !File.Exists(externalPath))
                    {
                        Console.Error.WriteLine($"Missing external image reference: {externalPath}");
                        missingReference = true;
                    }
                }

                if (missingReference)
                {
                    Console.Error.WriteLine("Conversion aborted due to missing external image references.");
                    return;
                }

                // Set rasterization options (example: default layout, white background is default)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200
                };

                // Prepare PNG save options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Build output file name
                string outputFile = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".png");

                // Save as PNG
                cadImage.Save(outputFile, pngOptions);
                Console.WriteLine($"File saved to: {outputFile}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}