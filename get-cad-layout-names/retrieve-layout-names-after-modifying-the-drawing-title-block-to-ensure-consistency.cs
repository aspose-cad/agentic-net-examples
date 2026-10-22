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
            Console.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                if (image is not CadImage cadImage)
                {
                    Console.WriteLine("Unable to load CAD image.");
                    return;
                }

                // Modify the title block (example: update text entities inside a block named "TitleBlock")
                foreach (CadBlockEntity blockEntity in cadImage.BlockEntities.ValuesTyped)
                {
                    if (string.Equals(blockEntity.Name, "TitleBlock", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (CadEntityBase entity in blockEntity.Entities)
                        {
                            if (entity is CadText textEntity)
                            {
                                textEntity.DefaultValue = "Updated Title Block Text";
                            }
                        }
                    }
                }

                // Retrieve and display layout names after the modification
                Console.WriteLine("Layout names in the drawing:");
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    Console.WriteLine(layout.LayoutName);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}