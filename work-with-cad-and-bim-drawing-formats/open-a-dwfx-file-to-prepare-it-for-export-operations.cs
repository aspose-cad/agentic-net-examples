using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.dwf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            var loadOptions = new LoadOptions();
            loadOptions.IgnoreErrors = true;

            using (Image cadImage = Image.Load(inputPath, loadOptions))
            {
                // The DWFx file is now loaded and ready for export operations.
                Console.WriteLine($"Loaded DWFx file successfully. Dimensions: {cadImage.Width}x{cadImage.Height}");
                // Export logic can be added here using appropriate ImageOptions.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error loading DWFx file: {ex.Message}");
        }
    }
}