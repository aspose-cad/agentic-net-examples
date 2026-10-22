using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input and output file paths
        string sourcePath = "input.dwg";
        string outputPath = "output.jpg";

        // Verify source file exists
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

        // Configure load options
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        CadImage cadImage = null;
        try
        {
            // Load the CAD file with the specified options
            cadImage = (CadImage)Image.Load(sourcePath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load CAD file: {ex.Message}");
            // Report any load errors collected in LoadOptions
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{err.RenderCode}: {err.Message}");
            }
            return;
        }

        // Set up rasterization options with a render result callback
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1200,
            RenderResult = result =>
            {
                if (!result.IsRenderComplete)
                {
                    Console.Error.WriteLine("Rendering did not complete.");
                }

                foreach (var failure in result.Failures)
                {
                    Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                }
            }
        };

        // Configure JPEG export options
        var jpegOptions = new JpegOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        try
        {
            // Save the CAD image as JPEG
            cadImage.Save(outputPath, jpegOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to save JPEG: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}