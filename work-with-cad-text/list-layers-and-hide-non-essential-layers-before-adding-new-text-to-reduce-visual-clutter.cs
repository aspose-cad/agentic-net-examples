using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // List all layers with visibility status
                Console.WriteLine("Layers in the drawing:");
                foreach (CadLayerTable layerObj in cadImage.Layers)
                {
                    if (layerObj is CadLayerTable layer)
                    {
                        bool isVisible = layer.ColorId >= 0;
                        Console.WriteLine($"- {layer.Name} (Visible: {isVisible})");
                    }
                }

                // Hide non‑essential layers (those whose name does not contain "Essential")
                foreach (CadLayerTable layerObj in cadImage.Layers)
                {
                    if (layerObj is CadLayerTable layer)
                    {
                        if (!layer.Name.Contains("Essential", StringComparison.OrdinalIgnoreCase))
                        {
                            // Off layer by setting a negative ColorId
                            layer.ColorId = -1;
                        }
                    }
                }

                // Determine SoftOwner for the new text (use first layout's block record handle if available)
                string softOwner = string.Empty;
                var firstLayout = cadImage.Layouts.Values.FirstOrDefault();
                if (firstLayout != null)
                {
                    softOwner = firstLayout.BlockTableRecordHandle;
                }

                // Add a new text entity
                CadText newText = new CadText
                {
                    DefaultValue = "Hello Aspose.CAD",
                    FirstAlignment = new Cad3DPoint(0, 0, 0),
                    TextHeight = 2.5,
                    LayerName = "EssentialLayer",
                    SoftOwner = softOwner
                };

                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { newText }).ToArray();

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
                Console.WriteLine($"Modified drawing saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}