using System;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;
using System.IO;

namespace CadLineWeightAdjustment
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input and output file paths
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "output.dwg";

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD object model
                        CadImage cadImage = (CadImage)image;

                        // Iterate over top‑level drawing entities
                        foreach (CadEntityBase entity in cadImage.Entities)
                        {
                            // Retrieve current line weight
                            int currentWeight = entity.LineWeight;

                            // Apply conditional logic to modify line weight
                            if (currentWeight < 20)
                            {
                                entity.LineWeight = 20;
                            }
                            else if (currentWeight > 100)
                            {
                                entity.LineWeight = 100;
                            }
                        }

                        // Save the modified drawing back to DWG format
                        cadImage.Save(outputPath);
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