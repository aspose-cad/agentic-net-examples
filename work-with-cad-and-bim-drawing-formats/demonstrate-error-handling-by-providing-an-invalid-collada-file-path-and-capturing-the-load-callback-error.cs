using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Path to a deliberately invalid COLLADA file.
        string inputPath = "invalid_model.dae";

        // Ensure the file exists to satisfy the placeholder rule.
        // The content is not a valid COLLADA document, so loading will fail.
        if (!File.Exists(inputPath))
        {
            File.WriteAllText(inputPath, "This is not a valid COLLADA file.");
        }

        // Configure load options. IgnoreErrors = false forces the loader to report problems.
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = false
        };

        try
        {
            // Attempt to load the file as a CAD image.
            // The cast to CadImage is safe when the format is recognized.
            using (Image cadImage = Image.Load(inputPath, loadOptions))
            {
                Console.WriteLine("File loaded successfully (unexpected).");
            }
        }
        catch (Exception ex)
        {
            // Capture any exception thrown during loading.
            Console.WriteLine($"Load exception: {ex.Message}");
        }

        // After the load attempt, inspect the Errors collection for detailed diagnostics.
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load errors reported by Aspose.CAD:");
            foreach (RenderResult error in loadOptions.Errors)
            {
                Console.WriteLine($"- Code: {error.RenderCode}, Message: {error.Message}");
            }
        }
        else
        {
            Console.WriteLine("No load errors reported.");
        }
    }
}