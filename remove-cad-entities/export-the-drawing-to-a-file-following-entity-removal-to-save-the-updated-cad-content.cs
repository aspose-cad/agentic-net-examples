using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace CadEntityRemovalExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Paths
            string sourcePath = "input.dwg";
            string outputPath = "output.png";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (Image cadImageRaw = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)cadImageRaw;

                    // Filter out line entities on layer "0"
                    var keptEntities = cadImage.Entities
                        .Where(e => !(e is CadLine && e.LayerName == "0"))
                        .ToArray();

                    // Replace the entity collection with the filtered one
                    // (Assuming the Entities property is settable; if not, this line can be omitted)
                    cadImage.Entities = keptEntities;

                    // Set up rasterization options for exporting to PNG
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        // No Layout property; default layout will be used
                        AutomaticLayoutsScaling = true
                    };

                    // Configure PNG export options
                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the modified drawing as a PNG image
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"Export completed successfully: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}