using System;
using System.IO;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" }
                };

                // Retrieve the PenOptions from the rasterization options
                PenOptions penOptions = rasterOptions.PenOptions;

                // Inspect pen configuration via reflection
                Console.WriteLine("PenOptions properties:");
                foreach (PropertyInfo prop in penOptions.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    try
                    {
                        object value = prop.GetValue(penOptions);
                        Console.WriteLine($"  {prop.Name}: {value}");
                    }
                    catch
                    {
                        Console.WriteLine($"  {prop.Name}: <unavailable>");
                    }
                }

                // Set up PNG output options using the rasterization options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rasterized image
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Rasterized image saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}