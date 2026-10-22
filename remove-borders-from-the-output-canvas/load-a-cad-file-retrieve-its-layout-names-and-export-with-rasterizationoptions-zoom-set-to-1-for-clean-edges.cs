using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Define input CAD file path
            string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string sourceFile = Path.Combine(dataDir, "sample.dwg");

            // Verify source file exists
            if (!File.Exists(sourceFile))
            {
                Console.WriteLine($"Source CAD file not found: {sourceFile}");
                return;
            }

            // Load CAD image
            using (Image image = Image.Load(sourceFile))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Retrieve and display layout names
                Console.WriteLine("Available layouts:");
                foreach (var layout in cadImage.Layouts.Values)
                {
                    Console.WriteLine("- " + layout.LayoutName);
                }

                // Prepare output directory
                string outDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                Directory.CreateDirectory(outDir);
                string outPath = Path.Combine(outDir, "exported.png");

                // Configure rasterization options with Zoom = 1
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Zoom = 1
                };

                // Optionally select the first layout for export if layouts exist
                if (cadImage.Layouts.Count > 0)
                {
                    var enumerator = cadImage.Layouts.Values.GetEnumerator();
                    if (enumerator.MoveNext())
                    {
                        rasterOptions.Layouts = new[] { enumerator.Current.LayoutName };
                    }
                }

                // Set up PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the CAD drawing to PNG
                cadImage.Save(outPath, pngOptions);
                Console.WriteLine($"Exported image saved to: {outPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}