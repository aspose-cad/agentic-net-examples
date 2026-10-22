using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source CAD file
                string inputPath = "input.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path for the output PNG file
                string outputPath = "output.png";

                // Load the CAD image
                Image cadImage = Image.Load(inputPath);

                // Cast to CadImage to access layout information
                CadImage cadSpecific = cadImage as CadImage;
                if (cadSpecific == null)
                {
                    Console.WriteLine("Failed to load CAD image as CadImage.");
                    return;
                }

                // Retrieve the name (key) of the first layout
                string firstLayoutName = null;
                foreach (KeyValuePair<string, CadLayout> kvp in cadSpecific.Layouts)
                {
                    firstLayoutName = kvp.Key;
                    break;
                }

                if (firstLayoutName == null)
                {
                    Console.WriteLine("No layouts found in the CAD file.");
                    return;
                }

                // Configure rasterization options for the selected layout
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.Layouts = new string[] { firstLayoutName };
                rasterOptions.PageWidth = 1600;   // optional: set desired width
                rasterOptions.PageHeight = 1200;  // optional: set desired height
                rasterOptions.UnitType = UnitType.Point;

                // Set PNG export options
                PngOptions pngOptions = new PngOptions();
                pngOptions.VectorRasterizationOptions = rasterOptions;

                // Save the image as PNG
                cadImage.Save(outputPath, pngOptions);

                // Verify that the PNG file was created
                if (File.Exists(outputPath))
                {
                    Console.WriteLine("PNG file created successfully: " + outputPath);
                }
                else
                {
                    Console.WriteLine("Failed to create PNG file.");
                }

                // Clean up
                cadImage.Dispose();
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}