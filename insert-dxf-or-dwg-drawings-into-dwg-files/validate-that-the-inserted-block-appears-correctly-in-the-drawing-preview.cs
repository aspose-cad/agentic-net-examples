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
            // Path to the CAD drawing file
            string cadFilePath = "sample.dwg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(cadFilePath))
            {
                Console.Error.WriteLine($"File not found: {cadFilePath}");
                return;
            }

            // Load the drawing
            using (Image image = Image.Load(cadFilePath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Name of the block we expect to be inserted
                string expectedBlockName = "MyBlock";

                // Flag to indicate whether the insert reference was found
                bool insertFound = false;

                // Search top‑level entities for an insert of the expected block
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadInsertObject insert)
                    {
                        if (string.Equals(insert.Name, expectedBlockName, StringComparison.OrdinalIgnoreCase))
                        {
                            insertFound = true;
                            Console.WriteLine($"Insert of block '{expectedBlockName}' found.");

                            // Verify that the block definition exists in the drawing
                            CadBlockEntity blockDefinition = null;
                            foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                            {
                                if (string.Equals(block.Name, expectedBlockName, StringComparison.OrdinalIgnoreCase))
                                {
                                    blockDefinition = block;
                                    break;
                                }
                            }

                            if (blockDefinition != null)
                            {
                                Console.WriteLine($"Block definition '{expectedBlockName}' contains {blockDefinition.Entities.Count} entities.");
                            }
                            else
                            {
                                Console.WriteLine($"Block definition '{expectedBlockName}' not found in BlockEntities.");
                            }

                            break; // Insert found, no need to continue scanning
                        }
                    }
                }

                if (!insertFound)
                {
                    Console.WriteLine($"Insert of block '{expectedBlockName}' not found in the drawing.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}