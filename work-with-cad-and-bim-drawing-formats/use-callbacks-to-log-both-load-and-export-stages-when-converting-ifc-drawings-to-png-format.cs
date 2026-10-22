using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class IfcToPngConverter
{
    static void Main()
    {
        string inputPath = "input.ifc";
        string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string? outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options with error collection enabled
        LoadOptions loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage = null;

        // Load the IFC file and capture any load‑time errors
        try
        {
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Load exception: {ex.Message}");
        }
        finally
        {
            // Log all load errors collected in LoadOptions.Errors
            foreach (var loadError in loadOptions.Errors)
            {
                Console.Error.WriteLine($"Load error {loadError.RenderCode}: {loadError.Message}");
            }
        }

        if (cadImage == null)
        {
            Console.Error.WriteLine("Failed to load the IFC drawing.");
            return;
        }

        // Configure rasterization options and attach a render‑result callback
        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1200,
            Layouts = new string[] { },

            RenderResult = result =>
            {
                if (!result.IsRenderComplete)
                {
                    Console.Error.WriteLine("Rendering completed with missing elements.");
                }

                foreach (var failure in result.Failures)
                {
                    Console.Error.WriteLine($"Render failure {failure.RenderCode}: {failure.Message}");
                }
            }
        };

        // Set PNG output options using the rasterization settings above
        PngOptions pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the image to PNG and capture any export‑time exceptions
        try
        {
            cadImage.Save(outputPath, pngOptions);
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