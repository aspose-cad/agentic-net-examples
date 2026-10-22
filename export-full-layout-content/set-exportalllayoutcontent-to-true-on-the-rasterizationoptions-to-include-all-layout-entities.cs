using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        string inputPath;
        string outputPath;

        if (args.Length >= 2)
        {
            inputPath = args[0];
            outputPath = args[1];
        }
        else
        {
            Console.Error.WriteLine("Usage: program <input CAD file> <output PDF file>");
            return;
        }

        if (string.IsNullOrWhiteSpace(inputPath) || !File.Exists(inputPath))
        {
            Console.Error.WriteLine("Input file does not exist: " + inputPath);
            return;
        }

        string outputDirectory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
        {
            try
            {
                Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to create output directory: " + ex.Message);
                return;
            }
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Configure rasterization to export all layout content
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.ExportAllLayoutContent = true;
                rasterOptions.PageWidth = 1600;
                rasterOptions.PageHeight = 1200;
                rasterOptions.Layouts = null; // null means all layouts

                // Set up PDF save options with the rasterization settings
                PdfOptions pdfOptions = new PdfOptions();
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the CAD file as PDF with all layout entities included
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine("Export completed successfully: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error during export: " + ex.Message);
        }
    }
}