using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                // Paths for input and output drawings
                string inputPath = "input.dwg";
                string outputPath = "output.dwg";

                // Name of the layer to assign to polylines
                string targetLayerName = "MyLayer";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
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

                    // Set layer name for top‑level lightweight polylines
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity is CadLwPolyline polyline)
                        {
                            polyline.LayerName = targetLayerName;
                        }
                    }

                    // Set layer name for polylines inside block definitions
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (CadEntityBase blockEntity in block.Entities)
                        {
                            if (blockEntity is CadLwPolyline blockPolyline)
                            {
                                blockPolyline.LayerName = targetLayerName;
                            }
                        }
                    }

                    // Save the modified drawing
                    cadImage.Save(outputPath);
                }

                Console.WriteLine($"Modified drawing saved to: {outputPath}");
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}