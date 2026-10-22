using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for input CAD file and output image
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputPath = "output.png";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Iterate over top‑level entities and process MText objects
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadMText existingMText)
                    {
                        // Position the new MText to the right of the existing one
                        double offset = 10.0; // distance to shift right
                        var newPosition = new Cad3DPoint(
                            existingMText.InsertionPoint.X + offset,
                            existingMText.InsertionPoint.Y,
                            existingMText.InsertionPoint.Z);

                        // Create a new MText entity based on the existing one
                        var newMText = new CadMText
                        {
                            Text = existingMText.Text + " Copy",
                            InsertionPoint = newPosition,
                            InitialTextHeight = existingMText.InitialTextHeight,
                            RotationAngleRad = existingMText.RotationAngleRad,
                            LayerName = existingMText.LayerName
                        };

                        // Add the new entity to the drawing
                        cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { newMText }).ToArray();

                        Console.WriteLine($"Added new MText on layer '{newMText.LayerName}' at ({newPosition.X}, {newPosition.Y})");
                    }
                }

                // Prepare PNG save options with rasterization settings
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200
                    }
                };

                // Save the modified drawing as PNG
                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}