using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class ObjToPdfWithErrorCallback
{
    static void Main()
    {
        try
        {
                // Input OBJ file path
                string inputPath = "sample.obj";
                // Output PDF file path
                string outputPath = "result.pdf";

                // Validate that the input file exists before attempting to load it
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Configure load options to ignore errors during loading
                var loadOptions = new LoadOptions
                {
                    IgnoreErrors = true
                };

                // Load the OBJ drawing
                using (Image cadImage = Image.Load(inputPath, loadOptions))
                {
                    // Prepare rasterization options for vector to raster conversion
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1000,
                        PageHeight = 1000,
                        UnitType = UnitType.Centimenter, // SDK spelling
                        LineScale = 1f,
                        AutomaticLayoutsScaling = true
                    };

                    // Attach a callback to capture rendering errors
                    rasterOptions.RenderResult = result =>
                    {
                        Console.WriteLine($"Render complete: {result.IsRenderComplete}");
                        foreach (var failure in result.Failures)
                        {
                            Console.WriteLine($"Failure: {failure.Message} (Code: {failure.RenderCode})");
                        }
                    };

                    // Set up PDF export options and assign the rasterization options
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the drawing as PDF; any rendering errors will be reported via the callback
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"Export finished. PDF saved to: {outputPath}");
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}