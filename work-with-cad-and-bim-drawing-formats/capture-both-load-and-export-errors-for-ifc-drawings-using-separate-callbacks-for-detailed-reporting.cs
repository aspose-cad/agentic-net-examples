using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the IFC file
                string inputPath = "sample.ifc";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Configure load options to ignore errors and collect them
                var loadOptions = new LoadOptions
                {
                    IgnoreErrors = true
                };

                // Load the IFC drawing
                Image cadImage = Image.Load(inputPath, loadOptions);

                // Report any errors that occurred during loading
                if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                {
                    Console.WriteLine("Load errors:");
                    foreach (var err in loadOptions.Errors)
                    {
                        // RenderResult provides RenderCode and Message for each error
                        Console.WriteLine($"Code: {err.RenderCode}, Message: {err.Message}");
                    }
                }

                // Set up rasterization options for export
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 0.5f
                };

                // Export callback to capture rendering errors
                rasterOptions.RenderResult = (CadRenderResult renderResult) =>
                {
                    if (!renderResult.IsRenderComplete)
                    {
                        Console.WriteLine("Export rendering errors:");
                        foreach (var failure in renderResult.Failures)
                        {
                            Console.WriteLine($"Code: {failure.RenderCode}, Message: {failure.Message}");
                        }
                    }
                };

                // Configure PNG export options and assign rasterization settings
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Path for the exported PNG file
                string outputPath = "output.png";

                // Save the drawing to PNG, invoking the export callback if needed
                cadImage.Save(outputPath, pngOptions);

                Console.WriteLine($"Export completed: {outputPath}");
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}