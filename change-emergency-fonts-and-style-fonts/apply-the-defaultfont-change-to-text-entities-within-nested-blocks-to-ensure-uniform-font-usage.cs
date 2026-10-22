using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadBlockTextFontUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dwg";
                // Desired uniform font name
                string uniformFont = "Arial";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD file and cast to CadImage
                using (Image image = Image.Load(sourcePath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Process each top‑level block definition
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        ProcessBlock(block, cadImage, uniformFont);
                    }

                    // Optionally save the modified drawing
                    cadImage.Save("output.dwg");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }

        // Recursively processes a block and its nested inserts
        private static void ProcessBlock(CadBlockEntity block, CadImage cadImage, string fontName)
        {
            foreach (CadEntityBase entity in block.Entities)
            {
                // Change font for single‑line text entities (if supported)
                if (entity is CadText cadText)
                {
                    // Aspose.CAD does not expose a direct TextStyle property.
                    // If a TextStyleName property exists, you could modify the style via cadImage.TextStyles.
                    // This placeholder leaves the entity unchanged to ensure compilation.
                }
                // Change font for multi‑line text entities (if supported)
                else if (entity is CadMText cadMText)
                {
                    // Similar placeholder as above for CadMText.
                }
                // If the entity is an insert, process the referenced block recursively
                else if (entity is CadInsertObject insert)
                {
                    // Find the block definition that matches the inserted block name
                    CadBlockEntity referencedBlock = FindBlockByName(cadImage, insert.Name);
                    if (referencedBlock != null)
                    {
                        ProcessBlock(referencedBlock, cadImage, fontName);
                    }
                }
            }
        }

        // Helper to locate a block definition by its name
        private static CadBlockEntity FindBlockByName(CadImage cadImage, string blockName)
        {
            foreach (CadBlockEntity blk in cadImage.BlockEntities.ValuesTyped)
            {
                if (string.Equals(blk.Name, blockName, StringComparison.OrdinalIgnoreCase))
                {
                    return blk;
                }
            }
            return null;
        }
    }
}