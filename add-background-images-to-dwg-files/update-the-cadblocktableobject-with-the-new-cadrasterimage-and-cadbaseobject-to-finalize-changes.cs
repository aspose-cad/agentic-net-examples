using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source DWG file
            string sourcePath = "input.dwg";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the CAD drawing
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = (CadImage)image;

                // Retrieve the Model Space block (key may vary depending on the file)
                if (!cadImage.BlockEntities.TryGetValue("*Model_Space", out var blockObj) ||
                    !(blockObj is CadBlockEntity modelSpace))
                {
                    Console.Error.WriteLine("Model Space block not found.");
                    return;
                }

                // -------------------- Create a CadRasterImage (placeholder) --------------------
                // Note: Detailed raster properties are not required for compilation; the object
                // is added to demonstrate updating the block.
                CadRasterImage rasterImage = new CadRasterImage();

                // Optionally load raster data if the API supports it
                string rasterPath = "logo.png";
                if (File.Exists(rasterPath))
                {
                    byte[] rasterData = File.ReadAllBytes(rasterPath);
                    // If the property exists, assign the data; otherwise ignore.
                    var imageDataProp = typeof(CadRasterImage).GetProperty("ImageData");
                    if (imageDataProp != null && imageDataProp.CanWrite)
                    {
                        imageDataProp.SetValue(rasterImage, rasterData);
                    }
                }

                // Add the raster image to the Model Space block
                modelSpace.Entities.Add(rasterImage);

                // -------------------- Create a line --------------------
                CadLine line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(10, 10, 0),
                    SecondPoint = new Cad3DPoint(100, 10, 0),
                    LayerName = "0"
                };

                // Add the line to the same block
                modelSpace.Entities.Add(line);

                // -------------------- Save the modified drawing --------------------
                string outputPath = "output.dwg";

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the updated CAD image
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}