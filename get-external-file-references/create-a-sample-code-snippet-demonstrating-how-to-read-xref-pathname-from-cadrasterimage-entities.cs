using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace ExternalFileReferencesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // -----------------------------------------------------------------
            // 1. Define input CAD file and output report file.
            // -----------------------------------------------------------------
            string sourcePath = "input.dwg";
            string reportPath = "ExternalReferences.txt";

            // -----------------------------------------------------------------
            // 2. Verify that the source file exists before attempting to load it.
            // -----------------------------------------------------------------
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // -----------------------------------------------------------------
            // 3. Load the CAD drawing.
            // -----------------------------------------------------------------
            using (Image image = Image.Load(sourcePath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load the file as a CAD image. Verify the format.");
                    return;
                }

                // -----------------------------------------------------------------
                // 4. Prepare a collection that will hold information about each
                //    external reference discovered in the drawing.
                // -----------------------------------------------------------------
                List<string> externalReferenceLines = new List<string>();

                // -----------------------------------------------------------------
                // 5. Iterate over top‑level entities. Insert objects (CadInsertObject)
                //    represent block insertions, which may be external references (Xrefs).
                // -----------------------------------------------------------------
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadInsertObject insert)
                    {
                        string blockName = insert.Name ?? insert.OriginalBlockName ?? "<Unnamed>";

                        string line = $"Block: {blockName}, " +
                                      $"Insertion: ({insert.InsertionPoint.X:F3}, {insert.InsertionPoint.Y:F3}, {insert.InsertionPoint.Z:F3}), " +
                                      $"Scale: ({insert.ScaleX:F3}, {insert.ScaleY:F3}, {insert.ScaleZ:F3}), " +
                                      $"Rotation: {insert.RotationAngle:F2}°";

                        externalReferenceLines.Add(line);
                    }
                }

                // -----------------------------------------------------------------
                // 6. Report results.
                // -----------------------------------------------------------------
                if (externalReferenceLines.Count == 0)
                {
                    Console.WriteLine("No insert objects (potential Xrefs) were detected in the drawing.");
                }
                else
                {
                    try
                    {
                        File.WriteAllLines(reportPath, externalReferenceLines);
                        Console.WriteLine($"External reference report successfully written to \"{reportPath}\".");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to write the report file. Exception: {ex.Message}");
                    }

                    Console.WriteLine();
                    Console.WriteLine("=== External Reference Details ===");
                    foreach (string line in externalReferenceLines)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
        }

        // -----------------------------------------------------------------
        // Helper method to locate a block definition by name within the CadImage.
        // -----------------------------------------------------------------
        private static CadBlockEntity FindBlockByName(CadImage cadImage, string blockName)
        {
            foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
            {
                if (string.Equals(block.Name, blockName, StringComparison.OrdinalIgnoreCase))
                {
                    return block;
                }
            }
            return null;
        }
    }
}