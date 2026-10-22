using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the CAD drawing file (replace with actual path)
            string filePath = "sample.dwg";

            // Verify the input file exists
            if (!File.Exists(filePath))
            {
                Console.Error.WriteLine($"Input file not found: {filePath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (Image image = Image.Load(filePath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Retrieve layout names from the drawing
                var layoutNames = new List<string>();
                foreach (var layout in cadImage.Layouts.Values)
                {
                    // Attempt to read the Name property of the layout object
                    var nameProp = layout.GetType().GetProperty("Name");
                    string name = nameProp?.GetValue(layout) as string ?? "UnnamedLayout";
                    layoutNames.Add(name);
                }

                Console.WriteLine("Layouts found in the drawing:");
                foreach (string layoutName in layoutNames)
                {
                    Console.WriteLine("- " + layoutName);
                }

                // Simple validation: ensure at least one layout is present
                if (layoutNames.Count == 0)
                {
                    Console.WriteLine("No layouts found. Validation failed.");
                }
                else
                {
                    Console.WriteLine("Layout validation passed.");
                }

                // Render each layout to PNG to confirm 3D entities are processed correctly
                foreach (string layoutName in layoutNames)
                {
                    // Configure rasterization options for the specific layout
                    var rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new[] { layoutName },
                        DrawType = CadDrawTypeMode.UseObjectColor,
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    // Set PNG save options
                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save rendered layout as PNG
                    string outputPath = Path.Combine(outputDir, $"output_{layoutName}.png");
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Rendered layout '{layoutName}' to {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}