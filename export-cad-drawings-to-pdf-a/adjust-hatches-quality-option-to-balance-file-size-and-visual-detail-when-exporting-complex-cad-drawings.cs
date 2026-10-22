using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Input CAD file path
        string inputPath = "input.dwg";
        // Output PDF file path
        string outputPath = "output.pdf";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure the output directory exists
        string outputDirectory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        try
        {
            // Load the CAD image
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options for balancing hatch quality and file size
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Inch,
                // Desired DPI (dots per inch)
                // Page dimensions are calculated as DPI * size in inches
                PageWidth = 8.27f * 150f,   // A4 width in inches * DPI
                PageHeight = 11.69f * 150f, // A4 height in inches * DPI
                LineScale = 0.8f,           // Values between 0.5 and 1.0 are typical
                AutomaticLayoutsScaling = true
            };

            // Set zero margins using property setters
            Margins margins = new Margins();
            margins.Left = margins.Right = margins.Top = margins.Bottom = 0;
            rasterOptions.Margins = margins;

            // Prepare PDF export options and assign the rasterization settings
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the CAD drawing as PDF
            cadImage.Save(outputPath, pdfOptions);

            Console.WriteLine($"Export completed successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}