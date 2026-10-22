using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"File '{inputPath}' not found.");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                string layoutName = "Layout1";

                CadLayout targetLayout = null;
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    if (layout.LayoutName.Equals(layoutName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetLayout = layout;
                        break;
                    }
                }

                if (targetLayout == null)
                {
                    Console.WriteLine($"Layout '{layoutName}' not found.");
                    return;
                }

                IEnumerable<CadEntityBase> entities = targetLayout.Entities;

                foreach (CadEntityBase entity in entities)
                {
                    string typeName = entity.TypeName.ToString();
                    string layerName = entity.LayerName;
                    Console.WriteLine($"Entity Type: {typeName}, Layer: {layerName}");
                    // Insert hyperlink processing logic here
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}