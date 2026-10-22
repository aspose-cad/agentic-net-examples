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
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string? outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Iterate through each layer
                foreach (CadLayerTable layerRecord in cadImage.Layers)
                {
                    bool containsText = false;

                    // Check top‑level entities for text on this layer
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity.LayerName == layerRecord.Name)
                        {
                            if (entity is CadText cadText)
                            {
                                containsText = true;
                                cadText.TextHeight *= 2;
                            }
                            else if (entity is CadMText cadMText)
                            {
                                containsText = true;
                                cadMText.InitialTextHeight *= 2;
                            }
                        }
                    }

                    // Check entities inside block definitions for text on this layer
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (CadEntityBase blockEntity in block.Entities)
                        {
                            if (blockEntity.LayerName == layerRecord.Name)
                            {
                                if (blockEntity is CadText cadText)
                                {
                                    containsText = true;
                                    cadText.TextHeight *= 2;
                                }
                                else if (blockEntity is CadMText cadMText)
                                {
                                    containsText = true;
                                    cadMText.InitialTextHeight *= 2;
                                }
                            }
                        }
                    }

                    // Output layer information
                    Console.WriteLine($"Layer: {layerRecord.Name}, Contains Text: {containsText}");
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}