using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerInspection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD file (DWG/DXF). Replace with actual file path or pass as argument.
                string cadFilePath = args.Length > 0 ? args[0] : "sample.dwg";

                // Guard against missing file.
                if (!File.Exists(cadFilePath))
                {
                    Console.Error.WriteLine($"File not found: {cadFilePath}");
                    return;
                }

                // Load the CAD file.
                using (Image image = Image.Load(cadFilePath))
                {
                    // Cast the loaded image to CadImage to access CAD-specific members.
                    CadImage cadImage = (CadImage)image;

                    // List all layers.
                    Console.WriteLine("All layers in the drawing:");
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        Console.WriteLine($"- Name: {layer.Name}, ColorId: {layer.ColorId}, Flags: {layer.Flags}, PlotFlag: {layer.PlotFlag}");
                    }

                    // Prioritize validation on visible layers.
                    Console.WriteLine();
                    Console.WriteLine("Visible layers (not off and not frozen):");
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        // Determine visibility according to SDK rules.
                        bool isOff = layer.ColorId < 0;
                        bool isFrozen = (layer.Flags & 1) != 0; // Bit 0 indicates frozen.

                        if (!isOff && !isFrozen)
                        {
                            Console.WriteLine($"- Name: {layer.Name}, ColorId: {layer.ColorId}, Flags: {layer.Flags}, PlotFlag: {layer.PlotFlag}");
                            // Place validation logic for the visible layer here.
                        }
                    }
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