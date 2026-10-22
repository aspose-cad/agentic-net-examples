using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace PltToPdfRenderer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Validate arguments.
            if (args == null || args.Length == 0 || string.IsNullOrEmpty(args[0]))
            {
                Console.Error.WriteLine("Error: No input file path provided.");
                return;
            }

            string inputPath = args[0];

            // Verify that the source file exists.
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file \"{inputPath}\" does not exist.");
                return;
            }

            // Determine output directory and file name.
            string outputDirectory = Path.GetDirectoryName(inputPath);
            if (string.IsNullOrEmpty(outputDirectory))
            {
                Console.Error.WriteLine("Error: Unable to determine output directory.");
                return;
            }

            // Ensure the output directory exists.
            Directory.CreateDirectory(outputDirectory);

            string outputFileName = Path.GetFileNameWithoutExtension(inputPath) + ".pdf";
            string outputPath = Path.Combine(outputDirectory, outputFileName);

            try
            {
                // Load the PLT file as a generic Aspose.CAD.Image.
                using (Image genericImage = Image.Load(inputPath))
                {
                    // Cast the generic image to a CadImage for CAD‑specific operations.
                    CadImage cadImage = genericImage as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Error: Loaded file is not a recognized CAD image.");
                        return;
                    }

                    // Configure rasterization options: set a white background.
                    // Note: The Aspose.CAD API does not expose an AntiAliasing property; anti‑aliasing cannot be explicitly enabled via this SDK version.
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        BackgroundColor = Aspose.CAD.Color.White
                    };

                    // Create PDF save options and attach the rasterization settings.
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the CAD image as a PDF file.
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Conversion successful. PDF saved to \"{outputPath}\".");
            }
            catch (Exception ex)
            {
                // Write a concise error message without exposing a stack trace.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}