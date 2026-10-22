using Aspose.CAD;
using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.ImageOptions;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Load the CAD drawing
                string inputPath = "input.dxf";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Retrieve all CadHatch entities and store them in a list
                    List<CadHatch> hatchEntities = cadImage.Entities
                        .OfType<CadHatch>()
                        .ToList();

                    // Example usage: display the number of hatches found
                    Console.WriteLine($"Number of hatch entities: {hatchEntities.Count}");
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}