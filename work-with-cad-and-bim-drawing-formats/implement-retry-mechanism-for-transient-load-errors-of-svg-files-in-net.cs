using System;
using System.IO;
using System.Linq;
using System.Threading;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class SvgLoadRetryExample
{
    static void Main()
    {
        // Path to the source SVG file
        string svgPath = @"C:\Input\example.svg";
        // Path to the output PNG file
        string pngPath = @"C:\Output\example.png";

        // Validate input file existence
        if (!File.Exists(svgPath))
        {
            Console.WriteLine($"Input file not found: {svgPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(pngPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        const int maxRetries = 3;
        int attempt = 0;
        bool succeeded = false;

        while (attempt < maxRetries && !succeeded)
        {
            try
            {
                // Configure load options – ignore errors to allow inspection after load
                var loadOptions = new LoadOptions
                {
                    IgnoreErrors = true
                };

                // Load the SVG file
                using (Image image = Image.Load(svgPath, loadOptions))
                {
                    // If any load errors were reported, treat them as transient and retry
                    if (loadOptions.Errors != null && loadOptions.Errors.Any())
                    {
                        throw new IOException("Load completed with errors; retrying.");
                    }

                    // Prepare rasterization options for PNG export
                    var rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter,
                        LineScale = 1.0f,
                        PageWidth = 800,
                        PageHeight = 600,
                        AutomaticLayoutsScaling = true,
                        Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                    };

                    // Set up PNG export options
                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the image as PNG
                    image.Save(pngPath, pngOptions);
                }

                // If we reach this point, loading and saving succeeded
                succeeded = true;
                Console.WriteLine("SVG loaded and saved successfully.");
            }
            catch (Exception ex)
            {
                attempt++;
                Console.WriteLine($"Attempt {attempt} failed: {ex.Message}");

                if (attempt >= maxRetries)
                {
                    Console.WriteLine("Maximum retry attempts reached. Operation aborted.");
                    throw;
                }

                // Optional back‑off before the next retry
                Thread.Sleep(1000);
            }
        }
    }
}