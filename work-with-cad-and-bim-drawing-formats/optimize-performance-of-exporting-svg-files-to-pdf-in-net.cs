using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace SvgToPdfPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            // Expect the first argument to be the path of the source SVG file.
            // If no argument is supplied, inform the user and exit gracefully.
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Usage: SvgToPdfPerformance <source-svg-file>");
                return;
            }

            string sourcePath = args[0];

            // Verify that the source file exists before attempting to load it.
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Prepare an output directory named \"output\" in the current working folder.
            // The directory is created if it does not already exist.
            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "output");
            try
            {
                Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Build the full path for the resulting PDF file.
            string outputFileName = Path.GetFileNameWithoutExtension(sourcePath) + ".pdf";
            string outputPath = Path.Combine(outputDirectory, outputFileName);

            try
            {
                // Load the SVG file using Aspose.CAD.Image.Load.
                // LoadOptions are configured to ignore non‑critical errors.
                LoadOptions loadOptions = new LoadOptions { IgnoreErrors = true };
                using (Image cadImage = Image.Load(sourcePath, loadOptions))
                {
                    // Cast to CadImage to gain access to CAD‑specific properties if needed.
                    CadImage cad = cadImage as CadImage;
                    if (cad == null)
                    {
                        Console.Error.WriteLine("Loaded image is not a CAD image. Export aborted.");
                        return;
                    }

                    // Configure rasterization options aimed at performance.
                    // Lower line scale reduces the amount of detail rendered.
                    // Automatic layout scaling is disabled to avoid extra calculations.
                    // Margins are set to zero to eliminate unnecessary whitespace.
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.UnitType = UnitType.Centimenter; // Use centimenter units.
                    rasterOptions.LineScale = 0.5f;                // Thinner lines for faster rendering.
                    rasterOptions.AutomaticLayoutsScaling = false;
                    rasterOptions.Margins = new Margins();
                    rasterOptions.Margins.Left = 0;
                    rasterOptions.Margins.Right = 0;
                    rasterOptions.Margins.Top = 0;
                    rasterOptions.Margins.Bottom = 0;

                    // Optionally, limit rasterization to specific layouts if the source contains many.
                    // Leaving Layouts null processes all layouts, which is acceptable for most SVG files.
                    // rasterOptions.Layouts = new string[] { "Model" };

                    // Prepare PDF save options and attach the rasterization settings.
                    PdfOptions pdfOptions = new PdfOptions();
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Perform the conversion and save the result.
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"SVG successfully exported to PDF: {outputPath}");
            }
            catch (Exception ex)
            {
                // Any exception during loading, processing, or saving is caught here.
                // The error message is written to the standard error stream.
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
            }
        }
    }
}