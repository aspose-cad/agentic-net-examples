using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Paths for input FBX and output PDF
        string inputPath = "sample.fbx";
        string outputPath = "output.pdf";

        // Validate input file existence
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Prepare load options (ignore errors to continue loading)
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage = null;
        try
        {
            // Load the FBX drawing
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during load: {ex.Message}");
        }

        // Log any load errors collected in loadOptions.Errors
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load errors:");
            foreach (var err in loadOptions.Errors)
            {
                // err is of type Aspose.CAD.ImageOptions.RenderResult
                Console.WriteLine($"- {err.Message}");
            }
        }

        if (cadImage == null)
        {
            Console.WriteLine("Failed to load CAD image. Export aborted.");
            return;
        }

        // Configure rasterization options with a render result callback
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            LineScale = 0.5f,
            Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
        };

        // Assign render result callback to capture rendering issues
        rasterOptions.RenderResult = result =>
        {
            if (!result.IsRenderComplete)
            {
                Console.WriteLine("Rendering was not completed.");
                if (result.Failures != null && result.Failures.Count > 0)
                {
                    foreach (var fail in result.Failures)
                    {
                        Console.WriteLine($"Render failure: {fail.Message}");
                    }
                }
            }
        };

        // Set up PDF export options using the rasterization settings
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Export the drawing to PDF
        cadImage.Save(outputPath, pdfOptions);
        Console.WriteLine($"PDF exported successfully to: {outputPath}");
    }
}