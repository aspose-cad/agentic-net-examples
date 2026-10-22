using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input IFC file and output PNG file paths
        string sourcePath = "input.ifc";
        string outputPath = "output.png";

        // Verify source file exists
        if (!File.Exists(sourcePath))
        {
            Console.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options – do not ignore errors so they can be inspected later
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = false
        };

        Image cadImage = null;
        try
        {
            // Load the IFC drawing using the specified load options
            cadImage = Image.Load(sourcePath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception while loading the file: {ex.Message}");
        }

        // After loading, examine any errors collected in LoadOptions.Errors
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load errors detected:");
            foreach (var err in loadOptions.Errors)
            {
                Console.WriteLine($"- {err.Message} (Code: {err.RenderCode})");
            }
        }

        if (cadImage == null)
        {
            Console.WriteLine("Failed to load the CAD image. Exiting.");
            return;
        }

        // Configure rasterization options for PNG export
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Millimeter,
            LineScale = 0.5f,
            PageWidth = cadImage.Width,
            PageHeight = cadImage.Height,
            AutomaticLayoutsScaling = true
        };

        // Attach a render result callback to capture rendering status
        rasterOptions.RenderResult = result =>
        {
            if (result.IsRenderComplete)
            {
                Console.WriteLine("Rendering completed successfully.");
            }
            else
            {
                Console.WriteLine("Rendering failed. Details:");
                foreach (var failure in result.Failures)
                {
                    Console.WriteLine($"- {failure.Message} (Code: {failure.RenderCode})");
                }
            }
        };

        // Set PNG output options (no watermark is applied)
        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        try
        {
            // Save the CAD image as a PNG file
            cadImage.Save(outputPath, pngOptions);
            Console.WriteLine($"PNG image saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception while saving PNG: {ex.Message}");
        }
        finally
        {
            // Release resources
            cadImage.Dispose();
        }
    }
}