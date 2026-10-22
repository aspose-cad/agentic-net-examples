using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadExportSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Verify command‑line arguments
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: CadExportSample <input‑dxf‑file> <output‑directory>");
                return;
            }

            string inputFilePath = args[0];
            string outputDirectory = args[1];

            // Validate input file
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Error: Input file \"{inputFilePath}\" does not exist.");
                return;
            }

            // Validate (or create) output directory
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: Unable to create output directory \"{outputDirectory}\". {ex.Message}");
                    return;
                }
            }

            try
            {
                // Load the DXF file as a CadImage
                CadImage cadImage = (CadImage)Image.Load(inputFilePath);

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.PageWidth = 1600;   // Desired bitmap width
                rasterOptions.PageHeight = 1200;  // Desired bitmap height

                // NOTE: BackgroundColor and DrawColor properties require System.Drawing.Color,
                // which is prohibited in this environment. Therefore they are left at default values.

                // Set up BMP export options
                BmpOptions bmpOptions = new BmpOptions();
                bmpOptions.VectorRasterizationOptions = rasterOptions;

                // Build output file path
                string outputFileName = Path.GetFileNameWithoutExtension(inputFilePath) + ".bmp";
                string outputFilePath = Path.Combine(outputDirectory, outputFileName);

                // Save the image as BMP
                cadImage.Save(outputFilePath, bmpOptions);

                Console.WriteLine($"Successfully exported \"{inputFilePath}\" to \"{outputFilePath}\".");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during conversion: {ex.Message}");
            }
        }
    }
}