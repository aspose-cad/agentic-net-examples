using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;
using Aspose.CAD.FileFormats.Cad;

namespace CadObserverPointExample
{
    class Program
    {
        static void Main()
        {
            const string inputPath = "input.dwg";
            const string outputPath = "output.obj";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the source CAD file (DWG, DXF, etc.)
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the generic Image to CadImage to access CAD‑specific members
                    CadImage cadImage = (CadImage)image;

                    // Configure rasterization options that will be used for OBJ export
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // ObserverPoint defines view rotation angles (X, Y, Z) in degrees.
                        // The SDK does not expose a direct distance parameter for zoom control.
                        // Zoom can be indirectly affected via LineScale or page size.
                        ObserverPoint = new ObserverPoint(30f, 45f, 0f),

                        // Adjust LineScale to influence the apparent zoom level.
                        // Larger values make the model appear smaller (zoomed out) and vice versa.
                        LineScale = 2.0f,

                        // Optional: define page size for the export (units are points)
                        PageWidth = 1000f,
                        PageHeight = 1000f
                    };

                    // Create OBJ save options and assign the rasterization settings
                    ObjOptions objOptions = new ObjOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Export the CAD drawing to OBJ format
                    cadImage.Save(outputPath, objOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}