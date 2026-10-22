using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static async Task Main()
    {
        // URL of the DRC file to download
        string url = "https://example.com/files/model.drc";

        // Temporary local path for the downloaded file
        string tempFile = Path.Combine(Path.GetTempPath(), "model.drc");

        const int maxRetries = 3;
        int attempt = 0;
        bool downloaded = false;

        // Download with retry logic for transient network failures
        using (HttpClient client = new HttpClient())
        {
            while (attempt < maxRetries && !downloaded)
            {
                try
                {
                    attempt++;
                    byte[] data = await client.GetByteArrayAsync(url);
                    File.WriteAllBytes(tempFile, data);
                    downloaded = true;
                }
                catch (HttpRequestException e)
                {
                    Console.Error.WriteLine($"Attempt {attempt} failed: {e.Message}");
                    if (attempt >= maxRetries)
                    {
                        Console.Error.WriteLine("Maximum retry attempts reached. Exiting.");
                        return;
                    }
                    await Task.Delay(2000);
                }
            }
        }

        // Guard: ensure the file exists before loading
        if (!File.Exists(tempFile))
        {
            Console.Error.WriteLine($"File not found after download: {tempFile}");
            return;
        }

        // Configure load options
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        CadImage cadImage = null;
        try
        {
            // Load the DRC file using Aspose.CAD
            cadImage = (CadImage)Image.Load(tempFile, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load DRC file: {ex.Message}");
            return;
        }

        // Report any load errors collected in LoadOptions.Errors
        foreach (var err in loadOptions.Errors)
        {
            Console.Error.WriteLine($"Load error {err.RenderCode}: {err.Message}");
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
                    Console.Error.WriteLine("Rendering incomplete.");
                }
                foreach (var failure in result.Failures)
                {
                    Console.Error.WriteLine($"Render failure {failure.RenderCode}: {failure.Message}");
                }
            }
        };

        // Configure PNG export options
        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        string outPath = Path.Combine(Environment.CurrentDirectory, "model.png");
        // Ensure output directory exists
        string outDir = Path.GetDirectoryName(outPath);
        if (!string.IsNullOrEmpty(outDir))
        {
            Directory.CreateDirectory(outDir);
        }

        try
        {
            // Save the rendered image
            cadImage.Save(outPath, pngOptions);
            Console.WriteLine($"Image saved to {outPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to save PNG: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();

            // Clean up the temporary file
            try { File.Delete(tempFile); } catch { }
        }
    }
}