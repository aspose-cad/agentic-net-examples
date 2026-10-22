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

        // Guard source file existence
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options (do not ignore errors to collect them)
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = false
        };

        Image image = null;
        try
        {
            // Load the IFC file as a CadImage
            image = Image.Load(sourcePath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Load exception: {ex.Message}");
        }

        // Log any errors that occurred during loading
        foreach (var loadError in loadOptions.Errors)
        {
            Console.Error.WriteLine($"Load error [{loadError.RenderCode}]: {loadError.Message}");
        }

        if (image == null)
        {
            // Cannot continue without a valid image
            return;
        }

        // Configure rasterization options for PNG export
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1000,
            PageHeight = 1000,
            RenderResult = result =>
            {
                if (!result.IsRenderComplete)
                {
                    Console.Error.WriteLine("Rendering incomplete.");
                }

                foreach (var failure in result.Failures)
                {
                    Console.Error.WriteLine($"Render error [{failure.RenderCode}]: {failure.Message}");
                }
            }
        };

        // Set PNG options with the rasterization settings
        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        try
        {
            // Save the image as PNG
            image.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Save exception: {ex.Message}");
        }
        finally
        {
            image.Dispose();
        }
    }
}