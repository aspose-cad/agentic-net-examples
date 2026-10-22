using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Placeholder path for the DXB file
        string inputPath = "input.dxb";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Configure load options to ignore non‑critical errors
            var loadOptions = new LoadOptions();
            loadOptions.IgnoreErrors = true;

            // Load the DXB drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath, loadOptions))
            {
                // Verify successful initialization
                if (cadImage != null && cadImage.Width > 0 && cadImage.Height > 0)
                {
                    Console.WriteLine("DXB file loaded successfully.");
                }
                else
                {
                    Console.Error.WriteLine("Failed to initialize DXB drawing.");
                }
            }
        }
        catch (Exception ex)
        {
            // Output any loading errors
            Console.Error.WriteLine($"Error loading DXB file: {ex.Message}");
        }
    }
}