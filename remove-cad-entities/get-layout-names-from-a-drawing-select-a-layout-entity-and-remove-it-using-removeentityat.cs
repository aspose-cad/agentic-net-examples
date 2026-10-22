using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace CadLayoutRemoval
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input DWG/DXF file path
                string inputPath = "input.dwg";

                // Output file path
                string outputPath = "output.dwg";

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine("Input file not found: " + inputPath);
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // -----------------------------------------------------------------
                    // 1. Retrieve and display layout names
                    // -----------------------------------------------------------------
                    Console.WriteLine("Available layouts:");
                    foreach (CadLayout layout in cadImage.Layouts.Values)
                    {
                        Console.WriteLine("- " + layout.LayoutName);
                    }

                    // -----------------------------------------------------------------
                    // 2. Select the first layout (if any) and remove its first entity
                    // -----------------------------------------------------------------
                    CadLayout firstLayout = cadImage.Layouts.Values.FirstOrDefault();
                    if (firstLayout != null)
                    {
                        // Find the block entity that corresponds to the selected layout
                        CadBlockEntity targetBlock = null;
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            if (block.Name == firstLayout.LayoutName)
                            {
                                targetBlock = block;
                                break;
                            }
                        }

                        if (targetBlock != null && targetBlock.Entities != null && targetBlock.Entities.Any())
                        {
                            // Convert the entity collection to a list for removal
                            List<CadEntityBase> entityList = targetBlock.Entities.ToList();

                            // Remove the first entity (index 0)
                            entityList.RemoveAt(0);

                            // Assign the modified collection back to the block
                            targetBlock.Entities = entityList;

                            Console.WriteLine($"Removed first entity from layout '{firstLayout.LayoutName}'.");
                        }
                        else
                        {
                            Console.WriteLine("No removable entities found in the selected layout.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No layouts found in the drawing.");
                    }

                    // -----------------------------------------------------------------
                    // 3. Save the modified drawing preserving editable objects
                    // -----------------------------------------------------------------
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };

                    cadImage.Save(outputPath);
                    Console.WriteLine("Modified drawing saved to: " + outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}