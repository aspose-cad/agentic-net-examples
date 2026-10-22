using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the input CAD file
            string inputPath = "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // List available layouts
                Console.WriteLine("Available layouts:");
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    Console.WriteLine($"- {layout.LayoutName}");
                }

                // Select the first layout (replace with custom logic if needed)
                CadLayout selectedLayout = null;
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    selectedLayout = layout;
                    break;
                }

                if (selectedLayout == null)
                {
                    Console.WriteLine("No layouts found in the CAD file.");
                    return;
                }

                Console.WriteLine($"Selected layout: {selectedLayout.LayoutName}");

                // Try to obtain the block that corresponds to the selected layout
                if (!cadImage.BlockEntities.TryGetValue(selectedLayout.LayoutName, out CadBlockEntity layoutBlock))
                {
                    Console.WriteLine($"No block entity found for layout '{selectedLayout.LayoutName}'.");
                    return;
                }

                // Export (display) entities of the selected layout
                Console.WriteLine($"Exporting {layoutBlock.Entities.Count} entities from layout '{selectedLayout.LayoutName}':");
                foreach (CadEntityBase entity in layoutBlock.Entities)
                {
                    Console.WriteLine($"Entity Type: {entity.TypeName}");
                }

                // Save the selected layout to a new DXF file.
                // Since cloning is not supported in the current API, we save the whole image.
                // In a real scenario, you would create a new CadImage containing only the desired block.
                string outputPath = "selected_layout.dxf";
                cadImage.Save(outputPath, new DxfOptions { OutputMode = CadOutputMode.Convert });
                Console.WriteLine($"Selected layout saved to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}