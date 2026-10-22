using Aspose.CAD.FileFormats.Cad.CadTables;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Retrieve layer information (CadImage.Tables is not available in the current API)
                Console.WriteLine("Layers:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    // CadLayerTable provides Name, ColorId, Flags, PlotFlag
                    Console.WriteLine($"  Name: {layer.Name}, ColorId: {layer.ColorId}, Flags: {layer.Flags}, PlotFlag: {layer.PlotFlag}");
                }

                // Note: Other tables such as LinetypeTable, StyleTable, DimStyleTable are not exposed
                // via the current Aspose.CAD API. Access to those tables is unavailable.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}