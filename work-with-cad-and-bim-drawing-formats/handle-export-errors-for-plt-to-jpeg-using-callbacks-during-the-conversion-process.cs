using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "sample.plt";
        string outputPath = "output.jpg";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        // Load options: ignore errors but collect them
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage = null;
        try
        {
            // Load the PLT file as a CadImage
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load CAD file: {ex.Message}");
            return;
        }

        // Report any load errors captured in LoadOptions
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load errors:");
            foreach (var err in loadOptions.Errors)
            {
                Console.WriteLine($"- {err}");
            }
        }

        // Configure rasterization options with a render callback
        var rasterOptions = new CadRasterizationOptions
        {
            // Example settings
            UnitType = UnitType.Centimenter, // SDK spelling
            LineScale = 1.0f,

            // Callback invoked after rendering
            RenderResult = result =>
            {
                if (!result.IsRenderComplete)
                {
                    Console.WriteLine("Rendering did not complete.");
                }

                if (result.Failures != null && result.Failures.Count > 0)
                {
                    Console.WriteLine("Render failures:");
                    foreach (var failure in result.Failures)
                    {
                        Console.WriteLine($"- {failure}");
                    }
                }
            }
        };

        // JPEG output options
        var jpegOptions = new JpegOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the image as JPEG
        try
        {
            cadImage.Save(outputPath, jpegOptions);
            Console.WriteLine($"Image successfully saved to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save JPEG: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}