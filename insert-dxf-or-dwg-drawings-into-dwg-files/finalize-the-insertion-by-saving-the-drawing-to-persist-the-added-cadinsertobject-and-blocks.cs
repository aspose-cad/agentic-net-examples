using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        string sourcePath = "input/source.dwg";
        string insertPath = "input/insert.dxf";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "result.dwg");

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        if (!File.Exists(insertPath))
        {
            Console.Error.WriteLine($"Insert file not found: {insertPath}");
            return;
        }

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the target DWG file
            using (Image sourceImage = Image.Load(sourcePath))
            {
                CadImage cadSource = (CadImage)sourceImage;

                // Load the drawing to be inserted and obtain its first block name (if any)
                string insertBlockName = Path.GetFileNameWithoutExtension(insertPath);
                using (Image insertImage = Image.Load(insertPath))
                {
                    CadImage cadInsert = (CadImage)insertImage;

                    // Copy block definitions from the insert drawing into the source drawing
                    foreach (CadBlockEntity block in cadInsert.BlockEntities.ValuesTyped)
                    {
                        // Use the block's own name if available
                        if (!string.IsNullOrEmpty(block.Name))
                        {
                            insertBlockName = block.Name;
                        }

                        // Add block to source if it does not already exist
                        if (!cadSource.BlockEntities.ContainsKey(insertBlockName))
                        {
                            cadSource.BlockEntities.Add(insertBlockName, block);
                        }
                    }
                }

                // Create an insert object referencing the block from the insert drawing
                CadInsertObject insertObject = new CadInsertObject
                {
                    InsertionPoint = new Cad3DPoint(0.0, 0.0, 0.0),
                    Name = insertBlockName,
                    ScaleX = 1.0,
                    ScaleY = 1.0,
                    ScaleZ = 1.0,
                    RotationAngle = 0.0
                };

                // Append the insert object to the entity collection
                cadSource.Entities = cadSource.Entities
                    .Concat(new CadEntityBase[] { insertObject })
                    .ToArray();

                // Optional diagnostics: list layers
                Console.WriteLine("Layers in target drawing:");
                foreach (CadLayerTable layer in cadSource.Layers)
                {
                    string status = (layer.ColorId < 0 ? "Off" : "On") +
                                    ((layer.Flags & 1) != 0 ? ", Frozen" : "") +
                                    ((layer.Flags & 4) != 0 ? ", Locked" : "");
                    Console.WriteLine($"- {layer.Name} (ColorId={layer.ColorId}, {status})");
                }

                // Optional diagnostics: list block definitions after insertion
                Console.WriteLine("Block definitions after insertion:");
                foreach (CadBlockEntity block in cadSource.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"- {block.Name}");
                }

                // Save the modified drawing
                cadSource.Save(outputPath);
                Console.WriteLine($"Modified drawing saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}