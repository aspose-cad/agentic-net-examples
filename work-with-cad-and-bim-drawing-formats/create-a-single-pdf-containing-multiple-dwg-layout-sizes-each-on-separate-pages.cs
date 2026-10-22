using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        // Expect the source DWG file path as the first argument.
        if (args.Length == 0)
        {
            Console.Error.WriteLine("Usage: Program <input-dwg-file> [output-pdf-file]");
            return;
        }

        string inputPath = args[0];
        string outputPath = args.Length > 1 ? args[1] : Path.ChangeExtension(inputPath, ".pdf");

        // Validate input file existence.
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file does not exist: {inputPath}");
            return;
        }

        // Ensure the output directory exists.
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }
        }

        try
        {
            // Load the DWG file using Aspose.CAD.
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access layout information.
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                    return;
                }

                // Retrieve all layout names present in the DWG file.
                string[] layoutNames = cadImage.Layouts
                    .Select(kvp => kvp.Key)
                    .ToArray();

                if (layoutNames.Length == 0)
                {
                    Console.Error.WriteLine("No layouts found in the DWG file.");
                    return;
                }

                // Configure rasterization options.
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Provide the list of layout names – each will become a separate PDF page.
                    Layouts = layoutNames,
                    AutomaticLayoutsScaling = true
                };

                // Configure PDF export options.
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the combined PDF.
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Successfully exported {layoutNames.Length} layout(s) to PDF: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}