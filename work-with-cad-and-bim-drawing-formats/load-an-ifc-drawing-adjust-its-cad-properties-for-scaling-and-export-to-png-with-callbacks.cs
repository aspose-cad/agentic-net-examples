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
        string sourcePath = "sample.ifc";
        string outputPath = "output.png";

        // Guard against missing input file
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Input file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options – ignore errors to allow loading partially corrupted files
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage = null;
        try
        {
            // Load the IFC drawing as a CAD image
            cadImage = Image.Load(sourcePath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Load exception: {ex.Message}");
        }

        // Report any load‑time errors collected in loadOptions.Errors
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"Load error {err.RenderCode}: {err.Message}");
            }
        }

        if (cadImage == null)
        {
            Console.Error.WriteLine("Failed to load the CAD image.");
            return;
        }

        // Adjust scaling – for example, double the original dimensions
        const float scaleFactor = 2.0f;

        // Configure rasterization options for PNG export
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            LineScale = 1.0f,
            PageWidth = cadImage.Size.Width * scaleFactor,
            PageHeight = cadImage.Size.Height * scaleFactor,
            RenderResult = result =>
            {
                if (!result.IsRenderComplete)
                {
                    Console.Error.WriteLine("Rendering did not complete fully.");
                }

                foreach (var failure in result.Failures)
                {
                    Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                }
            }
        };

        // Set PNG options and attach the rasterization options
        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        try
        {
            // Export the CAD image to PNG
            cadImage.Save(outputPath, pngOptions);
            Console.WriteLine($"PNG saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Save exception: {ex.Message}");
        }
        finally
        {
            cadImage.Dispose();
        }
    }
}