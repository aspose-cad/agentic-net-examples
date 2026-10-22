using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Inch,               // Set measurement unit
                LineScale = 1.0f,                       // Scale of line widths
                PageWidth = 8.5f,                       // Width of the output page
                PageHeight = 11.0f,                     // Height of the output page
                AutomaticLayoutsScaling = true,        // Enable automatic scaling of layouts
                Layouts = new string[] { "Model" }     // Export only the specified layout(s)
            };

            // Set margins (integer values)
            rasterOptions.Margins = new Margins
            {
                Left = 0,
                Right = 0,
                Top = 0,
                Bottom = 0
            };

            // Set up PDF save options and attach rasterization settings
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the CAD drawing as PDF
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine($"Conversion completed successfully. Output saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}