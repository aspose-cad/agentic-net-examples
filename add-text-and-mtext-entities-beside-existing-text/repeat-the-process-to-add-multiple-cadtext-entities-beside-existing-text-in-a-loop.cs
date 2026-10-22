using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AddTextBesideExisting
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input and output file paths
                    string inputPath = "input.dwg";
                    string outputPath = "output.dwg";

                    // Verify that the input file exists
                    if (!File.Exists(inputPath))
                    {
                        Console.WriteLine($"Input file not found: {inputPath}");
                        return;
                    }

                    // Load the CAD file
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Iterate over all block definitions in the drawing
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            // Collect new CadText entities to add after the current iteration
                            List<CadText> textsToAdd = new List<CadText>();

                            // Examine each entity inside the block
                            foreach (CadEntityBase entity in block.Entities)
                            {
                                // Process only CadText entities
                                if (entity is CadText existingText)
                                {
                                    // Add three new text entities beside the existing one
                                    for (int i = 1; i <= 3; i++)
                                    {
                                        CadText newText = new CadText();

                                        // Position the new text with an X offset
                                        newText.FirstAlignment = new Cad3DPoint(
                                            existingText.FirstAlignment.X + i * 10.0,
                                            existingText.FirstAlignment.Y,
                                            existingText.FirstAlignment.Z);

                                        // Set the text string (append an index for distinction)
                                        newText.DefaultValue = existingText.DefaultValue + $"_{i}";

                                        textsToAdd.Add(newText);
                                    }
                                }
                            }

                            // Append the newly created text entities to the block
                            foreach (CadText txt in textsToAdd)
                            {
                                block.AddEntity(txt);
                            }
                        }

                        // Save the modified drawing
                        cadImage.Save(outputPath);
                        Console.WriteLine($"Modified CAD file saved to: {outputPath}");
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