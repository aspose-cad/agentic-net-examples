using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD;
using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System.IO;

class Program
{
    static void Main()
    {
        // Path to the CAD file
        string inputPath = "input.dxf";

        
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }
// Load the CAD drawing
        CadImage cadImage = (CadImage)Image.Load(inputPath);

        // Iterate through each layout in the drawing
        foreach (KeyValuePair<string, CadLayout> layoutEntry in cadImage.Layouts)
        {
            CadLayout layout = layoutEntry.Value;

            // Select only CadText entities from the layout's entities
            IEnumerable<CadEntityBase> textEntities = layout.Entities
                .Where(entity => entity.TypeName == CadEntityTypeName.TEXT);

            // Output the text strings
            foreach (CadEntityBase entity in textEntities)
            {
                CadText cadText = (CadText)entity;
                Console.WriteLine($"Layout: {layout.LayoutName}, Text: {cadText.DefaultValue}");
            }
        }
    }
}