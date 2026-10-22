using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dae";
        string outputPath = "output.pdf";

        // Verify that the input file exists before attempting to load it
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Load the CAD image with load options that ignore errors but capture them
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage;
        try
        {
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during loading: {ex.Message}");
            return;
        }

        // Log any load-time errors or warnings captured in LoadOptions.Errors
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load errors/warnings:");
            foreach (var err in loadOptions.Errors)
            {
                // RenderResult provides RenderCode and Message
                Console.WriteLine($"- Code: {err.RenderCode}, Message: {err.Message}");
            }
        }

        // Configure rasterization options and attach an export callback
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter, // SDK spelling
            PageWidth = 210,   // A4 width in millimeters
            PageHeight = 297,  // A4 height in millimeters
            LineScale = 0.5f,
            RenderResult = result =>
            {
                Console.WriteLine($"Render completed: {result.IsRenderComplete}");
                if (result.Failures != null && result.Failures.Count > 0)
                {
                    Console.WriteLine("Render failures:");
                    foreach (var fail in result.Failures)
                    {
                        // Each failure is a RenderResult with RenderCode and Message
                        Console.WriteLine($"- Code: {fail.RenderCode}, Message: {fail.Message}");
                    }
                }
            }
        };

        // Set PDF options and assign the rasterization options
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the CAD image as PDF
        cadImage.Save(outputPath, pdfOptions);

        Console.WriteLine($"PDF saved to {outputPath}");
    }
}