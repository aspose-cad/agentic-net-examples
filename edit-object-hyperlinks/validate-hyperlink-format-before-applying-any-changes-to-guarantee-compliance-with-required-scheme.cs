using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        string sourcePath = "input.dwg";
        string outputPath = "output.png";

        if (!File.Exists(sourcePath))
        {
            Console.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            // Load the CAD file
            CadImage cadImage = (CadImage)Image.Load(sourcePath);

            // Validate hyperlinks in all entities
            foreach (CadEntityBase entity in cadImage.Entities)
            {
                // Attempt to get a Hyperlink property via reflection
                PropertyInfo hyperlinkProp = entity.GetType().GetProperty("Hyperlink", BindingFlags.Public | BindingFlags.Instance);
                if (hyperlinkProp != null && hyperlinkProp.PropertyType == typeof(string))
                {
                    string link = hyperlinkProp.GetValue(entity) as string;
                    if (!IsValidHyperlink(link))
                    {
                        Console.WriteLine($"Invalid hyperlink '{link}' in entity type {entity.GetType().Name}");
                        // Additional handling (e.g., removal or correction) can be placed here
                    }
                }
            }

            // Save the CAD image as PNG
            PngOptions pngOptions = new PngOptions();
            cadImage.Save(outputPath, pngOptions);
            Console.WriteLine($"Image saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static bool IsValidHyperlink(string hyperlink)
    {
        if (string.IsNullOrWhiteSpace(hyperlink))
            return false;

        if (Uri.TryCreate(hyperlink, UriKind.Absolute, out Uri uriResult))
        {
            return uriResult.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) ||
                   uriResult.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }
}