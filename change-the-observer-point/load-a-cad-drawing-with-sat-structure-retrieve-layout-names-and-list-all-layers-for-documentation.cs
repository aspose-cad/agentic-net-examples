using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the input SAT CAD file
                string inputPath = "input.sat";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the generic Image to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Retrieve and display layout names
                    Console.WriteLine("Layouts:");
                    foreach (KeyValuePair<string, CadLayout> kvp in cadImage.Layouts)
                    {
                        CadLayout layout = kvp.Value;
                        Console.WriteLine($"- {layout.LayoutName}");
                    }

                    // Retrieve and display layer information
                    Console.WriteLine("Layers:");
                    foreach (CadLayerTable layerObj in cadImage.Layers)
                    {
                        CadLayerTable layer = layerObj as CadLayerTable;
                        if (layer != null)
                        {
                            Console.WriteLine($"- Name: {layer.Name}, ColorId: {layer.ColorId}, Flags: {layer.Flags}, PlotFlag: {layer.PlotFlag}");
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