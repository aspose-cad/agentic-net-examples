using System;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadPolylineLayerExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the source DWG/DXF file
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path for the modified output file
                    string outputPath = "output.dwg";
                    // Desired custom layer name
                    string customLayerName = "MyCustomLayer";

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast the generic Image to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Iterate over top‑level drawing entities
                        foreach (CadEntityBase entity in cadImage.Entities)
                        {
                            // Check if the entity is a lightweight polyline
                            if (entity is CadLwPolyline polyline)
                            {
                                // Assign the custom layer name to the polyline
                                polyline.LayerName = customLayerName;
                                // If needed, you can also modify other polyline properties here
                            }
                        }

                        // Optionally, also inspect block definitions for polylines inside blocks
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            foreach (CadEntityBase blockEntity in block.Entities)
                            {
                                if (blockEntity is CadLwPolyline polylineInBlock)
                                {
                                    polylineInBlock.LayerName = customLayerName;
                                }
                            }
                        }

                        // Save the modified drawing back to DWG format
                        cadImage.Save(outputPath);
                    }

                    Console.WriteLine("Polyline layer updated and saved to: " + outputPath);
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}