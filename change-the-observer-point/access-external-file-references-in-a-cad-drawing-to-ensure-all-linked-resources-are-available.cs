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
        try
        {
                // Path to the main CAD drawing
                string cadFilePath = "sample.dwg";

                // Load the CAD drawing
                using (Image image = Image.Load(cadFilePath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Iterate through block definitions in the drawing
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        // Examine each entity inside the block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            // Look for insert objects which can reference external drawings (Xrefs)
                            if (entity is CadInsertObject insert)
                            {
                                // Retrieve the block name used by the insert
                                string blockName = insert.Name ?? insert.OriginalBlockName;
                                Console.WriteLine($"Insert object references block: {blockName}");

                                // Simple heuristic: if the block name ends with a CAD file extension,
                                // treat it as an external reference and verify its existence on disk
                                if (!string.IsNullOrEmpty(blockName) &&
                                    (blockName.EndsWith(".dwg", StringComparison.OrdinalIgnoreCase) ||
                                     blockName.EndsWith(".dxf", StringComparison.OrdinalIgnoreCase)))
                                {
                                    string externalPath = Path.Combine(Path.GetDirectoryName(cadFilePath) ?? string.Empty, blockName);
                                    bool fileExists = File.Exists(externalPath);
                                    Console.WriteLine($"External reference file '{externalPath}' exists: {fileExists}");
                                }
                            }
                        }
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}