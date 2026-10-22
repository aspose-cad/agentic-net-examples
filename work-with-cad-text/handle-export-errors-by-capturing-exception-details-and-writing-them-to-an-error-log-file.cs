using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.png";
        string logPath = "error.log";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Configure load options to ignore errors during loading
        var loadOptions = new LoadOptions();
        loadOptions.IgnoreErrors = true;

        try
        {
            using (Image cadImage = Image.Load(inputPath, loadOptions))
            {
                // Set up rasterization options with a render result callback
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    RenderResult = result =>
                    {
                        if (!result.IsRenderComplete)
                        {
                            string message = $"Render incomplete. Failures: {result.Failures}";
                            File.AppendAllText(logPath, $"{DateTime.Now}: {message}{Environment.NewLine}");
                        }
                    }
                };

                // Prepare PNG export options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the CAD image to PNG
                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur during load or export
            string errorMessage = $"Exception: {ex.Message}{Environment.NewLine}{ex.StackTrace}";
            File.AppendAllText(logPath, $"{DateTime.Now}: {errorMessage}{Environment.NewLine}");
        }

        // Log any load errors captured in LoadOptions
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            foreach (var err in loadOptions.Errors)
            {
                string errMsg = $"Load error: {err}";
                File.AppendAllText(logPath, $"{DateTime.Now}: {errMsg}{Environment.NewLine}");
            }
        }
    }
}