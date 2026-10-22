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
        // Path to the input CAD file
        string cadFilePath = "input.dwg";

        // Ensure the CAD file exists; if not, create a placeholder empty file to avoid runtime error
        if (!File.Exists(cadFilePath))
        {
            try
            {
                File.WriteAllBytes(cadFilePath, new byte[0]);
                Console.WriteLine($"Placeholder CAD file created at '{cadFilePath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create placeholder CAD file: {ex.Message}");
                return;
            }
        }

        // Consolidated list for non‑empty external file paths
        List<string> externalFilePaths = new List<string>();

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(cadFilePath))
            {
                CadImage cadImage = (CadImage)image;

                // Iterate through block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    // Iterate through entities inside each block
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        // Look for insert objects that may reference external blocks/files
                        if (entity is CadInsertObject insertObject)
                        {
                            // Use the Name property as the external reference (if present)
                            string path = insertObject.Name;
                            if (!string.IsNullOrWhiteSpace(path) && !externalFilePaths.Contains(path))
                            {
                                externalFilePaths.Add(path);
                            }
                        }
                    }
                }
            }

            // Output the collected external file paths preserving encounter order
            Console.WriteLine("External file paths found:");
            foreach (string path in externalFilePaths)
            {
                Console.WriteLine(path);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}