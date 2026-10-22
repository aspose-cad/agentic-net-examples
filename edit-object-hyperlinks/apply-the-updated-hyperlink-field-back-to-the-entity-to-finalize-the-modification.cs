using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Iterate through top‑level entities to find the target entity
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Example: modify the Hyperlink of a line on layer "0"
                    if (entity is CadLine line && line.LayerName == "0")
                    {
                        // Update the Hyperlink field
                        line.Hyperlink = "https://example.com/updated";

                        // Modification applied; exit loop
                        break;
                    }
                }

                // Prepare rasterization options (required for saving CAD files)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600
                };

                // Configure save options for DWG/DXF output
                DxfOptions saveOptions = new DxfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the modified drawing
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}