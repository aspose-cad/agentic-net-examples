using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace EditCadHyperlinks
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "sample_input.dwg";
            const string outputPath = "sample_output.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    const string oldHyperlink = "http://old.example.com";
                    const string newHyperlink = "http://new.example.com";

                    // Process top‑level entities.
                    ProcessEntityCollection(cadImage.Entities, cadImage, oldHyperlink, newHyperlink);

                    // Process block definitions.
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        ProcessEntityCollection(block.Entities, cadImage, oldHyperlink, newHyperlink);
                    }

                    try
                    {
                        cadImage.Save(outputPath);
                        Console.WriteLine($"File saved to {outputPath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.WriteLine($"Error saving file: {saveEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }

        private static void ProcessEntityCollection(
            IEnumerable<CadEntityBase> entities,
            CadImage cadImage,
            string oldHyperlink,
            string newHyperlink)
        {
            foreach (CadEntityBase entity in entities)
            {
                // Verify current hyperlink before changing.
                if (!string.IsNullOrEmpty(entity.Hyperlink))
                {
                    Console.WriteLine($"Entity type {entity.GetType().Name} has hyperlink: {entity.Hyperlink}");
                }

                if (!string.IsNullOrEmpty(entity.Hyperlink) &&
                    entity.Hyperlink.Equals(oldHyperlink, StringComparison.OrdinalIgnoreCase))
                {
                    entity.Hyperlink = newHyperlink;
                    Console.WriteLine($"Updated hyperlink on entity of type {entity.GetType().Name}.");
                }

                // Handle nested block inserts.
                if (entity is CadInsertObject insert)
                {
                    if (cadImage.BlockEntities.TryGetValue(insert.Name, out CadBlockEntity block))
                    {
                        ProcessEntityCollection(block.Entities, cadImage, oldHyperlink, newHyperlink);
                    }
                }
            }
        }
    }
}