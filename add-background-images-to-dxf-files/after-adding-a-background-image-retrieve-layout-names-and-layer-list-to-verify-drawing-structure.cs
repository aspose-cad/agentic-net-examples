using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = (CadImage)image;

                // TODO: Add background image handling here (placeholder)
                // Example: cadImage.BackgroundImage = Image.Load("background.png");

                // Retrieve and display layout names
                Console.WriteLine("Layouts:");
                foreach (var layoutEntry in cadImage.Layouts)
                {
                    CadLayout layout = layoutEntry.Value;
                    // Layout name property may be called LayoutName or Name; adjust as needed
                    string layoutName = layout.GetType().GetProperty("LayoutName")?.GetValue(layout) as string
                                        ?? layout.GetType().GetProperty("Name")?.GetValue(layout) as string
                                        ?? "Unnamed Layout";
                    Console.WriteLine($"- {layoutName}");
                }

                // Retrieve and display layer names
                Console.WriteLine("Layers:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    Console.WriteLine($"- {layer.Name}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}